using InventoryAPI.Data;
using InventoryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly InventoryContext _context;

        public ItemsController(InventoryContext context)
        {
            _context = context;
        }

        // Get all items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _context.Items.ToListAsync();
        }

        // Get a single item by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // Add a new item
        [HttpPost]
        public async Task<ActionResult<Item>> AddItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        // Update an existing item
       [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int Id, Item newItem)
        {
            var item =  await _context.Items.FindAsync(Id);
            if (item.Id != Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                item.Name =newItem.Name;
                item.Quantity =newItem.Quantity;
                item.Price = newItem.Price;

                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating item: {ex.Message}");
                return StatusCode(500, "Error updating item");
            }
        }

        // Delete a single item by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete all items
        [HttpDelete]
        [Route("delete-all")]
        public async Task<IActionResult> DeleteAllItems()
        {
            var items = await _context.Items.ToListAsync();
            _context.Items.RemoveRange(items);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper method to check if an item exists
        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
