// <copyright file="IPersonService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace ServiceLayer.Interfaces
{
    using System;
    using DomainModel;

    /// <summary>
    /// Defines the contract for managing persons, including operations such as starting auctions, providing feedback, and adjusting scores.
    /// </summary>
    public interface IPersonService
    {
        /// <summary>
        /// Starts a new auction for a person with the specified product, start and end dates, starting price, and currency.
        /// </summary>
        /// <param name="person">The person initiating the auction. The person must meet certain criteria, such as having a seriousness score above a threshold, to start an auction.</param>
        /// <param name="product">The product to be auctioned. This should be an instance of the <see cref="IProduct"/> interface representing the item that is being listed for auction.</param>
        /// <param name="startDate">The start date and time of the auction. The auction will not begin before this date and time.</param>
        /// <param name="endDate">The end date and time of the auction. The auction will end after this date and time.</param>
        /// <param name="startingPrice">The initial price at which the auction starts. This is the minimum amount that can be bid at the start of the auction.</param>
        /// <param name="currency">The currency in which the auction is conducted. This should match the currency of the bids placed in the auction.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the person does not meet the criteria to start an auction, such as having a seriousness score below the required threshold,
        /// or if the person has reached the maximum number of active auctions allowed based on their score.
        /// </exception>
        /// <remarks>
        /// The method validates that the person has a seriousness score above a predefined threshold and that they do not exceed the maximum number of active auctions.
        /// It also ensures that the auction does not exceed the allowed number of active auctions per category.
        /// </remarks>
        void StartAuction(IPerson person, IProduct product, DateTime startDate, DateTime endDate, decimal startingPrice, string currency);

        /// <summary>
        /// Finalizes an auction and adjusts the person's score if applicable.
        /// </summary>
        /// <param name="person">The person who owns the auction. The auction must be one of the person's active auctions to be finalized.</param>
        /// <param name="auction">The auction to be finalized. The auction must be active and associated with the specified person.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the auction is not active or was not initiated by the specified person.
        /// </exception>
        /// <remarks>
        /// The method removes the auction from the person's list of active auctions. If the auction received any bids, the person's score is adjusted positively.
        /// The person's score adjustment reflects their activity and performance in the auction process.
        /// </remarks>
        void FinalizeAuction(IPerson person, IAuction auction);

        /// <summary>
        /// Provides feedback to a person by adjusting their score based on the feedback provided.
        /// </summary>
        /// <param name="person">The person receiving the feedback. The person's score will be adjusted according to the feedback score.</param>
        /// <param name="feedbackScore">The feedback score to be applied. This value should be between -0.1 and 0.1 to reflect positive or negative feedback appropriately.</param>
        /// <remarks>
        /// The method adjusts the person's score based on the feedback score provided. Positive feedback increases the score, while negative feedback decreases it.
        /// The feedback score should be within the range of -0.1 to 0.1 to ensure balanced adjustments.
        /// </remarks>
        void ProvideFeedback(IPerson person, decimal feedbackScore);
    }
}
