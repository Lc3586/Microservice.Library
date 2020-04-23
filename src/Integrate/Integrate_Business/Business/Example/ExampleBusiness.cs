using Library.Container;
using Library.Models;
using Library.LinqTool;
using Newtonsoft.Json;
using Library.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using Library.Tree;
using System.Linq.Expressions;
using Nest;
using Integrate_Model.Example;
using AutoMapper;
using AutoMapper.Configuration;
using Integrate_Business.Util;
using Library.Elasticsearch;
using Library.Elasticsearch.Gen;
using ExampleEntity = Integrate_Entity.Example.Example;//当类和命名空间重名时使用别名（任何情况下都要避免出现重名）
using Library.FreeSql.Gen;
using Library.FreeSql.Extention;
using Library.DataMapping.Gen;

namespace Integrate_Business.Example
{
    /// <summary>
    /// 示例业务类
    /// </summary>
    public class ExampleBusiness : BaseBusiness<ExampleEntity>, IExampleBusiness, IDependency
    {
        #region DI

        /// <summary>
        /// 在构造函数中注入DI系统中注册的依赖
        /// </summary>
        /// <param name="freeSqlProvider">ORM（仅作示范，在实际业务中根据需要来注入依赖）</param>
        /// <param name="autoMapperProvider">映射器</param>
        public ExampleBusiness(
            IFreeSqlProvider freeSqlProvider,
            IAutoMapperProvider autoMapperProvider)
        {
            mapper = autoMapperProvider.GetMapper();
            orm = freeSqlProvider.GetFreeSql();
        }

        #endregion

        #region 外部接口

        public List<ExampleDTO.List> GetList(Pagination pagination)
        {
            #region 从数据库查询数据

            var select = SelectExtension.Select<ExampleDTO.List>((d, a) =>
            {
                a.Id_ = a.Id.ToString();
            });

            //var q = from a in orm            
            //            .Select<ExampleEntity>()
            //            .ToDynamic<ExampleEntity, ExampleDTO.List>(orm, pagination, typeof(ExampleDTO.List).GetNamesWithTagAndOther(true, "_List"))
            //        select @select.Invoke(a) as BindConfigDTO.List;

            //return q.ToList();

            #endregion

            #region 忽略此部分代码

            var m = ExampleData.AsQueryable();

            var q = from a in m select @select.Invoke(a);//这里使用示例数据进行示范

            if (!pagination.FilterToLinqDynamic(ref q))//生成搜索语句
                throw new MessageException("搜索条件不支持");

            if (!pagination.OrderByToLinqDynamic(ref q))//生成排序语句
                throw new MessageException("排序条件不支持");

            //var where = Where.True<ExampleDTO.List>();//查询结果集筛选条件
            var where = Where.TrueFunc<ExampleDTO.List>();

            var list = q.Where(where).GetPagination(pagination).ToList();

            return list;

            #endregion
        }

        public ExampleDTO.Edit GetEdit(string id)
        {
            //return mapper.Map<ExampleDTO.Edit>(repository.GetAndCheckNull(Convert.ToInt64(id)));//数据库获取实体
            var data = ExampleData.FirstOrDefault(o => o.Id.ToString() == id);
            return mapper.Map<ExampleDTO.Edit>(data);//实体映射到业务模型
        }

        public ExampleDTO.Detail GetDetail(string id)
        {
            //return mapper.Map<ExampleDTO.Detail>(repository.GetAndCheckNull(Convert.ToInt64(id)));//数据库获取实体
            var data = ExampleData.FirstOrDefault(o => o.Id.ToString() == id);
            return mapper.Map<ExampleDTO.Detail>(data);//实体映射到业务模型
        }

        //[DataRepeatValidate(
        //    new string[] { "Name" },
        //    new string[] { "名称" })]//去重判断（查询数据库）
        public AjaxResult Create(ExampleDTO.Create data)
        {
            var newData = mapper.Map<ExampleEntity>(data);//业务模型映射到实体
            //repository.Insert(newData.InitEntity());//插入数据库

            ExampleData.Add(newData.InitEntity());
            return Success();//返回成功
        }

        //[DataRepeatValidate(
        //    new string[] { "Name" },
        //    new string[] { "名称" })]//去重判断（查询数据库）
        public AjaxResult Edit(ExampleDTO.Edit data)
        {
            #region 修改数据库数据

            //if (repository.UpdateDiy
            // .SetSource(Mapper.Map<ExampleEntity>(data).ModifyEntity())
            // .UpdateColumns(typeof(ExampleDTO.Edit).GetNamesWithTagAndOther(false, "_Edit").ToArray())
            // .ExecuteAffrows() > 0)
            //    return Success();
            //else
            //    return Error("操作失败");

            #endregion 

            var oldData = ExampleData.FirstOrDefault(o => o.Id == data.Id_.ConvertToAny<long>());
            if (oldData == null)
                return Error("未找到数据");//返回错误
            data.ModifyEntity();//更改实体
            ExampleData.ForEach(o =>
            {
                if (o.Id == data.Id_.ConvertToAny<long>())
                    o = data;
            });
            return Success();//返回成功
        }

        public AjaxResult Delete(List<string> ids)
        {
            #region 从数据库删除数据

            var _ids = ids.Select(o => Convert.ToInt64(o));
            //if (repository.Delete(o => _ids.Contains(o.Id)) > 0)
            //    return Success();
            //else
            //    return Error("操作失败");

            #endregion

            ExampleData.RemoveAll(o => _ids.Contains(o.Id));
            return Success();//返回成功
        }

        #endregion

        #region 私有成员
        IMapper mapper { get; set; }

        IFreeSql orm { get; set; }

        /// <summary>
        /// 示例数据
        /// </summary>
        static List<ExampleEntity> ExampleData = new List<ExampleEntity>()
        {
            new ExampleEntity()
            {
                Id=1584086259284,
                Name = "名称A",
                Content = "内容A",
                CreatorId="admin",
                CreatorName = "管理员",
                CreateTime = DateTime.Now,
                ModifyTime = DateTime.Now
            },
            new ExampleEntity()
            {
                Id=1584086259285,
                Name = "名称B",
                Content = "内容B",
                CreatorId="admin",
                CreatorName = "管理员",
                CreateTime = DateTime.Now.AddMinutes(5),
                ModifyTime = DateTime.Now.AddMinutes(10)
            },
            new ExampleEntity()
            {
                Id=1584086259286,
                Name = "名称C",
                Content = "内容C",
                CreatorId="admin",
                CreatorName = "管理员",
                CreateTime = DateTime.Now.AddMinutes(5),
                ModifyTime = DateTime.Now.AddMinutes(15)
            }
        };

        #endregion

        #region 数据模型

        #endregion
    }
}