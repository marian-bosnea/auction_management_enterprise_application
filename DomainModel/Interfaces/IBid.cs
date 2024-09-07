// <copyright file="IBid.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>
namespace DomainModel
{
    using System;

    /// <summary>
    /// Defines the contract for a bid made in an auction.
    /// </summary>
    public interface IBid
    {
        /// <summary>
        /// Gets or sets the unique identifier for the bid.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the person who placed the bid.
        /// </summary>
        IPerson Bidder { get; set; }

        /// <summary>
        /// Gets the amount of the bid.
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Gets the currency in which the bid is made.
        /// </summary>
        string Currency { get; }

        /// <summary>
        /// Gets the time when the bid was made.
        /// </summary>
        DateTime BidTime { get; }

        /// <summary>
        /// Returns a string representation of the bid.
        /// </summary>
        /// <returns>A string that represents the current bid.</returns>
        string ToString();
    }
}
