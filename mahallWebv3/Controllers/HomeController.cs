using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mahallWebv3.Models;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;

namespace mahallWebv3.Controllers
{
    public class HomeController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "KyLluMEiVGhkKTopEyEMUgQEqvVmIGcWu3ByqziR",
            BasePath = "https://mahallsubscription.firebaseio.com/",
        };
        IFirebaseClient client;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("Create");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Subscription subscription)
        {
            try
            {
                AddSubscriptionToFirebase(subscription);
                ModelState.AddModelError(string.Empty, "Added Successfully");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

        private void AddSubscriptionToFirebase(Subscription subscription)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = subscription;
            PushResponse response = client.Push("Subscriptions/", data);
            data.subscription_id = response.Result.name;
            SetResponse setResponse = client.Set("Subscriptions/" + data.subscription_id, data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
