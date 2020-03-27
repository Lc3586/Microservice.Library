using AutoMapper;

namespace Library.DataMapping
{
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

        public TDestination Map(TDestination model)
        {
            return Mapper.Map<TDestination>(model);
        }
    }
}