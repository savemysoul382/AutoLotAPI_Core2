using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoLotDAL_Core2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AutoLotMVC_Core2.ViewComponents
{
    public class InventoryViewComponent : ViewComponent
    {
        private readonly String base_url;

        public InventoryViewComponent(IConfiguration configuration)
        {
            this.base_url = configuration.GetSection(key: "ServiceAddress").Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(requestUri: this.base_url);
            if (response.IsSuccessStatusCode)
            {
                String json = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<List<Inventory>>(value: json);
                return View(viewName: "InventoryPartialView", model: items);
            }
            return new ContentViewComponentResult(content: "Unable to return records.");
        }
    }
}