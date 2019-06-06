using System;
using System.Linq;
using AutoMapper;
using PgsKanban.BusinessLogic.Services;
using PgsKanban.DataAccess.Models;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Config
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterUserDto, User>()
                    .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.Email));

                cfg.CreateMap<ExternalUserDto, User>()
                    .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.Email))
                    .ForMember(dest => dest.EmailConfirmed, opts => opts.MapFrom(src => true));

                cfg.CreateMap<User, UserProfileDto>()
                    .ForMember(
                        profile => profile.HashMail,
                        options => options.MapFrom(user => Md5Hasher.HashString(user.Email)));
                
                cfg.CreateMap<ExternalUser, UserProfileDto>()
                    .ForMember(
                        profile => profile.HashMail,
                        options => options.MapFrom(user => Guid.NewGuid().ToString("d")));

                cfg.CreateMap<User, SecureUserProfileDto>()
                    .IncludeBase<User, UserProfileDto>();

                cfg.CreateMap<User, MemberDto>()
                    .ForMember(
                        profile => profile.HashMail,
                        options => options.MapFrom(user => Md5Hasher.HashString(user.Email)));

                cfg.CreateMap<AddBoardDto, Board>();
                cfg.CreateMap<Board, BoardMiniatureDto>()
                    .ForMember(dest => dest.MembersCount,
                        opt => opt.MapFrom(src => src.Members.Count(x => !x.IsDeleted)));

                cfg.CreateMap<Board, BoardDto>();
                cfg.CreateMap<EditBoardDto, Board>();

                cfg.CreateMap<Board, UserBoard>()
                    .ForMember(dest => dest.Board,
                        opts => opts.MapFrom(src => src))
                    .ForMember(dest => dest.UserId,
                        opts => opts.MapFrom(src => src.OwnerId))
                    .ForMember(dest => dest.BoardId,
                        opts => opts.MapFrom(src => src.Id));

                cfg.CreateMap<UserBoard, UserBoardDto>();

                cfg.CreateMap<AddListDto, List>();
                cfg.CreateMap<EditListDto, List>();
                cfg.CreateMap<List, ListDto>();

                cfg.CreateMap<AddCardDto, Card>();
                cfg.CreateMap<EditCardNameDto, Card>();
                cfg.CreateMap<EditCardDescriptionDto, Card>();
                cfg.CreateMap<Card, CardDto>();
                cfg.CreateMap<Card, CardDetailsDto>()
                    .ForMember(dest => dest.ListName,
                        opts => opts.MapFrom(src => src.List.Name));

                cfg.CreateMap<CommentCardDto, Comment>();
                cfg.CreateMap<Comment, CommentDto>();
            });
            return mapperConfiguration.CreateMapper();
        }
    }
}
