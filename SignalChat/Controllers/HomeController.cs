using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalChat.Models;

namespace SignalChat.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Entrance()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Entrance_post()
        {
            var username = Request.Form["username"];
            var userpwd = Request.Form["userpwd"];

            string userId = Guid.NewGuid().ToString().Replace("-", "");
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,username),   //储存用户name
                new Claim(ClaimTypes.NameIdentifier,userId)  //储存用户id
            };
            //身份【似身份证，多个声明（姓名，民族等）构成】
            var indentity = new ClaimsIdentity(claims, "formlogin");
            //证件所有者【类似身份证所有者】
            var claimPrincipal = new ClaimsPrincipal(indentity);
            //SignInAsync 认证成功会办法一个加密的凭证
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);
            if (claimPrincipal.Identity.IsAuthenticated)
            {
                return Json(new { code = "success", msg = "登陆成功" });
            }
            else
                return Json(new { code = "failed", msg = "登陆失败" });

        }
        //点击登录聊天功能按钮之后跳转的界面
        [Authorize]
        public IActionResult Index()
        {                        
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
