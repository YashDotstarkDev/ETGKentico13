using AutoMapper;
using ETG.Data.Models.Common;
using ETG.Data.Experience.Models;
using Ninject.Activation;
using ETG.Data.Destination.Models;
using ETG.Data.Article.Models;
using ETG.Data.Models.Base;
using ETG.WebAPI.Models.ArticleData;

namespace ETG.Web.AutoMapper
{
    public class WebAPIMapperProfile : Profile
    {
        public WebAPIMapperProfile(IContext context)
        {
            CreateMap<SearchResults<ArticleModel>, SearchResults<ArticleAPIModel>>();
            CreateMap<ArticleModel, ArticleAPIModel>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.HeroImage))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.CategoriesText))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.DisplayPublishDate))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.DestinationsText));
            }
    }
}