﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AutoLotMVC_Core2.Models;

namespace AutoLotMVC_Core2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData[index: "Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData[index: "Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(model: new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
