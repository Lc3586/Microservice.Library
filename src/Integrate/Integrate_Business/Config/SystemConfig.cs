using Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Integrate_Business.Config
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemConfig
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        public static SystemConfig systemConfig = null;

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
                if (RunMode == RunMode.Publish)
                    return PublishRootUrl;
                else
                    return localRootUrl;
            }
        }

        /// <summary>
        /// 发布后网站根地址
        /// </summary>
        public string PublishRootUrl { get; set; }

        /// <summary>
        /// 本地调试根地址
        /// </summary>
        public string localRootUrl { get; set; }

        /// <summary>
        /// 运行模式
        /// </summary>
        public RunMode RunMode { get; set; }

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
        /// 默认数据库类型
        /// </summary>
        public DatabaseType DefaultDatabaseType { get; set; }

        /// <summary>
        /// 默认数据库连接字符串
        /// </summary>
        public string DefaultDatabaseConnectString { get; set; }

        /// <summary>
        /// 实体类命名空间
        /// </summary>
        public string EntityAssembly { get; set; }

        /// <summary>
        /// 默认缓存类型
        /// </summary>
        public CacheType DefaultCacheType { get; set; }

        /// <summary>
        /// Redis配置
        /// </summary>
        public string RedisConfig { get; set; }

        /// <summary>
        /// 默认日志类型
        /// </summary>
        public LoggerType DefaultLoggerType { get; set; }

        #region 框架自定义类

        /// <summary>
        /// 框架自定义类命名空间
        /// </summary>
        public string[] FxAssembly
        {
            get { return _FxAssembly; }
            set
            {
                _FxAssembly = value;
                FxTypes = value.SelectMany(x =>
                {
                    try
                    {
                        return Assembly.Load(x).GetTypes();
                    }
                    catch
                    {
                        var result = Array.Empty<Type>();
                        if (x.IndexOfAny(new[] { '\\', '/', ':', '*', '"', '<', '>', '|' }) > -1)
                        {
                            var Files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"{x}.dll");
                            if (Files.Length > 0)
                                result = Files.SelectMany(F => Assembly.LoadFile(F).GetTypes()).ToArray();
                        }
                        else
                        {
                            var path = $"{AppDomain.CurrentDomain.BaseDirectory}{x}.dll";
                            if (File.Exists(path))
                                result = Assembly.LoadFile(path).GetTypes();
                        }
                        return result;
                    }
                }).ToList();
            }
        }

        /// <summary>
        /// 框架自定义类命名空间
        /// </summary>
        private string[] _FxAssembly { get; set; }

        /// <summary>
        /// 框架自定义类
        /// </summary>
        public List<Type> FxTypes { get; set; }

        #endregion

        #region ElasticSearch

        /// <summary>
        /// 启用ElasticSearch
        /// </summary>
        public bool ElasticsearchEnable { get; set; }

        /// <summary>
        /// ElasticSearch集群
        /// </summary>
        public Uri[] ElasticsearchNodes { get; set; }

        #endregion

        #region FreeSql

        /// <summary>
        /// 启用FreeSql
        /// </summary>
        public bool EnableFreeSql { get; set; }

        /// <summary>
        /// 启动时同步实体类型集合到数据库
        /// </summary>
        public bool FreeSqlSyncStructureOnStartup { get; set; }

        #endregion

        #region CAS配置

        /// <summary>
        /// 启用CAS
        /// </summary>
        public bool CASEnable { get; set; }

        /// <summary>
        /// CAS跨域地址
        /// </summary>
        public string[] CASCorsUrl { get; set; }

        /// <summary>
        /// CAS地址
        /// </summary>
        public string CASBaseUrl { get; set; }

        /// <summary>
        /// 启用CAS自定义登录
        /// </summary>
        public bool CASCustom { get; set; }

        /// <summary>
        /// CAS自定义登录地址
        /// </summary>
        public string CASCustomloginUrl { get; set; }

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
    }
}
