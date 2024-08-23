// <copyright file="IPerson.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the contract for a person who can initiate and manage auctions.
    /// </summary>
    public interface IPerson
    {
        /// <summary>
        /// Gets the name of the person.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the score of the person, representing their reliability.
        /// </summary>
        decimal Score { get; }

        /// <summary>
        /// Gets the list of active auctions initiated by this person.
        /// </summary>
        IReadOnlyList<Auction> ActiveAuctions { get; }

        /// <summary>
        /// Starts a new auction for the specified product.
        /// </summary>
        /// <param name="product">The product to be auctioned.</param>
        /// <param name="startDate">The start date of the auction.</param>
        /// <param name="endDate">The end date of the auction.</param>
        /// <param name="startingPrice">The starting price of the auction.</param>
        /// <param name="currency">The currency in which the auction is conducted.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the person has reached the maximum number of active auctions or the maximum number of active auctions in the product's categories.
        /// </exception>
        void StartAuction(Product product, DateTime startDate, DateTime endDate, decimal startingPrice, string currency);

        /// <summary>
        /// Finalizes the auction by marking it as closed and removing it from the active auctions list.
        /// </summary>
        /// <param name="auction">The auction to finalize.</param>
        /// <exception cref="InvalidOperationException">Thrown if the auction is not found in the active auctions list or if the person is not the initiator.</exception>
        void FinalizeAuction(Auction auction);

        /// <summary>
        /// Adjusts the person's score based on feedback or auction completion.
        /// </summary>
        /// <param name="amount">The amount to adjust the score by, between -0.1 and 0.1.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the amount is not within the valid range [-0.1, 0.1].</exception>
        void AdjustScore(decimal amount);

        /// <summary>
        /// Provides feedback to this person, adjusting their score.
        /// </summary>
        /// <param name="feedbackScore">The feedback score to add, between -0.1 and 0.1.</param>
        void ReceiveFeedback(decimal feedbackScore);
    }
}
