using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalChat.Libs
{
    public enum ActionOper:int
    {
        InGroupOPeration=1,
        OutGroupOperation=2,
        SendGroupOperation=3,
        BuildGroupSuccess=4,
        SuccessJoinGroup = 5,
        FailJoinGroup = 6,
        RemoveSuccess=7,
        RemoceFail=8
    }    
    public class Message
    {
        public string roomno { get; set; }
        public string nick { get; set; }
        public int action { get; set; }
        public string msg { get; set; }        
        public string password { get; set; }
    }
    public class Client
    {
        public Client(string connectionID, string nickName, string groupNo)
        {
            this.connectionID = connectionID;
            this.nickName = nickName;
            this.groupNo = groupNo;
        }

        public string connectionID { get; set; }
        public string nickName { get; set; }
        public string groupNo { get; set; }
    }
    public class GroupManager
    {
        #region 房间人员管理
        private static Dictionary<string, List<Client>> _groupDic =new Dictionary<string, List<Client>>();
        public static void Add(Client client)
        {
            if (_groupDic.ContainsKey(client.groupNo))
            {
                _groupDic[client.groupNo].Add(client);
            }
            else
            {
                List<Client> clients = new List<Client>();
                clients.Add(client);
                _groupDic.Add(client.groupNo, clients);                
            }            
        }
        public static void Remove(Client client)
        {
            if (_groupDic.ContainsKey(client.groupNo))
            {
                var res = _groupDic[client.groupNo].Where(p => p.connectionID == client.connectionID).FirstOrDefault();
                if (res != null)
                {
                    _groupDic[client.groupNo].Remove(res);
                }                
            }
            else
            {
                Console.WriteLine("请确定该成员是否存在该组！！");
            }
        }
        public static List<Client> GetClientsInGroup(string roomno)
        {
            List<Client> clientList = new List<Client>();
            if (_groupDic.ContainsKey(roomno))
            {
                clientList = _groupDic[roomno];
                return clientList;
            }           
            return null;
        }
        public static void OutputRoomInfo()
        {            
            foreach(KeyValuePair<string,List<Client>> kv in _groupDic)
            {
                Console.WriteLine($"房间号：{kv.Key}");
                foreach(var item in kv.Value)
                {
                    Console.WriteLine($"成员有：{item.nickName}");
                }
            }
        }
        #endregion
        #region 房间密码管理
        private static Dictionary<string, string> _roomPasswordDic = new Dictionary<string, string>();
        public static int AddToRPD(string roomNo,string password)
        {
            int status = (int)ActionOper.FailJoinGroup;
            if (_roomPasswordDic.ContainsKey(roomNo))
            {
                Console.WriteLine("房间密码管理字段里面已经存在该房间以及密码");
                if(_roomPasswordDic[roomNo]==password)
                {
                    status = (int)ActionOper.SuccessJoinGroup;
                }                
            }
            else
            {
                Console.WriteLine("房间密码管理字典里面不存在该房间以及密码");
                _roomPasswordDic.Add(roomNo, password);
                status = (int)ActionOper.BuildGroupSuccess;

            }
            return status;
        }
        public static int RemoveFromRPD(string roomNo)
        {
            int status = (int)ActionOper.FailJoinGroup;
            if (_roomPasswordDic.ContainsKey(roomNo))
            {
                _roomPasswordDic.Remove(roomNo);
                status = (int)ActionOper.RemoveSuccess;
            }
            else
            {               
                status = (int)ActionOper.RemoceFail;

            }
            return status;
        }
        #endregion
    }
}
