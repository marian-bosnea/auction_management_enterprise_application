namespace DataMapper
{
    using DomainModel;
    using Microsoft.EntityFrameworkCore;

    public class AuctionManagementEfCoreDbContext : DbContext, IAuctionManagementEfCoreDbContext
    {
        public AuctionManagementEfCoreDbContext(DbContextOptions<AuctionManagementEfCoreDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Auction> Auctions { get; set; }

        public DbSet<Bid> Bids { get; set; }

        public DbSet<Person> People { get; set; }
    }
}
