// <copyright file="IAuction.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the contract for an auction associated with a specific product.
    /// </summary>
    public interface IAuction
    {
        /// <summary>
        /// Gets or sets the unique identifier for the auction.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets the seller who initiated the auction.
        /// </summary>
        IPerson Seller { get; }

        /// <summary>
        /// Gets the product associated with this auction.
        /// </summary>
        IProduct Product { get; }

        /// <summary>
        /// Gets the start date of the auction.
        /// </summary>
        DateTime StartDate { get; }

        /// <summary>
        /// Gets the end date of the auction.
        /// </summary>
        DateTime EndDate { get; }

        /// <summary>
        /// Gets the starting price of the auction.
        /// </summary>
        decimal StartingPrice { get; }

        /// <summary>
        /// Gets the currency in which the auction is conducted.
        /// </summary>
        string Currency { get; }

        /// <summary>
        /// Gets the list of bids made in this auction.
        /// </summary>
        List<IBid> Bids { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the auction is completed.
        /// </summary>
        bool IsCompleted { get; set; }

        /// <summary>
        /// Adds a new bid to the auction.
        /// </summary>
        /// <param name="bid">The bid to add.</param>
        void AddBid(IBid bid);

        /// <summary>
        /// Returns a string representation of the auction.
        /// </summary>
        /// <returns>A string that represents the current auction.</returns>
        string ToString();
    }
}
