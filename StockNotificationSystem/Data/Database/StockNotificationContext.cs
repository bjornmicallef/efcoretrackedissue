using Microsoft.EntityFrameworkCore;

namespace Data.Database
{
    public partial class StockNotificationContext : DbContext
    {
        public StockNotificationContext()
        {
        }

        public StockNotificationContext(DbContextOptions<StockNotificationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<tblSellers> tblSellers { get; set; }
        public virtual DbSet<tblExcludedSellers> tblExcludedSellers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblExcludedSellers>(entity =>
            {
                entity.HasKey(e => new { e.SellerId, e.Username });
            });
        }
    }
}
