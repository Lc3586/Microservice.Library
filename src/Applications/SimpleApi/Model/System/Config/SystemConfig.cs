using FreeSql;
using FreeSql.Internal;
using Library.Cache;
using Library.Configuration.Annotations;
using System;
using System.Collections.Generic;

namespace Model.System.Config
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

        /// <summary>
        /// 服务器标识
        /// </summary>
        public string ServerKey { get; set; }

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

        #endregion

        #region 日志

        /// <summary>
        /// 默认日志组件名称
        /// </summary>
        public string DefaultLoggerName { get; set; } = "SystemLog";

        /// <summary>
        /// 默认日志组件类型
        /// </summary>
        public LoggerType DefaultLoggerType { get; set; } = LoggerType.Console;

        /// <summary>
        /// 默认日志组件布局
        /// </summary>
        public string DefaultLoggerLayout { get; set; }

        /// <summary>
        /// 需要记录的日志的最低等级
        /// </summary>
        public int MinLogLevel { get; set; }

        #endregion

        #region MyRegion

        /// <summary>
        /// 启用Swagger
        /// </summary>

        public bool EnableSwagger { get; set; }

        /// <summary>
        /// swagger说明文档
        /// </summary>
        public List<string> SwaggerXmlComments { get; set; }

        /// <summary>
        /// swagger接口版本说明
        /// </summary>
        public SwaggerApiVersionDescription SwaggerApiVersion { get; set; }

        /// <summary>
        /// swagger接口多版本说明
        /// </summary>
        public List<SwaggerApiMultiVersionDescription> SwaggerApiMultiVersion { get; set; }

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
        /// 默认数据库类型
        /// </summary>
        public DataType DefaultDatabaseType { get; set; }

        /// <summary>
        /// 默认数据库连接字符串
        /// </summary>
        public string DefaultDatabaseConnectString { get; set; }

        /// <summary>
        /// 实体类命名空间
        /// </summary>
        public List<string> EntityAssembly { get; set; }

        /// <summary>
        /// 是否多数据库
        /// </summary>
        public bool MultiDatabases { get; set; }

        /// <summary>
        /// 多数据库配置
        /// </summary>
        [JsonConfig("jsonconfig/database.json")]//独立的配置文件
        public List<DatabaseSetting> Databases { get; set; }

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
        [JsonConfig("jsonconfig/soap.json")]
        public List<SoapSetting> Soaps { get; set; }

        #endregion

        #region CAS

        /// <summary>
        /// 启用CAS
        /// </summary>
        public bool EnableCAS { get; set; }

        /// <summary>
        /// 协议版本号
        /// </summary>
        public string CASProtocolVersion { get; set; }

        /// <summary>
        /// CAS跨域地址
        /// </summary>
        public List<string> CASCorsUrl { get; set; }

        /// <summary>
        /// CAS地址
        /// </summary>
        public string CASBaseUrl { get; set; }

        /// <summary>
        /// 启用单点注销
        /// </summary>
        public bool EnableCasSingleSignOut { get; set; }

        /// <summary>
        /// CAS获取TGT票据地址
        /// </summary>
        public string CASTGTUrl { get; set; }

        /// <summary>
        /// CAS生成ST票据地址
        /// </summary>
        public string CASSTUrl { get; set; }

        /// <summary>
        /// CAS获取用户信息地址
        /// </summary>
        public string CASUserInfoUrl { get; set; }

        /// <summary>
        /// CAS删除TGT地址
        /// </summary>
        public string CASDeleteSTUrl { get; set; }

        #endregion

        #region 微信服务

        /// <summary>
        /// 启用微信服务
        /// </summary>
        public bool EnableWeChatService { get; set; }

        /// <summary>
        /// 微信服务配置
        /// </summary>
        [JsonConfig("jsonconfig/wechatservice.json")]
        public WeChatServiceSetting WeChatService { get; set; }

        #endregion
    }
}
