using Library.Configuration;
using Library.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.Config
{
    /// <summary>
    /// 控制器测试数据
    /// </summary>
    public class ControllerConfig
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public static ControllerConfig GetData()
        {
            return new ConfigHelper("jsonconfig/controller.json").GetModel<ControllerConfig>("Datas");
        }

        /// <summary>
        /// 控制器设置集合
        /// </summary>
        public Dictionary<string, ControllerTestSetting> Controllers { get; set; }
    }

    /// <summary>
    /// 控制器设置
    /// </summary>
    public class ControllerTestSetting
    {
        /// <summary>
        /// 服务器
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// 启用数据库命令
        /// </summary>
        public bool EnableDatabaseCmd { get; set; } = false;

        /// <summary>
        /// 操作设置集合
        /// </summary>
        public Dictionary<string, ActionTestSetting> Actions { get; set; }
    }

    /// <summary>
    /// 操作设置
    /// </summary>
    public class ActionTestSetting
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, object> Paramters { get; set; }

        /// <summary>
        /// 测试数据量
        /// </summary>
        public int TestDataCount { get; set; }

        /// <summary>
        /// 设置测试数据语句(占位符含义 {0} => {i:000})
        /// </summary>
        public List<string> SetupTestDataCmd { get; set; }

        /// <summary>
        /// 清除测试数据语句(占位符含义 {0} => {i:000})
        /// </summary>
        public List<string> ClearTestDataCmd { get; set; }
    }
}
