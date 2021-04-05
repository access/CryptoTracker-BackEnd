using Microsoft.EntityFrameworkCore;

namespace CryptocurrencyTracker.Models
{
    public class CryptoTrackerDbContext : DbContext
    {
        public DbSet<CryptoCurrencyItem> CryptoCurrencyItems { get; set; }
        public DbSet<CryptoCurrencyValue> CryptoCurrencyValues { get; set; }

        public CryptoTrackerDbContext(DbContextOptions<CryptoTrackerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // change default precision when store to DB DECIMAL (10,8)
            modelBuilder.Entity<CryptoCurrencyValue>().Property(o => o.MarketValue).HasPrecision(10, 8);
            modelBuilder.Entity<CryptoCurrencyItem>().Property(o => o.LastTradeRate).HasPrecision(10, 8);
        }
    }
}
