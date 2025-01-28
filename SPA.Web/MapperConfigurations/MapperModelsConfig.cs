using AutoMapper;
using SPA.BLL.Models;
using SPA.DAL.Entity;
using SPA.Web.Models;

namespace SPA.Web.MapperConfigurations
{
    public class MapperModelsConfig : Profile
    {
        public MapperModelsConfig()
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<UserModel, UserViewModel>();
            CreateMap<UserCreateViewModel, UserModel>();
            CreateMap<AuthorizationInfo, AuthorizationInfoModel>().ReverseMap();

            CreateMap<CommentCreateModel, CommentModel>();
            CreateMap<Comment, CommentModel>()
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments));
            
            CreateMap<CommentModel, CommentViewModel>()
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments));
            
            CreateMap<Attachment, AttachmentModel>().ReverseMap();
            CreateMap<AttachmentModel, AttachmentViewModel>().ReverseMap();
            
            CreateMap<PaginationResultModel<CommentModel>, PaginationResultModel<CommentViewModel>>().ReverseMap();
        }
    }
}