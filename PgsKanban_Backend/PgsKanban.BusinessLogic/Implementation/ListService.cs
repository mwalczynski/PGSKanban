using System.Collections.Generic;
using AutoMapper;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.DataAccess.Interfaces;
using PgsKanban.DataAccess.Models;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Implementation
{
    public class ListService : IListService
    {
        private readonly IListRepository _listRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IMapper _mapper;

        public ListService(IListRepository listRepository, IBoardRepository boardRepository, IMapper mapper)
        {
            _listRepository = listRepository;
            _boardRepository = boardRepository;
            _mapper = mapper;
        }

        public ListDto GetList(int id, string userId)
        {
            if (!_listRepository.IsMember(id, userId))
            {
                return null;
            }

            var list = _listRepository.GetList(id);
            var result = _mapper.Map<ListDto>(list);
            return result;
        }

        public ListDto CreateList(AddListDto listDto, string userId)
        {
            if (!_boardRepository.IsOwner(listDto.BoardId, userId))
            {
                return null;
            }

            var list = _mapper.Map<List>(listDto);
            var position = _listRepository.GetNumberOfListsInBoard(listDto.BoardId) + 1;
            list.Position = position;
            var addedList = _listRepository.CreateList(list);
            var result = _mapper.Map<ListDto>(addedList);
            return result;
        }

        public ListDto EditList(EditListDto listDto, string userId)
        {
            if (!_listRepository.IsOwner(listDto.Id, userId))
            {
                return null;
            }

            var list = _mapper.Map<List>(listDto);
            var editedList = _listRepository.UpdateListName(list);
            return _mapper.Map<ListDto>(editedList);
        }

        public ListDto EditListPosition(EditListPositionDto listDto, string userId)
        {
            if (!_listRepository.IsOwner(listDto.Id, userId))
            {
                return null;
            }

            var listToEdit = _listRepository.GetList(listDto.Id);
            if (listToEdit.BoardId != listDto.BoardId)
            {
                return null;
            }

            var listsWithPositionToUpdate = GetListsWithPositionToUpdate(listDto.BoardId, listToEdit.Position, listDto.NewPosition);
            listToEdit.Position = listDto.NewPosition;
            listsWithPositionToUpdate.Add(listToEdit);
            _listRepository.UpdateListsPosition(listsWithPositionToUpdate);

            var result = _mapper.Map<ListDto>(listToEdit);
            return result;
        }

        public ListDto DeleteList(DeleteListDto deleteListDto, string userId)
        {
            if (!_listRepository.IsOwner(deleteListDto.ListId, userId))
            {
                return null;
            }

            var list = _listRepository.GetList(deleteListDto.ListId);
            if (list == null)
            {
                return null;
            }
            var deletedList = _listRepository.DeleteList(list);

            var listsWithPositionToUpdate = GetCardsWithPositionToDecrease(deletedList.BoardId, deletedList.Position);
            _listRepository.UpdateListsPosition(listsWithPositionToUpdate);

            var result = _mapper.Map<ListDto>(deletedList);
            return result;
        }

        private ICollection<List> GetListsWithPositionToUpdate(int boardId, int oldPosition, int newPosition)
        {
            ICollection<List> listsWithPositionToUpdate;
            if (newPosition > oldPosition)
            {
                listsWithPositionToUpdate = _listRepository.GetListsInPositionRange(boardId, oldPosition, newPosition + 1);
                foreach (var list in listsWithPositionToUpdate)
                {
                    list.Position--;
                }
            }
            else
            {
                listsWithPositionToUpdate = _listRepository.GetListsInPositionRange(boardId, newPosition - 1, oldPosition);
                foreach (var list in listsWithPositionToUpdate)
                {
                    list.Position++;
                }
            }
            return listsWithPositionToUpdate;
        }

        private List<List> GetCardsWithPositionToDecrease(int boardId, int position)
        {
            var listsWithPositionToUpdate = _listRepository.GetListsWithGreaterPosition(boardId, position);
            foreach (var list in listsWithPositionToUpdate)
            {
                list.Position--;
            }
            return listsWithPositionToUpdate;
        }
    }
}
