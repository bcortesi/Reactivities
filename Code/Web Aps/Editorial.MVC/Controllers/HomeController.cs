using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Editorial.MVC.Models;
using Editorial.MVC.Services;
using Newtonsoft.Json;
using Editorial.MVC.Models.Content;
using Content.Models;

namespace Editorial.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContentHttpClient _contentHttpClient;

        public HomeController(ILogger<HomeController> logger, IContentHttpClient contentHttpClient)
        {
            _logger = logger;
            _contentHttpClient = contentHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = await _contentHttpClient.GetClient();
            var response = await httpClient.GetAsync("contents/v1/GetContents").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                string contents = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var IndexViewModel = new ListViewModel(
                    JsonConvert.DeserializeObject<IList<ContentModel>>(contents).ToList());

                return View(IndexViewModel);
            }
            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
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
