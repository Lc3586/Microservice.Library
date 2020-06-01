using AutoMapper;

namespace Library.DataMapping.Helper
{
    /// <summary>
    /// AutoMapper帮助类
    /// </summary>
    /// <typeparam name="TSouurce"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public class AutoMapperHelper<TSouurce, TDestination>
    {
        public AutoMapperHelper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.CreateMap<TSouurce, TDestination>()).CreateMapper();
        }

        private static IMapper Mapper { get; set; }

        public IMapper GetMapper()
        {
            return Mapper;
        }

        public TDestination Map(object model)
        {
            return Mapper.Map<TDestination>(model);
        }
    }
}