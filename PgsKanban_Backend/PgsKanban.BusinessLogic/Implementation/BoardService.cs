using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.BusinessLogic.Responses;
using PgsKanban.DataAccess.Interfaces;
using PgsKanban.DataAccess.Models;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Implementation
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IUserBoardRepository _userBoardRepository;
        private readonly ICardRepository _cardRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IObfuscator _obfuscator;

        public BoardService(IBoardRepository boardRepository, IUserBoardRepository userBoardRepository, UserManager<User> userManager, IMapper mapper, IObfuscator obfuscator, ICardRepository cardRepository)
        {
            _boardRepository = boardRepository;
            _userBoardRepository = userBoardRepository;
            _userManager = userManager;
            _mapper = mapper;
            _obfuscator = obfuscator;
            _cardRepository = cardRepository;
        }

        public UserBoardDto CreateBoard(AddBoardDto boardDto, string ownerId)
        {
            var board = _mapper.Map<Board>(boardDto);
            board.OwnerId = ownerId;
            board.Hash = Guid.NewGuid().ToString("N");
            var userBoard = _mapper.Map<UserBoard>(board);
            var addedUserBoard = _userBoardRepository.CreateUserBoard(userBoard);
            var result = _mapper.Map<UserBoardDto>(addedUserBoard);
            result.Board.ObfuscatedId = _obfuscator.Obfuscate(result.BoardId);
            return result;
        }

        public ICollection<UserBoardDto> GetBoardsOfUser(string userId)
        {
            var boards = _userBoardRepository.GetUserBoards(userId);
            var result = _mapper.Map<ICollection<UserBoard>, ICollection<UserBoardDto>>(boards);
            for (var i = 0; i < boards.Count; i++)
            {
                var element = result.ElementAt(i);
                element.IsOwner = boards.ElementAt(i).Board.OwnerId == userId;
                element.Board.ObfuscatedId = _obfuscator.Obfuscate(element.BoardId);
            }
            return result;
        }

        public QueryResponse<BoardDto> GetBoard(int id, string userId)
        {
            QueryResponse<BoardDto> result;
            if (!_boardRepository.BoardExists(id))
            {
                result = new QueryResponse<BoardDto>()
                {
                    ResponseDto = null,
                    HttpStatusCode = HttpStatusCode.NotFound
                };

                return result;
            }
            if (!_userBoardRepository.IsMember(id, userId))
            {
                result = new QueryResponse<BoardDto>()
                {
                    ResponseDto = null,
                    HttpStatusCode = HttpStatusCode.Forbidden
                };

                return result;
            }
            var userBoard = _userBoardRepository.GetUserBoard(id, userId);
            result = HandleGettingBoard(userId, userBoard);
            return result;
        }

        public QueryResponse<BoardDto> GetBoardByObfuscatedId(string obfuscatedId, string userId)
        {
            var id = _obfuscator.Deobfuscate(obfuscatedId);
            var result = GetBoard(id, userId);
            return result;
        }

        public QueryResponse<BoardDto> GetBoardByCardId(string cardObfuscatedId, string userId)
        {
            var cardId = _obfuscator.Deobfuscate(cardObfuscatedId);
            QueryResponse<BoardDto> result;
            if (!_cardRepository.CardExists(cardId))
            {
                result = new QueryResponse<BoardDto>()
                {
                    ResponseDto = null,
                    HttpStatusCode = HttpStatusCode.NotFound
                };

                return result;
            }

            var userBoard = _userBoardRepository.GetUserBoardByCardId(cardId, userId);
            if (userBoard == null)
            {
                result = new QueryResponse<BoardDto>()
                {
                    ResponseDto = null,
                    HttpStatusCode = HttpStatusCode.Forbidden
                };
                return result;
            }
            result = HandleGettingBoard(userId, userBoard);
            return result;
        }

        public UserBoardDto CreateFirstBoard(FirstBoardDto boardDto, string userId)
        {
            var userBoard = InitializeUserBoard(boardDto, userId);
            var addedUserBoard = _userBoardRepository.CreateUserBoard(userBoard);
            var result = _mapper.Map<UserBoardDto>(addedUserBoard);
            result.Board.ObfuscatedId = _obfuscator.Obfuscate(result.BoardId);
            return result;
        }

        public async Task<MemberDto> ConnectUserWithBoard(AddMemberDto addMemberDto, string userId)
        {
            if (!_boardRepository.IsOwner(addMemberDto.BoardId, userId) ||
                _userBoardRepository.IsMember(addMemberDto.BoardId, addMemberDto.MemberId) ||
                await _userManager.FindByIdAsync(addMemberDto.MemberId) == null)
            {
                return null;
            }

            UserBoard addedUserBoard;
            if (_userBoardRepository.GetUserBoard(addMemberDto.BoardId, addMemberDto.MemberId) == null)
            {
                var userBoard = new UserBoard
                {
                    BoardId = addMemberDto.BoardId,
                    UserId = addMemberDto.MemberId
                };
                addedUserBoard = _userBoardRepository.CreateUserBoard(userBoard);
            }
            else
            {
                addedUserBoard = _userBoardRepository.ChangeUserBoardState(addMemberDto.BoardId, addMemberDto.MemberId);
            }
            var result = _mapper.Map<MemberDto>(addedUserBoard.User);
            return result;
        }

        public UserBoardDto ChangeFavoriteState(int boardId, string userId)
        {
            if (!_userBoardRepository.IsMember(boardId, userId))
            {
                return null;
            }
            var userBoard = _userBoardRepository.UpdatePriorityOfBoard(boardId, userId);
            var result = _mapper.Map<UserBoardDto>(userBoard);
            return result;
        }

        public BoardDto DeleteBoard(int boardId, string userId)
        {
            if (!_userBoardRepository.IsMember(boardId, userId))
            {
                return null;
            }
            Board board;
            if (_boardRepository.IsOwner(boardId, userId))
            {
                board = _boardRepository.DeleteBoard(boardId);
            }
            else
            {
                var userboard = _userBoardRepository.ChangeUserBoardState(boardId, userId);
                board = userboard.Board;
            }
            var result = _mapper.Map<BoardDto>(board);
            return result;
        }

        public MembersDto GetMembersOfBoard(int boardId, string userId)
        {
            if (!_userBoardRepository.IsMember(boardId, userId))
            {
                return null;
            }
            var allMembers = _userBoardRepository.GetMembersOfBoard(boardId);
            var members = _mapper.Map<ICollection<MemberDto>>(allMembers.Where(x => x.Id != userId).ToList());
            var result = new MembersDto
            {
                Members = members,
                IsAllowedToModify = _boardRepository.IsOwner(boardId, userId)
            };
            return result;
        }

        public MemberDto DeleteMember(int boardId, string memberId, string userId)
        {
            if (!_userBoardRepository.IsMember(boardId, memberId) || !_boardRepository.IsOwner(boardId, userId))
            {
                return null;
            }
            var deletedUserBoard = _userBoardRepository.ChangeUserBoardState(boardId, memberId);
            var member = deletedUserBoard.User;
            var result = _mapper.Map<MemberDto>(member);
            return result;
        }

        public bool HasUserBoards(string userId)
        {
            var anyBoards = _userBoardRepository.HasUserBoards(userId);
            return anyBoards;
        }

        public BoardDto EditBoard(EditBoardDto boardDto, string userId)
        {
            if (!_boardRepository.IsOwner(boardDto.Id, userId))
            {
                return null;
            }

            var board = _mapper.Map<Board>(boardDto);
            var editedBoard = _boardRepository.UpdateBoardName(board);
            var result = _mapper.Map<BoardDto>(editedBoard);
            return result;
        }

        private QueryResponse<BoardDto> HandleGettingBoard(string userId, UserBoard userBoard)
        {
            QueryResponse<BoardDto> result;
            _userBoardRepository.UpdateBoardTimeVisited(userBoard);
            var board = userBoard.Board;
            board.Lists = board.Lists.Where(x => !x.IsDeleted).ToList();
            foreach (var x in board.Lists)
            {
                x.Cards = x.Cards.Where(c => !c.IsDeleted).ToList();
            }
            var boardDto = _mapper.Map<Board, BoardDto>(board,
                opts => opts.AfterMap((src, dest) => dest.IsOwner = src.OwnerId == userId));
            boardDto.ObfuscatedId = _obfuscator.Obfuscate(boardDto.Id);

            foreach (var boardList in boardDto.Lists)
            {
                foreach (var card in boardList.Cards)
                {
                    card.ObfuscatedId = _obfuscator.Obfuscate(card.Id);
                }
            }

            result = new QueryResponse<BoardDto>
            {
                ResponseDto = boardDto,
                HttpStatusCode = HttpStatusCode.OK
            };
            return result;
        }

        private UserBoard InitializeUserBoard(FirstBoardDto boardDto, string userId)
        {
            var hash = Guid.NewGuid().ToString("N");
            var userBoard = new UserBoard
            {
                UserId = userId,
                Board = new Board
                {
                    Name = boardDto.BoardName,
                    OwnerId = userId,
                    Lists = new List<List>(),
                    Hash = hash
                }
            };
            var result = TryInitializeFirstList(boardDto, out List list);
            if (result)
            {
                userBoard.Board.Lists.Add(list);
            }
            return userBoard;
        }

        private bool TryInitializeFirstList(FirstBoardDto boardDto, out List list)
        {
            list = null;

            if (string.IsNullOrEmpty(boardDto.ListName))
            {
                return false;
            }

            list = new List
            {
                Name = boardDto.ListName,
                Cards = new List<Card>()
            };

            var result = TryInitializeFirstCard(boardDto, out Card card);
            if (result)
            {
                list.Cards.Add(card);
            }

            return true;
        }

        private bool TryInitializeFirstCard(FirstBoardDto boardDto, out Card card)
        {
            card = null;

            if (string.IsNullOrEmpty(boardDto.CardName))
            {
                return false;
            }
            card = new Card
            {
                Name = boardDto.CardName
            };
            return true;
        }
    }
}
