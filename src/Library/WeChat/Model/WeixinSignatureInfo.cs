using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    public class WeixinSignatureInfo
    {
        public string Ticket { get; set; }
        public string Timestamp { get; set; }
        public string NonceStr { get; set; }
        public string Signature { get; set; }
    }
}
