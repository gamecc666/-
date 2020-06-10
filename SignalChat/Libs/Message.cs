using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalChat.Libs
{
    public enum ActionOper:int
    {
        InGroupOPeration=1,
        OutGroupOperation=2,
        SendGroupOperation=3
    }
    public class Message
    {
        public string roomno { get; set; }
        public string nick { get; set; }
        public int action { get; set; }
        public string msg { get; set; }        
    }
}
