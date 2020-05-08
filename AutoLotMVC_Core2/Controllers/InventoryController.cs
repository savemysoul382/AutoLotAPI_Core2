using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoLotDAL_Core2.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AutoLotMVC_Core2.Controllers
{
    public class InventoryController : Controller
    {
        private readonly String base_url;

        public InventoryController(IConfiguration configuration)
        {
            this.base_url = configuration.GetSection(key: "ServiceAddress").Value;
        }
        private async Task<Inventory> GetInventoryRecordAsync(Int32 id)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(requestUri: $"{this.base_url}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var inventory = JsonConvert.DeserializeObject<Inventory>(value: await response.Content.ReadAsStringAsync());
                return inventory;
            }
            return null;
        }
        public async Task<IActionResult> IndexAsync()
        {
            //var client = new HttpClient();
            //var response = await client.GetAsync(this.base_url);
            //if (response.IsSuccessStatusCode)
            //{
            //    var items = JsonConvert.DeserializeObject<List<Inventory>>(await response.Content.ReadAsStringAsync());
            //    return View(items);
            //}
            //return NotFound();

            return View(viewName: "IndexWithViewComponent");
        }
        public async Task<IActionResult> DetailsAsync(Int32? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var inventory = await GetInventoryRecordAsync(id: id.Value);
            return inventory != null ? (IActionResult)View(model: inventory) : NotFound();
        }
        // GET: Inventory/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Inventory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([Bind("Make,Color,PetName")] Inventory inventory)
        {
            if (!ModelState.IsValid) return View(model: inventory);
            try
            {
                var client = new HttpClient();
                String json = JsonConvert.SerializeObject(value: inventory);
                var response = await client.PostAsync(requestUri: this.base_url,
                    content: new StringContent(content: json, encoding: Encoding.UTF8, mediaType: "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(actionName: nameof(IndexAsync));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(key: String.Empty, errorMessage: $"Unable to create record: {ex.Message}");
            }
            return View(model: inventory);
        }

        // GET: Inventory/Edit/5
        public async Task<IActionResult> EditAsync(Int32? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var inventory = await GetInventoryRecordAsync(id: id.Value);
            return inventory != null ? (IActionResult)View(model: inventory) : NotFound();
        }
        // POST: Inventory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(Int32 id, [Bind("Make,Color,PetName,Id,Timestamp")] Inventory inventory)
        {
            if (id != inventory.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model: inventory);
            var client = new HttpClient();
            String json = JsonConvert.SerializeObject(value: inventory);
            var response = await client.PutAsync(requestUri: $"{this.base_url}/{inventory.Id}",
                content: new StringContent(content: json, encoding: Encoding.UTF8, mediaType: "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(actionName: nameof(IndexAsync));
            }
            return View(model: inventory);
        }

        // GET: Inventory/Delete/5
        public async Task<IActionResult> DeleteAsync(Int32? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var inventory = await GetInventoryRecordAsync(id: id.Value);
            return inventory != null ? (IActionResult)View(model: inventory) : NotFound();
        }

        // POST: Inventory/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync([Bind("Id,Timestamp")] Inventory inventory)
        {
            var client = new HttpClient();
            var time_stamp_string = JsonConvert.SerializeObject(value: inventory.Timestamp);
            HttpRequestMessage request =
                new HttpRequestMessage(method: HttpMethod.Delete, requestUri: $"{this.base_url}/{inventory.Id}/{time_stamp_string}");
            await client.SendAsync(request: request);
            return RedirectToAction(actionName: nameof(IndexAsync));
        }
    }
}