// <copyright file="AuctionManagementEfCoreDbContext.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper
{
    using System.Data.Entity;
    using DomainModel;

    /// <summary>
    /// Represents the Entity Framework Core database context for auction management.
    /// </summary>
    public class AuctionManagementEfCoreDbContext : DbContext
    {
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
    }
}
