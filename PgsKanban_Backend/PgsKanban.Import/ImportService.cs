using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.DataAccess.Interfaces;
using PgsKanban.DataAccess.Models;
using PgsKanban.Dto;
using PgsKanban.Import.Dtos;

namespace PgsKanban.Import
{
    public class ImportService : IImportService
    {
        private const string COMMENT_CARD_ACTION = "commentCard";
        private readonly IUserRepository _userRepository;
        private readonly IUserBoardRepository _boardRepository;
        private readonly IObfuscator _obfuscator;
        private readonly IMapper _mapper;
        private Dictionary<string, (string UserId, bool IsExternal)> _alreadyFoundUsers;

        public ImportService(IUserRepository userRepository, IUserBoardRepository boardRepository, IMapper mapper, IObfuscator obfuscator)
        {
            _userRepository = userRepository;
            _boardRepository = boardRepository;
            _obfuscator = obfuscator;
            _mapper = mapper;
        }

        public ImportStatisticsDto ImportBoard(FileFormDto data, string userId)
        {
            try
            {
                var readedFile = ReadFile(data.File);
                var result = ImportBoardFromJson(userId, readedFile, data.BoardName);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ImportStatisticsDto ImportBoardFromJson(string userId, string readedFile, string boardName)
        {
            var importedBoardDto = JsonConvert.DeserializeObject<ImportedBoardDto>(readedFile);
            _alreadyFoundUsers = new Dictionary<string, (string, bool)>();

            var userBoard = CreateUserBoard(userId, importedBoardDto, boardName);
            var result = CreateImportStatistics(userBoard);
            return result;
        }

        private UserBoard CreateUserBoard(string userId, ImportedBoardDto importedBoardDto, string boardName)
        {
            var lists = CreateLists(importedBoardDto);

            var board = new Board
            {
                Name = boardName,
                OwnerId = userId,
                Lists = lists,
                Hash = Guid.NewGuid().ToString()
            };

            var userBoard = _boardRepository.CreateUserBoard(new UserBoard
            {
                Board = board,
                UserId = userId
            });

            return userBoard;
        }

        private List<List> CreateLists(ImportedBoardDto importedBoardDto)
        {
            var lists = importedBoardDto.Lists.Select(list => new List
            {
                Name = list.Name,
                Cards = CreateCards(importedBoardDto, list.Id)
            }).ToList();
            return lists;
        }

        private List<Card> CreateCards(ImportedBoardDto importedBoardDto, string importedListId)
        {
            var commentsData = GetCommentsData(importedBoardDto);

            var result = importedBoardDto.Cards.Where(card => card.ListId == importedListId)
                .Select(importedCard => new Card
                {
                    Name = importedCard.Name,
                    Description = importedCard.Description,
                    Comments = commentsData.Where(x => importedCard.Id == x.ImportedCardId).Select(x => x.Comment).ToList()
                })
                .ToList();
            return result;
        }

        private IList<ImportedCommentDataDto> GetCommentsData(ImportedBoardDto importedBoardDto)
        {
            var comments = importedBoardDto.Actions.Where(x => x.Type == COMMENT_CARD_ACTION).Select(action =>
            {
                var importedComment = action.Data.ToObject<ImportedCommentDto>();
                var comment = CreateComment(importedComment, action);

                return new ImportedCommentDataDto
                {
                    Comment = comment,
                    ImportedCardId = importedComment.Card.Id
                };
            });
            return comments.ToList();
        }

        private Comment CreateComment(ImportedCommentDto importedComment, ImportedActionDto action)
        {
            var comment = new Comment
            {
                Content = importedComment.Text,
                TimeCreated = action.Date,
            };
            comment = HandleCommentAuthor(action, comment);
            return comment;
        }

        private Comment HandleCommentAuthor(ImportedActionDto action, Comment comment)
        {
            if (_alreadyFoundUsers.ContainsKey(action.MemberCreator.FullName))
            {
                (var userId, var isExternal) = _alreadyFoundUsers[action.MemberCreator.FullName];
                if (isExternal)
                {
                    comment.ExternalUserId = userId;
                }
                else
                {
                    comment.UserId = userId;
                }

                return comment;
            }

            (var firstName, var lastName) = ParseNames(action.MemberCreator.FullName);
            var user = _userRepository.GetUserByFullName(firstName, lastName);

            if (user == null)
            {
                return HandleExternalUser(comment, firstName, lastName);
            }
            AddFoundedUserToDictionary(user.FirstName, user.LastName, user.Id, false);
            comment.UserId = user.Id;
            return comment;
        }

        private Comment HandleExternalUser(Comment comment, string firstName, string lastName)
        {
            var externalUser = _userRepository.GetExternalUserByFullName(firstName, lastName);
            if (externalUser == null)
            {
                externalUser = CreateExternalUser(firstName, lastName);
                comment.ExternalUserId = externalUser.Id;
            }
            else
            {
                comment.ExternalUserId = externalUser.Id;
            }

            AddFoundedUserToDictionary(externalUser.FirstName, externalUser.LastName, externalUser.Id, true);
            return comment;
        }

        private ExternalUser CreateExternalUser(string firstName, string lastName)
        {
            var externalUser = new ExternalUser
            {
                FirstName = firstName,
                LastName = lastName,
            };

            externalUser = _userRepository.AddExternalUser(externalUser);
            return externalUser;
        }

        private (string firstName, string lastName) ParseNames(string fullName)
        {
            var names = fullName.Split(' ');
            var firstname = names.FirstOrDefault();
            var lastname = names.Length > 1 ? names.LastOrDefault() : "";
            return (firstname, lastname);
        }

        private string ReadFile(IFormFile file)
        {
            string readedFile;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                readedFile = reader.ReadToEnd();
            }

            return readedFile;
        }

        private ImportStatisticsDto CreateImportStatistics(UserBoard userBoard)
        {
            var userBoardDto = _mapper.Map<UserBoardDto>(userBoard);
            userBoardDto.Board.ObfuscatedId = _obfuscator.Obfuscate(userBoard.BoardId);
            var result = new ImportStatisticsDto
            {
                ListsCount = userBoard.Board.Lists.Count,
                CardsCount = userBoard.Board.Lists.SelectMany(x => x.Cards).Count(),
                CommentsCount = userBoard.Board.Lists.SelectMany(x=>x.Cards).SelectMany(x=>x.Comments).Count(),
                UserBoard = userBoardDto
            };
            return result;
        }

        private void AddFoundedUserToDictionary(string firstName, string lastName, string id, bool isExternal)
        {
            _alreadyFoundUsers.Add($"{firstName} {lastName}".TrimEnd(), (id, isExternal));
        }
    }
}
