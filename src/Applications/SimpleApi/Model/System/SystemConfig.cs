using FreeSql.Internal;
using Library.Configuration.Annotations;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Model.System
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemConfig
    {
        #region 基础

        /// <summary>
        /// 超级管理员Id
        /// </summary>
        public string AdminId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 网站根地址
        /// </summary>
        public string WebRootUrl
        {
            get
            {
                if (RunMode == RunMode.Publish || RunMode == RunMode.Publish_Swagger)
                    return PublishRootUrl;
                else
                    return LocalRootUrl;
            }
        }

        /// <summary>
        /// 发布后网站根地址
        /// </summary>
        public string PublishRootUrl { get; set; }

        /// <summary>
        /// 本地调试根地址
        /// </summary>
        public string LocalRootUrl { get; set; }

        /// <summary>
        /// 运行模式
        /// </summary>
        public RunMode RunMode { get; set; }

        #endregion

        #region 高级

        /// <summary>
        /// 标记了IDependency的类的命名空间集合
        /// <para>支持通配符</para>
        /// </summary>
        public List<string> FxAssembly { get; set; }

        #endregion

        #region 业务

        /// <summary>
        /// 数据删除模式,默认逻辑删除
        /// </summary>
        public DeleteMode DeleteMode { get; set; }

        /// <summary>
        /// JWT秘钥
        /// </summary>
        public string JWTSecret { get; set; }

        /// <summary>
        /// 工作ID
        /// </summary>
        public long WorkerId { get; set; }

        /// <summary>
        /// 默认日志类型
        /// </summary>
        public LoggerType DefaultLoggerType { get; set; }

        /// <summary>
        /// 日志最小记录等级
        /// </summary>
        public string LogMinLevel { get; set; }

        #endregion

        #region 缓存

        /// <summary>
        /// 默认缓存类型
        /// </summary>
        public CacheType DefaultCacheType { get; set; }

        /// <summary>
        /// Redis配置
        /// </summary>
        public string RedisConfig { get; set; }

        #endregion

        #region 数据映射

        /// <summary>
        /// 启用AutoMapper
        /// </summary>
        public bool EnableAutoMapper { get; set; }

        /// <summary>
        /// AutoMapper命名空间集合
        /// <para>支持通配符</para>
        /// </summary>
        public List<string> AutoMapperAssemblys { get; set; }

        #endregion

        #region 数据库

        /// <summary>
        /// 数据库配置
        /// </summary>
        [JsonConfig("/jsonconfig/database.json")]//独立的配置文件
        public List<DatabaseConfig> Databases { get; set; }

        /// <summary>
        /// 是否多数据库
        /// </summary>
        public bool MultiDatabases { get; set; }

        #endregion

        #region FreeSql

        /// <summary>
        /// 启用FreeSql
        /// </summary>
        public bool EnableFreeSql { get; set; }

        /// <summary>
        /// 数据库缓存过滤（不将相同的变更提交数据库）
        /// </summary>
        public bool DbCacheFilter { get; set; }

        /// <summary>
        /// 自动同步实体结构到数据库，程序运行中检查实体表是否存在，然后创建或修改
        /// </summary>
        public bool FreeSqlAutoSyncStructure { get; set; }

        /// <summary>
        /// 实体类名 -> 数据库表名，命名转换（类名、属性名都生效）
        /// 优先级小于 [Table(Name = "xxx")]、[Column(Name = "xxx")]
        /// </summary>
        public NameConvertType? FreeSqlSyncStructureNameConvert { get; set; }

        /// <summary>
        /// 启动时同步实体类型集合到数据库
        /// </summary>
        public bool FreeSqlSyncStructureOnStartup { get; set; }

        #endregion

        #region ES服务

        /// <summary>
        /// 启用ElasticSearch
        /// </summary>
        public bool EnableElasticsearch { get; set; }

        /// <summary>
        /// ElasticSearch集群
        /// </summary>
        public List<Uri> ElasticsearchNodes { get; set; }

        #endregion

        #region Soap

        /// <summary>
        /// 启用Soap
        /// </summary>
        public bool EnableSoap { get; set; }

        /// <summary>
        /// Soap配置
        /// </summary>
        [JsonConfig("/jsonconfig/soap.json")]
        public List<SoapConfig> Soaps { get; set; }

        #endregion
    }
}
