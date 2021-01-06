using Library.Container;
using Model.System.UserDTO;

namespace Business.Interface.System
{
    /// <summary>
    /// 操作者
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// 用户信息
        /// </summary>
        Base Property { get; }

        #region 操作方法

        /// <summary>
        /// 判断是否为超级管理员
        /// </summary>
        /// <returns></returns>
        bool IsAdmin();

        #endregion
    }
}
