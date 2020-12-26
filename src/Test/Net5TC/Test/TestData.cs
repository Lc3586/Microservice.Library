using Coldairarrow.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5TC.Test
{
    /// <summary>
    /// 测试数据
    /// </summary>
    public static class TestData
    {
        static TestData()
        {
            new IdHelperBootstrapper()
                .SetWorkderId(100)
                .Boot();
        }

        private static readonly Random Random = new Random();

        private static readonly List<DTO.DB_ADTO.List> ObjectList = new List<DTO.DB_ADTO.List>();

        private static string ObjectListJson;

        private static int ObjectListJsonCount = 0;

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public static List<DTO.DB_ADTO.List> GetList(int total)
        {
            if (ObjectList.Any() && ObjectList.Count >= total)
                return ObjectList.Take(total).ToList();

            var _total = total - ObjectList.Count;

            for (int i = 0; i < _total; i++)
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

            return ObjectList.Take(total).ToList();
        }

        /// <summary>
        /// 获取Json数据
        /// </summary>
        /// <returns></returns>
        public static string GetJson(int total)
        {
            if (!string.IsNullOrWhiteSpace(ObjectListJson) && ObjectListJsonCount == total)
                return ObjectListJson;

            var data = GetList(total);

            ObjectListJsonCount = total;
            ObjectListJson = JsonConvert.SerializeObject(data);

            return ObjectListJson;
        }
    }
}
