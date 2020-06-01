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

            var maps = new List<(Type from, Type target, bool enableForMember, bool isFrom)>();

            maps.AddRange(_options.AutoMapperGeneratorOptions.Types.Where(x => x.GetCustomAttribute<MapToAttribute>() != null)
                .Select(x =>
                {
                    var attribute = x.GetCustomAttribute<MapToAttribute>();
                    return (x, attribute.TargetType, attribute.EnableForMember, false);
                }));
            maps.AddRange(_options.AutoMapperGeneratorOptions.Types.Where(x => x.GetCustomAttribute<MapFromAttribute>() != null)
                .Select(x =>
                {
                    var attribute = x.GetCustomAttribute<MapFromAttribute>();
                    return (attribute.FromType, x, attribute.EnableForMember, true);
                }));

            var config = new MapperConfiguration(cfg =>
            {
                maps.ForEach(aMap =>
                {
                    var map = cfg.CreateMap(aMap.from, aMap.target);
                    if (aMap.enableForMember)
                    {
                        (aMap.isFrom ?
                            aMap.target.GetForMembersOptions(aMap.isFrom) :
                            aMap.from.GetForMembersOptions(aMap.isFrom))?
                        .ForEach(o => map.ForMember(o.Name, o.Option));
                    }
                });
            });

            mapper = config.CreateMapper();

            return mapper;
        }
    }
}
