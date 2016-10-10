using grandelivery.WebUI.Models;
using SX.WebCore;
using System.Data.Entity;

namespace grandelivery.WebUI.Infrastructure
{
    public sealed class DbContext : SxDbContext
    {
        public DbContext() : base("DbContext") { }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderTrack> OrderTracks { get; set; }

        public new DbSet<SiteService> SiteServices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderTrack>().HasRequired(x => x.Order).WithMany().HasForeignKey(x => new { x.OrderId }).WillCascadeOnDelete(true);
            modelBuilder.Entity<OrderTrack>().HasRequired(x => x.User).WithMany().HasForeignKey(x => new { x.UserId }).WillCascadeOnDelete(false);
        }
    }
}