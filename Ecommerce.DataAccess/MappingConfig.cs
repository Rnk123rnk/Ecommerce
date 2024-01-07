using AutoMapper;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;

namespace Ecommerce.DataAccess
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<UsersDTO, Users>().ReverseMap();
            CreateMap<UserPersonalAddresseDTO, UserPersonalAddresse>().ReverseMap();
            CreateMap<SubCategoryDTO, SubCategory>().ReverseMap();
            CreateMap<ThirdCategoryDTO, ThirdCategory>().ReverseMap();
            CreateMap<CompanyDTO, Company>().ReverseMap();

        }
    }
}
