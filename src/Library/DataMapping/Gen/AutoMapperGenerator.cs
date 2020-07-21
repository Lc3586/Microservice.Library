using AutoMapper;
using Library.DataMapping.Annotations;
using Library.DataMapping.Application;
using Library.DataMapping.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.DataMapping.Gen
{
    public class AutoMapperGenerator : IAutoMapperProvider
    {
        private readonly AutoMapperGenOptions _options;

        public AutoMapperGenerator(AutoMapperGenOptions options)
        {
            _options = options ?? new AutoMapperGenOptions();
        }

        private IMapper mapper;

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
                _options.AutoMapperGeneratorOptions.Types.ForEach(x =>
                {
                    if (_options.AutoMapperGeneratorOptions.EnableMapFrom)
                        cfg.CreateMap<MapFromAttribute>(x);
                    if (_options.AutoMapperGeneratorOptions.EnableMapTo)
                        cfg.CreateMap<MapToAttribute>(x);
                });
            });

            mapper = config.CreateMapper();

            return mapper;
        }
    }
}
