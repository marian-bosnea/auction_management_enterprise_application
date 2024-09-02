// <copyright file="IAuctionRepository.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a repository for managing auction-related data.
    /// </summary>
    public interface IAuctionRepository
    {
        /// <summary>
        /// Saves the specified auction to the repository.
        /// </summary>
        /// <param name="auction">The auction to be saved.</param>
        void Save(IAuction auction);

        /// <summary>
        /// Retrieves a list of active auctions initiated by a specific person in a specific category.
        /// </summary>
        /// <param name="person">The person who initiated the auctions.</param>
        /// <param name="category">The category in which the auctions belong.</param>
        /// <returns>
        /// A list of <see cref="Auction"/> objects representing the active auctions
        /// that the specified person has in the specified category.
        /// </returns>
        List<Auction> GetActiveAuctionsForPersonInCategory(IPerson person, ICategory category);
    }
}
