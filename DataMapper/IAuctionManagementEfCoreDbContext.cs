// <copyright file="IAuctionManagementEfCoreDbContext.cs" company="Transilvania University of Brasov">
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

        /// <summary>
        /// Gets the <see cref="DbEntityEntry"/> for the given entity, allowing for access to its state and metadata.
        /// </summary>
        /// <param name="entity">The entity for which to get the <see cref="DbEntityEntry"/>.</param>
        /// <returns>
        /// A <see cref="DbEntityEntry"/> instance that provides access to the state and metadata of the given entity.
        /// </returns>
        DbEntityEntry Entry(object entity);
    }
}
