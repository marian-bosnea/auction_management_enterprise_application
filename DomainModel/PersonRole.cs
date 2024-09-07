// <copyright file="PersonRole.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    /// <summary>
    /// Represents the role of a person in the auction process.
    /// </summary>
    public enum PersonRole
    {
        /// <summary>
        /// The person who offers an object for auction.
        /// </summary>
        Seller = 0,

        /// <summary>
        /// The person who places Bids in an auction.
        /// </summary>
        Bidder = 1,

        /// <summary>
        /// The person who has both Seller and Bidder roles.
        /// </summary>
        Both = Seller | Bidder,
    }
}
