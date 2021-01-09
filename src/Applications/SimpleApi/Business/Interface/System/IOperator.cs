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
        Authorities Property { get; }

        /// <summary>
        /// 判断是否为管理员
        /// </summary>
        /// <returns></returns>
        bool IsAdmin { get; }
    }
}
