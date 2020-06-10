using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using SignalChat.Libs;
using Newtonsoft.Json;
using System.Security.Claims;

namespace SignalChat.Hubs
{
    /*
     *ChatHub继承自SignalR Hub类，Hub 管理连接，组和消息
     *      可通过已连接的客户端调用Sendmessage，向所有的客户端发送消息
     *      SignalR代码是异步模式，可提供最大的可伸缩性
     *技术注意:
     *      不要将状态存储在hub类的属性中     
     *      组名区分大小写
     *      连接重连后，不会保留组成员身份。重新建立组后，连接需要重新加入组     
     *代码结构：
     *      可以重构，统一进入一个方法里面，然后分开处理（高内聚低耦合）
     */
    public class ChatHub:Hub
    {
        #region 代码重构
        public async Task HandleClientMsg(string msg)
        {
            Console.WriteLine("统一从客户端收到的信息："+ msg);
            Message message = JsonConvert.DeserializeObject<Message>(msg);
            Client _client = new Client(Context.ConnectionId, message.nick, message.roomno);
            string path = $"./ChatLogs/{message.nick}_{message.roomno}.txt";            
            switch (message.action)
            {
                case (int)ActionOper.InGroupOPeration:
                    int codeStatus= GroupManager.AddToRPD(message.roomno, message.password);
                    if(codeStatus==(int)ActionOper.FailJoinGroup)
                    {
                        string tipInfo = $"操作有误，请检查房间密码是否有误！";
                        await SendPrivateMessage(Context.UserIdentifier, tipInfo);                       
                    }
                    else
                    {
                        GroupManager.Add(_client);
                        string inGroupInfo = $"用户 {message.nick} 加入房间 {message.roomno}";
                        FileHelp.CreateFile(path, inGroupInfo);
                        Console.WriteLine(inGroupInfo);
                        await Groups.AddToGroupAsync(Context.ConnectionId, message.roomno);
                        await Clients.Group(message.roomno).SendAsync("ReceiveMessage", inGroupInfo);
                    }                       
                    break;
                case (int)ActionOper.OutGroupOperation:
                    GroupManager.Remove(_client);                    
                    string outGroupInfo = $"用户 {message.nick} 离开房间 {message.roomno}";
                    var list = GroupManager.GetClientsInGroup(message.roomno);
                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            string _path = $"../../../ChatLogs/{item.nickName}_{message.roomno}.txt";

                            await Task.Factory.StartNew(() =>
                            {
                                FileHelp.AppendToFile(_path, outGroupInfo);
                            });
                        }
                    }

                    Console.WriteLine(outGroupInfo);
                    await Clients.Group(message.roomno).SendAsync("ReceiveMessage", outGroupInfo);
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, message.roomno);
                    break;
                case (int)ActionOper.SendGroupOperation:
                    string sendGroupInfo = $"{message.nick} : {message.msg}";
                    FileHelp.AppendToFile(path, sendGroupInfo);
                    Console.WriteLine(sendGroupInfo);
                    await Clients.Group(message.roomno).SendAsync("ReceiveMessage", sendGroupInfo);
                    break;
            }
            GroupManager.OutputRoomInfo();
        }
        #endregion 
        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine("------------输出connectionid:" + Context.ConnectionId);
            await Clients.All.SendAsync("ReceiveMessage", user, message);         
        }             

        public async Task SendPrivateMessage(string userId, string message)
        {
            Console.WriteLine($"开始发送给id为{userId}的用户！");
            await Clients.User(userId).SendAsync("TipsMessage", message);
        }

        #region 连接相关事件自动监听不需重构
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"------------ConnectionId：{Context.ConnectionId} 连接成功！");
            Console.WriteLine($"------------UserIdentifier：{Context.UserIdentifier}");
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"------------用户 {Context.ConnectionId} 断开连接！");
            return base.OnDisconnectedAsync(exception);
        }
        #endregion
    }
}
