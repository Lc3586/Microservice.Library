using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Common
{
    /// <summary>
    /// 操作者用户信息
    /// </summary>
    public class OperatorUserInfo
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Tel { get; set; }
    }
}
