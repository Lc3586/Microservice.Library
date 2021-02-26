using AutoMapper;

namespace Microservice.Library.DataMapping.Extention
{
    /// <summary>
    /// AutoMapper帮助类
    /// </summary>
    /// <typeparam name="TSouurce">来源数据类型</typeparam>
    /// <typeparam name="TDestination">目标数据类型</typeparam>
    public class AutoMapperHelper<TSouurce, TDestination>
    {
        /// <summary>
        /// 
        /// </summary>
        public AutoMapperHelper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.CreateMap<TSouurce, TDestination>()).CreateMapper();
        }

        private static IMapper Mapper { get; set; }

        /// <summary>
        /// 获取映射器
        /// </summary>
        /// <returns></returns>
        public IMapper GetMapper()
        {
            return Mapper;
        }

        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TDestination Map(object model)
        {
            return Mapper.Map<TDestination>(model);
        }
    }
}