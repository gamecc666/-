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

        public IActionResult Book()
        {
            return View();
        }
        [HttpPost]
        public async Task <ActionResult> Book([ModelBinder] MeetInfoModel meetinfo)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await Task.Run(()=> {
                //成功的话直接写在数据库里
                var sqlobj = new SqlOperation();
                sqlobj.OpenConnection();
                string sqlstr = $"insert into MeetInfo  values('{meetinfo.RoomNo}','{meetinfo.Theme}','{meetinfo.StartTime}','{meetinfo.EndTime}','{meetinfo.Password}')";
                sqlobj.ExecuteQuery(sqlstr);
                sqlobj.CloseConnection();
            });
            
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
    }
}