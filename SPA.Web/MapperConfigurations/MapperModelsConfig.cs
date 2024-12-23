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
    }
}