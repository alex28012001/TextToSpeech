using AutoMapper;
using System.Collections.Generic;

namespace BLL.Helper
{
    public static class GenericMapping<TSource, TDest> where TSource : class
    {
        public static TDest Map(TSource source)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDest>()).CreateMapper();
            return mapper.Map<TSource, TDest>(source);
        }

        public static IEnumerable<TDest> MapCollection(IEnumerable<TSource> source)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDest>()).CreateMapper();
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDest>>(source);
        }
    }
}
