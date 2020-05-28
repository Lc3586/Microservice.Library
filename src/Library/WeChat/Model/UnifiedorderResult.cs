using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    public class UnifiedorderResult
    {
        public string AppId { get; set; }
        public string TimeStamp { get; set; }
        public string NonceStr { get; set; }
        public string Package { get; set; }
        public string PaySign { get; set; }
        public string CodeUrl { get; set; }

        public string Data0 { get; set; }
    }
}
