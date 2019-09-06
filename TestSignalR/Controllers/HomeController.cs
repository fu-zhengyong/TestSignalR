using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestSignalR.Hubs;
using TestSignalR.Models;

namespace TestSignalR.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ClientName = "聊客-" + Guid.NewGuid().ToString("N");
            var onLineUserList = ChatHub.OnLineUsers.Select(u => new SelectListItem() { Text = u.Value, Value = u.Key }).ToList();
            onLineUserList.Insert(0, new SelectListItem() { Text = "-所有人-", Value = "" });
            ViewBag.OnLineUsers = onLineUserList;
            return View();
        }


    }
}
