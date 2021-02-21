using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

/* 
 * CAS相关业务模型
 */
namespace Model.Utils.CAS.CASDTO
{
    /// <summary>
    /// 验证后的身份信息
    /// </summary>
    public class AuthorizedInfo
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        ///// <summary>
        ///// 不能使用Cooki时将此值存入请求头中（Key：CasToken）
        ///// </summary>
        //public string CasToken { get; set; }
    }

    /// <summary>
    /// 获取tgt
    /// </summary>
    public class GetTGT
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不可为空")]
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不可为空")]
        public string Password { get; set; }
    }

    /// <summary>
    /// 获取票据
    /// </summary>
    public class GetST
    {
        [Required(ErrorMessage = "TGT不可为空")]
        public string TGT { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string Service { get; set; }
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    public class GetUserInfo
    {
        /// <summary>
        /// 票据
        /// </summary>
        [Required(ErrorMessage = "ST不可为空")]
        public string ST { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string Service { get; set; }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string account { get; set; }
    }

    /// <summary>
    /// 登出
    /// </summary>
    public class LogOut
    {
        //[Required(ErrorMessage = "TGT不可为空")]
        public string TGT { get; set; }

        /// <summary>
        /// 注销cas（当前账号登录的所有应用都会注销）
        /// </summary>
        public bool LogoutCAS { get; set; }

        /// <summary>
        /// 登出后跳转地址
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
