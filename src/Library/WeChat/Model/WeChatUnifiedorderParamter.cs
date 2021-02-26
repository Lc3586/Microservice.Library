using Senparc.Weixin.TenPay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 统一下单参数
    /// </summary>
    public class WeChatUnifiedorderParamter
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="outTradeNo">商家订单号</param>
        /// <param name="totalFee">商品金额, 以分为单位</param>
        /// <param name="body">商品信息</param>
        /// <param name="productId">二维码中包含的商品ID，商户自行定义。 </param>
        /// <param name="tradeType">交易类型</param>
        /// <returns></returns>
        public static WeChatUnifiedorderParamter GetSimpleParamter(string outTradeNo, int totalFee, string body, string productId, TenPayV3Type tradeType = TenPayV3Type.NATIVE)
        {
            return new WeChatUnifiedorderParamter
            {
                TradeType = tradeType,
                OutTradeNo = outTradeNo,
                TotalFee = totalFee,
                Body = body,
                ProductId = productId
            };
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="openId">用户的openId</param>
        /// <param name="outTradeNo">商家订单号</param>
        /// <param name="totalFee">商品金额, 以分为单位</param>
        /// <param name="body">商品信息</param>
        /// <param name="productId">二维码中包含的商品ID，商户自行定义。 </param>
        /// <param name="tradeType">交易类型</param>
        /// <returns></returns>
        public static WeChatUnifiedorderParamter GetSimpleParamter(string openId, string outTradeNo, int totalFee, string body, string productId, TenPayV3Type tradeType = TenPayV3Type.JSAPI)
        {
            return new WeChatUnifiedorderParamter
            {
                TradeType = tradeType,
                OutTradeNo = outTradeNo,
                TotalFee = totalFee,
                Body = body,
                ProductId = productId,
                OpenId = openId
            };
        }

        #region 必填

        /// <summary>
        /// 商家订单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 商品金额,
        /// 以分为单位
        /// </summary>
        public int TotalFee { get; set; }

        /// <summary>
        /// 商品信息
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public TenPayV3Type TradeType { get; set; }

        /// <summary>
        /// trade_type=NATIVE时（即扫码支付），此参数必传。
        /// 此参数为二维码中包含的商品ID，商户自行定义。 
        /// String(32)，如：12235413214070356458058
        /// </summary>
        public string ProductId { get; set; }

        #endregion

        #region 选填

        /// <summary>
        /// 用户的openId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 商品标记，使用代金券或立减优惠功能时需要的参数，说明详见代金券或立减优惠。String(32)，如：WXG
        /// </summary>
        public string GoodsTag { get; set; }

        /// <summary>
        /// 订单生成时间，
        /// 最终生成格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见时间规则。
        /// 如果为空，则默认为当前服务器时间
        /// </summary>
        public DateTimeOffset? TimeStart { get; set; }

        /// <summary>
        /// 订单失效时间，
        /// 格式为yyyyMMddHHmmss，如2009年12月27日9点10分10秒表示为20091227091010。其他详见时间规则 注意：最短失效时间间隔必须大于5分钟。
        /// 留空则不设置失效时间
        /// </summary>
        public DateTime? TimeExpire { get; set; }

        /// <summary>
        /// 场景信息，
        /// 目前支持上报实际门店信息。
        /// </summary>
        public WeChatUnifiedorderSceneInnfo SceneInfo { get; set; }

        /// <summary>
        /// 上传此参数no_credit--可限制用户不能使用信用卡支付
        /// </summary>
        public bool LimitPay { get; set; } = false;

        /// <summary>
        /// <see cref="FreeType"/>
        /// 符合ISO 4217标准的三位字母代码，默认人民币：CNY，详细列表请参见货币类型。String(16)，如：CNY
        /// </summary>
        public string FeeType { get; set; }

        /// <summary>
        /// 附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用。String(127)，如：深圳分店
        /// </summary>
        public string Attach { get; set; }

        /// <summary>
        /// 商品详细列表
        /// String(6000)
        /// </summary>
        /// <remarks>
        /// <![CDATA[{}]]>
        /// 使用Json格式，传输签名前请务必使用CDATA标签将JSON文本串保护起来。 
        /// cost_price Int 可选 32 订单原价，商户侧一张小票订单可能被分多次支付，订单原价用于记录整张小票的支付金额。当订单原价与支付金额不相等则被判定为拆单，无法享/受/优/惠。
        /// receipt_id String 可选 32 商家小票ID 
        /// goods_detail 服务商必填
        /// []： 
        /// └ goods_id String 必填 32 商品的编号 
        /// └ wxpay_goods_id String 可选 32 微信支付定义的统一商品编号 
        /// └ goods_name String 可选 256 商品名称 
        /// └ quantity Int 必填 32 商品数量 
        /// └ price Int 必填 32 商品单价，如果商户有优惠，需传输商户优惠后的单价 注意：单品总金额应小于或等于订单总金额total_fee，否则会无法享受优惠。
        /// </remarks>
        public WeChatUnifiedorderDetail Detail { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; }

        /// <summary>
        /// 自定义参数，
        /// 可以为终端设备号(门店号或收银设备ID)，
        /// PC网页或公众号内支付可以传"WEB"，
        /// String(32)如：013467007045764
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProfitSharing { get; set; }

        #endregion
    }
}
