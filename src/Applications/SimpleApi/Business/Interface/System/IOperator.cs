using Model.Common;
using System.Collections.Generic;

namespace Business.Interface.System
{
    /// <summary>
    /// 操作者
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// 当前操作者Id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 用户类型
        /// </summary>
        /// <remarks><see cref="Model.System.UserType"/></remarks>
        string UserType { get; }

        /// <summary>
        /// 详细信息
        /// </summary>
        OperatorDetail Detail { get; }

        /// <summary>
        /// 判断是否为管理员
        /// </summary>
        /// <returns></returns>
        bool IsAdmin { get; }
    }
}
