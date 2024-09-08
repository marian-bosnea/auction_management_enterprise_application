// <copyright file="IAuctionManagementDbContext.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using DomainModel;

    /// <summary>
    /// Interface representing the contract for the AuctionManagement database context.
    /// </summary>
    public interface IAuctionManagementEfCoreDbContext
    {
        /// <summary>
        /// Gets or sets the <see cref="DbSet{Auction}"/> representing auctions in the database.
        /// </summary>
        DbSet<Auction> Auctions { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Product}"/> representing products in the database.
        /// </summary>
        DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Category}"/> representing categories in the database.
        /// </summary>
        DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Bid}"/> representing bids in the database.
        /// </summary>
        DbSet<Bid> Bids { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Person}"/> representing people in the database.
        /// </summary>
        DbSet<Person> People { get; set; }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        int SaveChanges();



        DbEntityEntry Entry(object entity);

    }
}
