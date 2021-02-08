using Entity.Common;
using Library.DataMapping.Annotations;
using Library.OpenApi.Annotations;
using System.Collections.Generic;
using System.ComponentModel;

/* 
 * 微信业务模型
 */
namespace Model.Common.WeChatUserInfoDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(Common_WeChatUserInfo))]
    [OpenApiMainTag("List")]
    public class List : Common_WeChatUserInfo
    {

    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(Common_WeChatUserInfo))]
    [OpenApiMainTag("Detail")]
    public class Detail : Common_WeChatUserInfo
    {
        /// <summary>
        /// 用户详情
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("用户详情")]
        public List<System.UserDTO.Detail> _Users { get; set; }
        /// <summary>
        /// 用户详情
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("用户详情")]
        public List<Public.MemberDTO.Detail> _Members { get; set; }
    }

    /// <summary>
    /// 微信网页授权时携带的参数信息
    /// </summary>
    public class StateInfo
    {
        /// <summary>
        /// 类型
        /// </summary>
        public WeChatStateType Type { get; set; }

        /// <summary>
        /// 重定向地址
        /// </summary>
        public string RedirectUrl { get; set; }
    }
}
