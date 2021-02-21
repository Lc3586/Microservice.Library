﻿namespace Model.Utils.Config
{
    /// <summary>
    /// 运行模式
    /// </summary>
    public enum RunMode
    {
        /// <summary>
        /// 本地测试模式
        /// </summary>
        LocalTest,

        /// <summary>
        /// 本地测试模式（严格）
        /// </summary>
        LocalTest_Strict,

        /// <summary>
        /// 发布模式
        /// </summary>
        Publish,

        /// <summary>
        /// 发布模式（开放接口文档）
        /// </summary>
        Publish_Swagger
    }
}
