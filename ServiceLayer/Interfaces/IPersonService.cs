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
        /// Creates and starts a new auction for a person.
        /// </summary>
        /// <param name="person">The person starting the auction.</param>
        void StartAuction(Person person);

        /// <summary>
        /// Adds a Bid to the auction, provided the person meets the seriousness threshold required for Bidding.
        /// </summary>
        /// <param name="person">The person placing the Bid.</param>
        /// <param name="Bid">The Bid to be added to the auction.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the person's seriousness score is below the required threshold, preventing them from placing a Bid.
        /// </exception>
        /// <remarks>
        /// This method checks the seriousness score of the person attempting to place a Bid. If the person's score is below
        /// the predefined threshold (`seriousnessThreshold`), an exception is thrown, indicating that the Bid cannot be placed.
        /// This ensures that only individuals with a seriousness score meeting or exceeding the threshold are allowed to place Bids.
        /// </remarks>
        void AddBid(Person person, Bid Bid);

        /// <summary>
        /// Finalizes an auction and adjusts the person's score if applicable.
        /// </summary>
        /// <param name="person">The person who owns the auction. The auction must be one of the person's active auctions to be finalized.</param>
        /// <param name="auction">The auction to be finalized. The auction must be active and associated with the specified person.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the auction is not active or was not initiated by the specified person.
        /// </exception>
        /// <remarks>
        /// The method removes the auction from the person's list of active auctions. If the auction received any Bids, the person's score is adjusted positively.
        /// The person's score adjustment reflects their activity and performance in the auction process.
        /// </remarks>
        void FinalizeAuction(Person person, Auction auction);

        /// <summary>
        /// Provides feedback to a person by adjusting their score based on the feedback provided.
        /// </summary>
        /// <param name="person">The person receiving the feedback. The person's score will be adjusted according to the feedback score.</param>
        /// <param name="feedbackScore">The feedback score to be applied. This value should be between -0.1 and 0.1 to reflect positive or negative feedback appropriately.</param>
        /// <remarks>
        /// The method adjusts the person's score based on the feedback score provided. Positive feedback increases the score, while negative feedback decreases it.
        /// The feedback score should be within the range of -0.1 to 0.1 to ensure balanced adjustments.
        /// </remarks>
        void ProvideFeedback(Person person, decimal feedbackScore);
    }
}
