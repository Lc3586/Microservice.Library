using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net5TC.DTO;
using Coldairarrow.Util;
using Library.OpenApi.JsonSerialization;
using Library.ConsoleTool;

namespace Net5TC.Test
{
    /// <summary>
    /// 序列化测试
    /// </summary>
    [SimpleJob(RuntimeMoniker.Net461, baseline: true)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [RPlotExporter]
    public class SerializeTest
    {
        public List<DTO.DB_ADTO.List> ObjectList;

        [GlobalSetup]
        public void Setup()
        {
            ObjectList = new List<DTO.DB_ADTO.List>();

            new IdHelperBootstrapper()
                .SetWorkderId(100)
                .Boot();

            //var random = new Random();
            var total = Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));

            for (int i = 0; i < total; i++)
            {
                var data = new DTO.DB_ADTO.List
                {
                    Id = IdHelper.GetId(),
                    Name = "名称",
                    Content = "内容",
                    CreateTime = DateTime.Now,
                    CreatorId = Guid.NewGuid().ToString(),
                    CreatorName = "管理员",
                    ModifyTime = DateTime.Now,
                    ParentId = ObjectList.LastOrDefault()?.Id
                };

                data.BId = IdHelper.GetId();
                data.DB_B = new DTO.DB_BDTO.List
                {
                    Id = data.BId,
                    Name = "名称",
                    CreateTime = DateTime.Now,
                    CreatorId = Guid.NewGuid().ToString(),
                    CreatorName = "管理员",
                    ModifyTime = DateTime.Now
                };

                data.DB_Cs = new List<DTO.DB_CDTO.List>();
                for (int j = 0; j < 10; j++)
                {
                    var _data = new DTO.DB_CDTO.List
                    {
                        Id = IdHelper.GetId(),
                        Name = "名称",
                        CreateTime = DateTime.Now,
                        CreatorId = Guid.NewGuid().ToString(),
                        CreatorName = "管理员",
                        ModifyTime = DateTime.Now
                    };
                    data.DB_Cs.Add(_data);
                }

                data.DB_Ds = new List<DTO.DB_DDTO.List>();
                for (int j = 0; j < 10; j++)
                {
                    var _data = new DTO.DB_DDTO.List
                    {
                        Id = IdHelper.GetId(),
                        Name = "名称",
                        CreateTime = DateTime.Now,
                        CreatorId = Guid.NewGuid().ToString(),
                        CreatorName = "管理员",
                        ModifyTime = DateTime.Now,
                        AId = data.Id
                    };
                    data.DB_Ds.Add(_data);
                }

                ObjectList.Add(data);
            }
        }

        [Benchmark(Baseline = true, Description = "不过滤属性的序列化")]
        public void ToJsonWithoutFilter()
        {
            DeserializeTest.Json = JsonConvert.SerializeObject(ObjectList);
        }

        [Benchmark(Description = "过滤属性的序列化")]
        public void ToJsonFilter()
        {
            ObjectList.ToOpenApiJson();
        }
    }
}
