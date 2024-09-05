// <copyright file="IBidDAO.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper.Interfaces
{
    using System.Collections.Generic;
    using DomainModel;

    /// <summary>
    /// Defines the data access operations for bids.
    /// </summary>
    public interface IBidDAO
    {
        /// <summary>
        /// Adds a new bid to the data store.
        /// </summary>
        /// <param name="bid">The bid to add.</param>
        void Add(IBid bid);

        /// <summary>
        /// Retrieves all bids from the data store.
        /// </summary>
        /// <returns>A list of all bids.</returns>
        List<IBid> GetAll();

        /// <summary>
        /// Retrieves a specific bid by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the bid to retrieve.</param>
        /// <returns>The bid with the specified identifier, or null if not found.</returns>
        Bid Get(int id);

        /// <summary>
        /// Updates an existing bid in the data store.
        /// </summary>
        /// <param name="bid">The bid to update.</param>
        void Update(IBid bid);

        /// <summary>
        /// Deletes a bid from the data store by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the bid to delete.</param>
        void Delete(int id);
    }
}