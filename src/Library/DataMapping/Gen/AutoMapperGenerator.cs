using AutoMapper;
using Microservice.Library.DataMapping.Application;
using Microservice.Library.DataMapping.Extention;

namespace Microservice.Library.DataMapping.Gen
{
    /// <summary>
    /// 生成器
    /// </summary>
    public class AutoMapperGenerator : IAutoMapperProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public AutoMapperGenerator(AutoMapperGenOptions options)
        {
            Options = options ?? new AutoMapperGenOptions();
        }

        readonly AutoMapperGenOptions Options;

        IMapper mapper;

        /// <summary>
        /// 获取映射器
        /// </summary>
        /// <returns></returns>
        public IMapper GetMapper()
        {
            if (mapper != null)
                return mapper;

            var config = new MapperConfiguration(cfg =>
            {
                Options.AutoMapperGeneratorOptions.Types.ForEach(x =>
                {
                    if (Options.AutoMapperGeneratorOptions.EnableMapFrom)
                        cfg.CreateMapFrom(x);

                    if (Options.AutoMapperGeneratorOptions.EnableMapTo)
                        cfg.CreateMapTo(x);
                });
            });

            mapper = config.CreateMapper();

            return mapper;
        }
    }
}
