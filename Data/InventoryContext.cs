using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Data
{
    public class InventoryContext : DbContext{

        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base (options){}

        public DbSet<Item> Items {get; set;}
    }
}