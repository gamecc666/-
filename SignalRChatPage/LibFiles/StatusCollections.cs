using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChatPage.LibFiles
{
    /// <summary>
    /// -1:失败
    /// 0：初始化
    /// 1：成功
    /// </summary>
    public class StatusCollections
    {
        public int Code { get; set; }
        public string CodeInfo { get; set; }

        public StatusCollections()
        {
            Code = 0;
            CodeInfo = "";
        }
    }
}
