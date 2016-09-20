using SX.WebCore;

namespace grandelivery.WebUI.Infrastructure
{
    public sealed class DbContext : SxDbContext
    {
        public DbContext() : base("DbContext") { }
    }
}