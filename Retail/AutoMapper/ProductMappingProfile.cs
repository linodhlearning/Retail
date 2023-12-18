using AutoMapper;
using Retail.Model;
using Retail.Repository.Entity;

namespace Retail.AutoMapper
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductModel, ProductEntity >()
                //.ForMember(dest => dest.ModifiedBy, act => act.MapFrom(src => src.ModifiedBy)) 
                .ReverseMap();
        }
    }
}
