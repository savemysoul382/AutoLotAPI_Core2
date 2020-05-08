using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoLotDAL_Core2.Models;
using AutoLotDAL_Core2.Repos;
using AutoMapper;
using Newtonsoft.Json;

namespace AutoLotAPI_Core2.Controllers
{
    [Produces("application/json")]
    [Route("api/Inventory")]
    public class InventoryController : Controller
    {
        private readonly IInventoryRepo repo;

        public InventoryController(IInventoryRepo repo)
        {
            this.repo = repo;
            Mapper.Initialize(
                cfg =>
                {
                    cfg.CreateMap<Inventory, Inventory>()
                        .ForMember(x => x.Orders, opt => opt.Ignore());
                });
        }

        // GET: api/Inventory
        [HttpGet]
        public IEnumerable<Inventory> GetCars()
        {
            var inventories = this.repo.GetAll();
            return Mapper.Map<List<Inventory>, List<Inventory>>(inventories);
        }

        // GET: api/Inventory/5
        [HttpGet("{id}", Name = "DisplayRoute")]
        public async Task<IActionResult> GetInventoryAsync([FromRoute] int id)
        {
            Inventory inventory = this.repo.GetOne(id);

            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Inventory, Inventory>(inventory));
        }

        // PUT: api/Inventory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryAsync([FromRoute] int id, [FromBody] Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != inventory.Id)
            {
                return BadRequest();
            }

            this.repo.Update(inventory);
            return NoContent();
        }

        // POST: api/Inventory
        [HttpPost]
        public async Task<IActionResult> PostInventoryAsync([FromBody] Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            this.repo.Add(inventory);
            return CreatedAtRoute("DisplayRoute", new { id = inventory.Id }, inventory);

            //return CreatedAtAction("GetInventory", new { id = inventory.Id }, inventory);
        }

        // DELETE: api/Inventory/5
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> DeleteInventoryAsync([FromRoute] int id, [FromRoute] string timestamp)
        {
            if (!timestamp.StartsWith("\""))
            {
                timestamp = $"\"{timestamp}\"";
            }
            var ts = JsonConvert.DeserializeObject<byte[]>(timestamp);

            this.repo.Delete(id, ts);
            return Ok();
        }

    }
}