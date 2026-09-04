using AutoMapper;
using ETG.Data.Models.Common;
using ETG.Data.Experience.Models;
using Ninject.Activation;
using ETG.Data.Destination.Models;
using ETG.Data.Article.Models;
using ETG.WebAPI.Models.ArticleData;

namespace ETG.Web.AutoMapper
{
    public class LandingPageMapperProfile : Profile
    {
        public LandingPageMapperProfile(IContext context)
        {
            CreateMap<ArticleListingRequest, ArticleLandingFilter>();
            CreateMap<DestinationSummaryModel, PrimaryLandingItemModel>().ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.HeroImage)).ForMember(dest => dest.Heading, opt => opt.MapFrom(src => src.Name));
            CreateMap<ExperienceSummaryModel, PrimaryLandingItemModel>().ForMember(dest => dest.Heading, opt => opt.MapFrom(src => src.Name));
            //CreateMap(typeof(DestinationSummaryModel), typeof(PrimaryLandingItemModel), MemberList.None).ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.HeroImage)).ReverseMap();
        }
    }
}