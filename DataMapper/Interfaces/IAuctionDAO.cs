// <copyright file="IAuctionDAO.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper.Interfaces
{
    using System.Collections.Generic;
    using DomainModel;

    /// <summary>
    /// Defines the data access operations for auctions.
    /// </summary>
    public interface IAuctionDAO
    {
        /// <summary>
        /// Adds a new auction to the data store.
        /// </summary>
        /// <param name="auction">The auction to add.</param>
        void Add(IAuction auction);

        /// <summary>
        /// Retrieves all auctions from the data store.
        /// </summary>
        /// <returns>A list of all auctions.</returns>
        List<IAuction> GetAll();

        /// <summary>
        /// Retrieves a specific auction by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the auction to retrieve.</param>
        /// <returns>The auction with the specified identifier, or null if not found.</returns>
        Auction Get(int id);

        /// <summary>
        /// Updates an existing auction in the data store.
        /// </summary>
        /// <param name="auction">The auction to update.</param>
        void Update(IAuction auction);

        /// <summary>
        /// Deletes an auction from the data store by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the auction to delete.</param>
        void Delete(int id);

        /// <summary>
        /// Retrieves active auctions for a specific person in a particular category.
        /// </summary>
        /// <param name="person">The person whose active auctions are to be retrieved.</param>
        /// <param name="category">The category in which to look for active auctions.</param>
        /// <returns>A list of active auctions for the specified person and category.</returns>
        List<Auction> GetActiveAuctionsForPersonInCategory(IPerson person, ICategory category);
    }
}