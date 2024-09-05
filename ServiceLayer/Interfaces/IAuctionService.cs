// <copyright file="IAuctionService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace ServiceLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using DomainModel;

    /// <summary>
    /// Defines the interface for the AuctionService which provides business logic for managing auctions.
    /// </summary>
    public interface IAuctionService
    {
        /// <summary>
        /// Creates and starts a new auction for a person.
        /// </summary>
        /// <param name="person">The person starting the auction.</param>
        /// <param name="product">The product to be auctioned.</param>
        /// <param name="startDate">The start date of the auction.</param>
        /// <param name="endDate">The end date of the auction.</param>
        /// <param name="startingPrice">The starting price of the auction.</param>
        /// <param name="currency">The currency for the auction.</param>
        void StartAuction(IPerson person, IProduct product, DateTime startDate, DateTime endDate, decimal startingPrice, string currency);

        /// <summary>
        /// Adds a new bid to an auction after validating the bid's currency and amount.
        /// </summary>
        /// <param name="auction">The auction to which the bid is being added.</param>
        /// <param name="bid">The bid to be added to the auction.</param>
        void AddBid(IAuction auction, IBid bid);

        /// <summary>
        /// Finalizes an auction and adjusts the person's score if applicable.
        /// </summary>
        /// <param name="person">The person who owns the auction.</param>
        /// <param name="auction">The auction to finalize.</param>
        void FinalizeAuction(IPerson person, IAuction auction);

        /// <summary>
        /// Provides feedback to this person, adjusting their score.
        /// </summary>
        /// <param name="person">The person to receive feedback.</param>
        /// <param name="feedbackScore">The feedback score to adjust, between -0.1 and 0.1.</param>
        void ProvideFeedback(IPerson person, decimal feedbackScore);

        /// <summary>
        /// Adds a new auction to the system.
        /// </summary>
        /// <param name="auction">The auction to add.</param>
        void AddAuction(IAuction auction);

        /// <summary>
        /// Retrieves an auction by its ID.
        /// </summary>
        /// <param name="id">The ID of the auction to retrieve.</param>
        /// <returns>The auction with the specified ID, or null if not found.</returns>
        IAuction GetAuctionById(int id);

        /// <summary>
        /// Retrieves all auctions in the system.
        /// </summary>
        /// <returns>A list of all auctions.</returns>
        List<IAuction> GetAllAuctions();

        /// <summary>
        /// Updates an existing auction in the system.
        /// </summary>
        /// <param name="auction">The auction to update.</param>
        void UpdateAuction(IAuction auction);

        /// <summary>
        /// Deletes an auction from the system.
        /// </summary>
        /// <param name="id">The ID of the auction to delete.</param>
        void DeleteAuction(int id);
    }
}
