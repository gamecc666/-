using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace NetMeetingPro.Controllers
{
    public class EntranceController : Controller
    {
        public IActionResult Index()
        {
            int a = 1;
            Debug.Assert(a == 2);
            return View();
        }
    }
}