using Microservice.Library.Cache.Model;
using Microservice.Library.Configuration.Annotations;
using System.Collections.Generic;

namespace Model.Utils.Config
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
        /// 超级管理员账号
        /// </summary>
        public string AdminAccount { get; set; }

        /// <summary>
        /// 超级管理员账号初始化密码
        /// </summary>
        public string AdminInitPassword { get; set; }

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

        /// <summary>
        /// 简易身份验证
        /// </summary>
        public bool EnableSampleAuthentication { get; set; }

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

        #region Swagger

        /// <summary>
        /// 启用Swagger
        /// </summary>

        public bool EnableSwagger { get; set; }

        /// <summary>
        /// Swagger配置
        /// </summary>
        [JsonConfig("jsonconfig/swagger.json")]
        public SwaggerSetting Swagger { get; set; }

        #endregion

        #region 缓存

        /// <summary>
        /// 启用缓存
        /// </summary>
        public bool EnableCache { get; set; }

        /// <summary>
        /// 默认缓存类型
        /// </summary>
        public CacheType DefaultCacheType { get; set; }

        /// <summary>
        /// Redis配置
        /// </summary>
        [JsonConfig("jsonconfig/redis.json", "FreeRedis")]
        public RedisSetting Redis { get; set; }

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
        /// 启用多数据库
        /// </summary>
        public bool EnableMultiDatabases { get; set; }

        /// <summary>
        /// 单数据库配置
        /// </summary>
        [JsonConfig("jsonconfig/database.json", "Single")]
        public DatabaseSetting Database { get; set; }

        /// <summary>
        /// 多数据库配置
        /// </summary>
        [JsonConfig("jsonconfig/database.json", "Multi")]
        public List<DatabaseSetting> Databases { get; set; }

        #endregion

        #region FreeSql

        /// <summary>
        /// 启用FreeSql
        /// </summary>
        public bool EnableFreeSql { get; set; }

        /// <summary>
        /// FreeSql配置
        /// </summary>
        [JsonConfig("jsonconfig/freesql.json", "FreeSql")]
        public FreeSqlSetting FreeSql { get; set; }

        #endregion

        #region ES服务

        /// <summary>
        /// 启用ElasticSearch
        /// </summary>
        public bool EnableElasticsearch { get; set; }

        /// <summary>
        /// ElasticSearch配置
        /// </summary>
        [JsonConfig("jsonconfig/elasticsearch.json", "ElasticSearch")]
        public ElasticsearchSetting Elasticsearch { get; set; }

        #endregion

        #region Kafka中间件

        /// <summary>
        /// 启用Kafka中间件
        /// </summary>
        public bool EnableKafka { get; set; }

        /// <summary>
        /// kafka中间件配置
        /// </summary>
        [JsonConfig("jsonconfig/kafka.json", "Kafka")]
        public KafkaSetting Kafka { get; set; }

        #endregion

        #region Soap

        /// <summary>
        /// 启用Soap
        /// </summary>
        public bool EnableSoap { get; set; }

        /// <summary>
        /// Soap配置
        /// </summary>
        [JsonConfig("jsonconfig/soap.json", "Soaps")]
        public List<SoapSetting> Soaps { get; set; }

        #endregion

        #region CAS

        /// <summary>
        /// 启用CAS
        /// </summary>
        public bool EnableCAS { get; set; }

        /// <summary>
        /// ElasticSearch配置
        /// </summary>
        [JsonConfig("jsonconfig/cas.json", "CAS")]
        public CASSetting CAS { get; set; }

        #endregion

        #region 微信服务

        /// <summary>
        /// 启用微信服务
        /// </summary>
        public bool EnableWeChatService { get; set; }

        /// <summary>
        /// 微信服务配置
        /// </summary>
        [JsonConfig("jsonconfig/wechatservice.json", "WeChatService")]
        public WeChatServiceSetting WeChatService { get; set; }

        #endregion
    }
}
