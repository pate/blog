using System.Collections.Generic;
using AutoMapper;
using PagedList;

namespace Blog.Web.Helpers
{
    public static class AutoMapperHelpers
    {
        public static IMappingExpression<TDestination, TSource> BothWays<TSource, TDestination>
            (this IMappingExpression<TSource, TDestination> mappingExpression)
        {
            return Mapper.CreateMap<TDestination, TSource>();
        }

        public static IPagedList<TDestination> ToMappedPagedList<TSource, TDestination>(this IPagedList<TSource> list)
        {
            IEnumerable<TDestination> sourceList = Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(list);
            IPagedList<TDestination> pagedResult = new StaticPagedList<TDestination>(sourceList, list.GetMetaData());
            return pagedResult;

        }
    }
}