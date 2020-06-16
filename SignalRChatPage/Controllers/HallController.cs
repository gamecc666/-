using Microsoft.AspNetCore.Mvc;
using SignalRChatPage.LibFiles;
using SignalRChatPage.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SignalRChatPage.Controllers
{
    public class HallController : Controller
    {
        public IActionResult Index()
        {            
            return View();
        }       

        public async Task<IActionResult> Book(string id)
        {        
            if (!string.IsNullOrEmpty(id))
            {
                MeetInfoModel model = new MeetInfoModel();
                await Task.Run(() =>
                {
                    var sqlobj = new SqlOperation();
                    sqlobj.OpenConnection();
                    string sqlstr = $"select * from MeetInfo where RoomNo='{id}'";
                    SqlDataReader dbdata = sqlobj.ExecuteQuery(sqlstr);
                    if (dbdata.HasRows)
                    {
                        while (dbdata.Read())
                        {                            
                            model.RoomNo = dbdata["RoomNo"].ToString();
                            model.Theme = dbdata["Theme"].ToString();
                            model.StartTime = dbdata.GetDateTime(2);                            
                            model.EndTime = dbdata.GetDateTime(3);
                            model.Password = dbdata["Password"].ToString();                   
                        }
                    }
                    sqlobj.CloseConnection();
                });

                return View(model);
            }

            return View();
        }
        [HttpPost]
        public async Task <ActionResult> Book([ModelBinder] MeetInfoModel meetinfo)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //TODO:首先判断数据库里面有没有该数据；有的话属于更新，没的话就是插入
            Console.WriteLine($"---输出提交信息的房间号:{meetinfo.RoomNo}");
            SqlOperation DB = new SqlOperation();
            DB.OpenConnection();
            string commondStr = $"select * from MeetInfo MI where MI.RoomNo='{meetinfo.RoomNo}'";
            SqlDataReader sqlQueryResult= DB.ExecuteQuery(commondStr);
            if (sqlQueryResult.HasRows)
            {
                //更新操作
                await Task.Run(()=> {
                    string _commondStr = $"update MeetInfo set Theme='{meetinfo.Theme}'," +
                                        $"StartTime='{meetinfo.StartTime}',EndTime='{meetinfo.EndTime}'," +
                                        $" Password='{meetinfo.Password}' where MeetInfo.RoomNo='{meetinfo.RoomNo}'";
                    DB.ExecuteQuery(_commondStr);
                });
            }
            else
            {
                //插入操作
                await Task.Run(() => {
                    string sqlstr = $"insert into MeetInfo  values('{meetinfo.RoomNo}','{meetinfo.Theme}','{meetinfo.StartTime}','{meetinfo.EndTime}','{meetinfo.Password}')";
                    DB.ExecuteQuery(sqlstr);
                });
            }

            DB.CloseConnection();
            
            return View(nameof(Index));
        }    
        //获得初始化页面数据
        [HttpGet]
        public async Task<JsonResult> GetInitData()
        {
            //查找数据库中的所有预定义会议（个人）
            var data = await Task.Run(()=> {               
               List<MeetInfoModel> list_model = new List<MeetInfoModel>();
               var sqlobj = new SqlOperation();
               sqlobj.OpenConnection();
               string sqlstr = $"select * from MeetInfo";
               SqlDataReader dbdata= sqlobj.ExecuteQuery(sqlstr);               
               if (dbdata.HasRows)
               {
                   while (dbdata.Read())
                   {
                        MeetInfoModel model = new MeetInfoModel {
                            RoomNo = dbdata["RoomNo"].ToString(),
                            Theme = dbdata["Theme"].ToString(),
                            StartTime = Convert.ToDateTime(dbdata["StartTime"]),                            
                            EndTime = Convert.ToDateTime(dbdata["EndTime"]),
                            Password = dbdata["Password"].ToString(),
                       };
                       list_model.Add(model);
                   }
               }
               sqlobj.CloseConnection();

               return list_model;
            });

            return Json(data);
        }
        //删除数据库中对应的meetinfo数据
        [HttpGet]
        public async Task<JsonResult> DeleteDataFromMeetInfo(string roomno)
        {
            var status = new StatusCollections();
            await Task.Run(()=> {
                var sqlobj = new SqlOperation();
                try { 
                    string sqlstr = $"delete from MeetInfo where RoomNo='{roomno}'";                    
                    sqlobj.OpenConnection();
                    sqlobj.ExecuteQuery(sqlstr);
                    status.Code = 1;
                    status.CodeInfo = "删除成功";
                }
                catch(Exception ex)
                {
                    status.Code = -1;
                    status.CodeInfo = ex.ToString();
                }
                finally
                {
                    sqlobj.CloseConnection();                      
                }
            });     

            return Json(status);
        } 
        //验证房间号是否已经存在
        [AcceptVerbs("Get","Post")]
        public IActionResult VerifyRoomNo(string roomno)
        {
            bool isHas = false;
            SqlOperation DB = new SqlOperation();
            DB.OpenConnection();
            string sqlCommond = $"select MeetInfo.RoomNo from MeetInfo where MeetInfo.RoomNo='{roomno}'";
            SqlDataReader queryResult = DB.ExecuteQuery(sqlCommond);
            isHas = queryResult.HasRows ? true : false;
            DB.CloseConnection();
            if(isHas)
            {
                return Json($"房间号:{roomno}已经存在");
            }
            return Json(true);
        }
    }
}