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
using ExampleEntity = Integrate_Entity.Example.Example;//����������ռ�����ʱʹ�ñ������κ�����¶�Ҫ�������������
using Library.FreeSql.Gen;
using Library.FreeSql.Extention;
using Library.DataMapping.Gen;

namespace Integrate_Business.Example
{
    /// <summary>
    /// ʾ��ҵ����
    /// </summary>
    public class ExampleBusiness : BaseBusiness<ExampleEntity>, IExampleBusiness, IDependency
    {
        #region DI

        /// <summary>
        /// �ڹ��캯����ע��DIϵͳ��ע�������
        /// </summary>
        /// <param name="freeSqlProvider">ORM������ʾ������ʵ��ҵ���и�����Ҫ��ע��������</param>
        /// <param name="autoMapperProvider">ӳ����</param>
        public ExampleBusiness(
            IFreeSqlProvider freeSqlProvider,
            IAutoMapperProvider autoMapperProvider)
        {
            mapper = autoMapperProvider.GetMapper();
            orm = freeSqlProvider.GetFreeSql();
        }

        #endregion

        #region �ⲿ�ӿ�

        public List<ExampleDTO.List> GetList(Pagination pagination)
        {
            #region �����ݿ��ѯ����

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

            #region ���Դ˲��ִ���

            var m = ExampleData.AsQueryable();

            var q = from a in m select @select.Invoke(a);//����ʹ��ʾ�����ݽ���ʾ��

            if (!pagination.FilterToLinqDynamic(ref q))//�����������
                throw new MessageException("����������֧��");

            if (!pagination.OrderByToLinqDynamic(ref q))//�����������
                throw new MessageException("����������֧��");

            //var where = Where.True<ExampleDTO.List>();//��ѯ�����ɸѡ����
            var where = Where.TrueFunc<ExampleDTO.List>();

            var list = q.Where(where).GetPagination(pagination).ToList();

            return list;

            #endregion
        }

        public ExampleDTO.Edit GetEdit(string id)
        {
            //return mapper.Map<ExampleDTO.Edit>(repository.GetAndCheckNull(Convert.ToInt64(id)));//���ݿ��ȡʵ��
            var data = ExampleData.FirstOrDefault(o => o.Id.ToString() == id);
            return mapper.Map<ExampleDTO.Edit>(data);//ʵ��ӳ�䵽ҵ��ģ��
        }

        public ExampleDTO.Detail GetDetail(string id)
        {
            //return mapper.Map<ExampleDTO.Detail>(repository.GetAndCheckNull(Convert.ToInt64(id)));//���ݿ��ȡʵ��
            var data = ExampleData.FirstOrDefault(o => o.Id.ToString() == id);
            return mapper.Map<ExampleDTO.Detail>(data);//ʵ��ӳ�䵽ҵ��ģ��
        }

        //[DataRepeatValidate(
        //    new string[] { "Name" },
        //    new string[] { "����" })]//ȥ���жϣ���ѯ���ݿ⣩
        public AjaxResult Create(ExampleDTO.Create data)
        {
            var newData = mapper.Map<ExampleEntity>(data);//ҵ��ģ��ӳ�䵽ʵ��
            //repository.Insert(newData.InitEntity());//�������ݿ�

            ExampleData.Add(newData.InitEntity());
            return Success();//���سɹ�
        }

        //[DataRepeatValidate(
        //    new string[] { "Name" },
        //    new string[] { "����" })]//ȥ���жϣ���ѯ���ݿ⣩
        public AjaxResult Edit(ExampleDTO.Edit data)
        {
            #region �޸����ݿ�����

            //if (repository.UpdateDiy
            // .SetSource(Mapper.Map<ExampleEntity>(data).ModifyEntity())
            // .UpdateColumns(typeof(ExampleDTO.Edit).GetNamesWithTagAndOther(false, "_Edit").ToArray())
            // .ExecuteAffrows() > 0)
            //    return Success();
            //else
            //    return Error("����ʧ��");

            #endregion 

            var oldData = ExampleData.FirstOrDefault(o => o.Id == data.Id_.ConvertToAny<long>());
            if (oldData == null)
                return Error("δ�ҵ�����");//���ش���
            data.ModifyEntity();//����ʵ��
            ExampleData.ForEach(o =>
            {
                if (o.Id == data.Id_.ConvertToAny<long>())
                    o = data;
            });
            return Success();//���سɹ�
        }

        public AjaxResult Delete(List<string> ids)
        {
            #region �����ݿ�ɾ������

            var _ids = ids.Select(o => Convert.ToInt64(o));
            //if (repository.Delete(o => _ids.Contains(o.Id)) > 0)
            //    return Success();
            //else
            //    return Error("����ʧ��");

            #endregion

            ExampleData.RemoveAll(o => _ids.Contains(o.Id));
            return Success();//���سɹ�
        }

        #endregion

        #region ˽�г�Ա
        IMapper mapper { get; set; }

        IFreeSql orm { get; set; }

        /// <summary>
        /// ʾ������
        /// </summary>
        static List<ExampleEntity> ExampleData = new List<ExampleEntity>()
        {
            new ExampleEntity()
            {
                Id=1584086259284,
                Name = "����A",
                Content = "����A",
                CreatorId="admin",
                CreatorName = "����Ա",
                CreateTime = DateTime.Now,
                ModifyTime = DateTime.Now
            },
            new ExampleEntity()
            {
                Id=1584086259285,
                Name = "����B",
                Content = "����B",
                CreatorId="admin",
                CreatorName = "����Ա",
                CreateTime = DateTime.Now.AddMinutes(5),
                ModifyTime = DateTime.Now.AddMinutes(10)
            },
            new ExampleEntity()
            {
                Id=1584086259286,
                Name = "����C",
                Content = "����C",
                CreatorId="admin",
                CreatorName = "����Ա",
                CreateTime = DateTime.Now.AddMinutes(5),
                ModifyTime = DateTime.Now.AddMinutes(15)
            }
        };

        #endregion

        #region ����ģ��

        #endregion
    }
}