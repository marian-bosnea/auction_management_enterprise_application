namespace DataMapper
{
    using System.Threading;
    using System.Threading.Tasks;
    using DomainModel;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public interface IAuctionManagementEfCoreDbContext
    {
        DbSet<Product> Products { get; set; }

        DbSet<Category> Categories { get; set; }

        DbSet<Auction> Auctions { get; set; }

        DbSet<Bid> Bids { get; set; }

        DbSet<Person> People { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
