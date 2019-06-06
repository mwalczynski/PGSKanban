using System.Collections.Generic;
using System.Threading.Tasks;
using PgsKanban.BusinessLogic.Responses;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Interfaces
{
    public interface IBoardService
    {
        UserBoardDto CreateBoard(AddBoardDto boardDto, string ownerId);
        UserBoardDto CreateFirstBoard(FirstBoardDto boardDto, string userId);
        ICollection<UserBoardDto> GetBoardsOfUser(string userId);
        QueryResponse<BoardDto> GetBoard(int id, string userId);
        bool HasUserBoards(string userId);
        BoardDto EditBoard(EditBoardDto boardDto, string userId);
        Task<MemberDto> ConnectUserWithBoard(AddMemberDto addMemberDto, string userId);
        UserBoardDto ChangeFavoriteState(int boardId, string userId);
        BoardDto DeleteBoard(int boardId, string userId);
        MembersDto GetMembersOfBoard(int boardId, string userId);
        MemberDto DeleteMember(int boardId, string memberId, string userId);
        QueryResponse<BoardDto> GetBoardByObfuscatedId(string obfuscatedId, string userId);
        QueryResponse<BoardDto> GetBoardByCardId(string cardObfuscatedId, string userId);
    }
}
