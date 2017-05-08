using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microphone;
using Refit;

namespace WebAppMicroService.Controllers
{
    public class HomeController : Controller
    {
        IClusterClient client { get; set; }
        public IActionResult Index([FromServices]IClusterClient client)
        {
            var data = client.GetServiceInstancesAsync("environment").Result;
            var serv = data.First();
            var apiMicro = RestService.For<IEnvironnementAPI>(string.Format("http://{0}:{1}", serv.Host, serv.Port));
            ViewData["Message"] = string.Join(",", apiMicro.GetData().Result);
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }


    }

    public interface IEnvironnementAPI
    {
        [Get("/api/values")]
        Task<IEnumerable<string>> GetData();
    }
}
