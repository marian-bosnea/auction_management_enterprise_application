// <copyright file="IBidDAO.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper.Interfaces
{
    using System.Collections.Generic;
    using DomainModel;

    /// <summary>
    /// Defines the data access operations for Bids.
    /// </summary>
    public interface IBidDAO
    {
        /// <summary>
        /// Adds a new Bid to the data store.
        /// </summary>
        /// <param name="bid">The Bid to add.</param>
        void Add(Bid bid);

        /// <summary>
        /// Retrieves all Bids from the data store.
        /// </summary>
        /// <returns>A list of all Bids.</returns>
        List<Bid> GetAll();

        /// <summary>
        /// Retrieves a specific Bid by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the Bid to retrieve.</param>
        /// <returns>The Bid with the specified identifier, or null if not found.</returns>
        Bid Get(int id);

        /// <summary>
        /// Updates an existing Bid in the data store.
        /// </summary>
        /// <param name="bid">The Bid to update.</param>
        void Update(Bid bid);

        /// <summary>
        /// Deletes a Bid from the data store by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the Bid to delete.</param>
        void Delete(int id);
    }
}