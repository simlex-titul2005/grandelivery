using grandelivery.WebUI.Models;
using SX.WebCore;
using System.Data.Entity;

namespace grandelivery.WebUI.Infrastructure
{
    public sealed class DbContext : SxDbContext
    {
        public DbContext() : base("DbContext") { }

        public DbSet<Order> Orders { get; set; }
    }
}