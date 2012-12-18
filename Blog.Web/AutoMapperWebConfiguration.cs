using System.Web.Mvc;
using AutoMapper;
using Blog.Data.Models;
using Blog.Web.Areas.Admin.Models;
using Blog.Web.ViewModels;

namespace Blog.Web
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.AddProfile(new PageMappingProfile());
        }
    }

    public class PageMappingProfile : Profile
    {
        protected override void Configure()
        {
            
            CreateMap<ViewTemplateInputModel, ViewTemplate>();
            CreateMap<ViewTemplate, ViewTemplateInputModel>();

            CreateMap<PageInputModel, Page>();
            CreateMap<Page, PageInputModel>();
            CreateMap<Page, PageViewModel>()
                .ForMember(dest => dest.Body, opt => opt.MapFrom(s => MvcHtmlString.Create(s.Body)));

            CreateMap<PostInputModel, Post>();
            CreateMap<Post, PostInputModel>();
            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.Body, opt => opt.MapFrom(s => MvcHtmlString.Create(s.Body)));
        }
    }
}