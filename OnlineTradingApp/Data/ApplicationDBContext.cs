using Microsoft.EntityFrameworkCore;
using OnlineTradingApp.Models;

namespace OnlineTradingApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<PortfolioShare> PortfolioShares { get; set; }
        public DbSet<Transaction> Transactions { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Any additional configuration
        }
    }

}
