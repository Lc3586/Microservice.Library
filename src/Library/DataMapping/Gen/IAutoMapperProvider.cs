using AutoMapper;

namespace Microservice.Library.DataMapping.Gen
{
    /// <summary>
    /// 构造器
    /// </summary>
    public interface IAutoMapperProvider
    {
        /// <summary>
        /// 获取映射器
        /// </summary>
        /// <returns></returns>
        IMapper GetMapper();
    }
}
