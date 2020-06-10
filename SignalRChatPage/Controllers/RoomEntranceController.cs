using Microsoft.AspNetCore.Mvc;
using SignalRChatPage.Models;
using System;

namespace SignalRChatPage.Controllers
{
    public class RoomEntranceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(nameof(Login));
        }
        [HttpPost]
        public IActionResult Login([ModelBinder]RoomModel roommodel)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("模型验证未通过");
                return View();
            }

            Console.WriteLine("模型验证通过");
            return View();
        }


        [AcceptVerbs("GET", "POST")]
        public IActionResult VeryName(string NickName)
        { 
            if(string.IsNullOrEmpty(NickName))
            {
                return Json("名字不能为空！！！！！！");
            }

            return Json(true);
        }
    }
}