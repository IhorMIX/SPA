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
            CreateMap<Comment, CommentModel>().ReverseMap();
            CreateMap<CommentModel, CommentViewModel>();

            // Явный маппинг для PaginationResultModel с вложенными коллекциями
            CreateMap<PaginationResultModel<IEnumerable<CommentModel>>, PaginationResultModel<IEnumerable<CommentViewModel>>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom((src, dest, member, context) => 
                    src.Data.Select(commentList => commentList.Select(c => context.Mapper.Map<CommentViewModel>(c))))
                )
                .ForMember(dest => dest.PageSize, opt => opt.Ignore())
                .ReverseMap();
        }
    }

}