using AutoMapper;
using SPA.BLL.Models;
using SPA.DAL.Entity;
using SPA.Web.Models;

namespace SPA.Web.MapperConfigurations;

public class MapperModelsConfig : Profile
{
    public MapperModelsConfig()
    {
        CreateMap<User, UserModel>().ReverseMap();
        CreateMap<UserModel, UserViewModel>();
        CreateMap<UserCreateModel, UserModel>();

        CreateMap<CommentCreateModel, CommentModel>();
        CreateMap<Comment, CommentModel>().ReverseMap();
        CreateMap<CommentModel, CommentViewModel>()
            .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies));
    }
}