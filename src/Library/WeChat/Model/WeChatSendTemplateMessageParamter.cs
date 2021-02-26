using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 微信发送模板消息参数
    /// </summary>
    public class WeChatSendTemplateMessageParamter
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="openId">接收消息的用户openid</param>
        /// <param name="templateId">订阅消息模板ID</param>
        /// <param name="data">
        /// 消息正文，
        /// value为消息内容文本（200字以内），
        /// 没有固定格式，可用\n换行，
        /// color为整段消息内容的字体颜色（目前仅支持整段消息为一种颜色）
        /// </param>
        /// <param name="url">
        /// 点击消息跳转的链接，
        /// 需要有ICP备案
        /// </param>
        /// <returns></returns>
        public static WeChatSendTemplateMessageParamter GetSimpleParamter(string openId, string templateId, object data, string url = null)
        {
            return new WeChatSendTemplateMessageParamter
            {
                OpenId = openId,
                TemplateId = templateId,
                Data = data,
                Url = url,
            };
        }

        #region 必填

        /// <summary>
        /// 接收消息的用户openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 订阅消息模板ID
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// 消息正文，
        /// value为消息内容文本（200字以内），
        /// 没有固定格式，可用\n换行，
        /// color为整段消息内容的字体颜色（目前仅支持整段消息为一种颜色）
        /// </summary>
        public object Data { get; set; }

        #endregion

        #region 选填

        /// <summary>
        /// 点击消息跳转的链接，
        /// 需要有ICP备案
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 跳小程序所需数据，
        /// 不需跳小程序可不用传该数据
        /// </summary>
        public TemplateModel_MiniProgram MiniProgram { get; }

        /// <summary>
        /// 代理请求超时时间（毫秒）
        /// </summary>
        public int TimeOut { get; set; } = 10000;

        #endregion
    }
}
