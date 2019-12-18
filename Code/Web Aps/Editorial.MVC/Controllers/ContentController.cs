using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Editorial.MVC.Models.Content;
using Editorial.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net.Http;
using Newtonsoft.Json;
using Content.Models;

namespace Editorial.MVC.Controllers
{
    public class ContentController : Controller
    {
        private readonly IContentHttpClient _contentHttpClient;

        public ContentController(IContentHttpClient contentHttpClient)
        {
            _contentHttpClient = contentHttpClient;
        }

        public IActionResult Create()
        {
            ContentViewModel model = new ContentViewModel();
            model.PublicationDateTime = DateTime.UtcNow;
            return View("Edit", model);
        }

        public async Task<IActionResult> Edit(Guid Id)
        {
            var httpClient = await _contentHttpClient.GetClient();
            var response = await httpClient.GetAsync($"contents/v1/GetContent/{Id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                string strContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var ContentViewModel = JsonConvert.DeserializeObject<ContentViewModel>(strContent);
                return View(ContentViewModel);
            }
            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAsDraft(ContentViewModel model)
        {

            model.FullText = System.Net.WebUtility.HtmlDecode(model.FullText);
            model.InBrief = System.Net.WebUtility.HtmlDecode(model.InBrief);

            // serialize it
            var serializedContent = JsonConvert.SerializeObject(model);

            //call API
            var httpClient = await _contentHttpClient.GetClient();
            HttpResponseMessage response;

            if (string.IsNullOrEmpty(model.Id))
            {
                //Create
                response = await httpClient.PostAsync(
                   $"contents/v1/Create",
                   new StringContent(serializedContent, System.Text.Encoding.Unicode, "application/json"))
                   .ConfigureAwait(false);
            }
            else
            {
                //Update
                response = await httpClient.PutAsync(
                   $"contents/v1/Update",
                   new StringContent(serializedContent, System.Text.Encoding.Unicode, "application/json"))
                   .ConfigureAwait(false);
            }

            if (response.IsSuccessStatusCode)
            {
                return Redirect("~/");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAndPublish(ContentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Info.  
                System.Diagnostics.Debug.WriteLine(model.Title);
            }
            return View();
        }
    }
}