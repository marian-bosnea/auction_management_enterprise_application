namespace DataMapper
{
    using System.Data.Common;
    using System.Data.Entity;
    using DataMapper.Interfaces;
    using DomainModel;

    /// <summary>
    /// Represents the Entity Framework Core database context for auction management.
    /// </summary>
    public class AuctionManagementEfCoreDbContext : DbContext, IAuctionManagementEfCoreDbContext
    {
        public AuctionManagementEfCoreDbContext(DbConnection connection)
        : base(connection, contextOwnsConnection: false) // Set contextOwnsConnection to false if you don't want the context to own the connection
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Product}"/> representing products in the database.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Category}"/> representing categories in the database.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Auction}"/> representing auctions in the database.
        /// </summary>
        public DbSet<Auction> Auctions { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Bid}"/> representing bids in the database.
        /// </summary>
        public DbSet<Bid> Bids { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Person}"/> representing people in the database.
        /// </summary>
        public DbSet<Person> People { get; set; }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
