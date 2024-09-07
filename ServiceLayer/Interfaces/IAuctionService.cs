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
        /// Ends the specified auction by setting its status to completed if the person trying to end it is the owner.
        /// </summary>
        /// <param name="auction">The auction to be ended.</param>
        /// <param name="person">The person attempting to end the auction.</param>
        /// <exception cref="UnauthorizedAccessException">
        /// Thrown when the person attempting to end the auction is not the owner of the auction.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the auction is already completed.
        /// </exception>
        /// <remarks>
        /// This method sets the <c>IsCompleted</c> field of the auction to <c>true</c> if the person
        /// trying to end the auction is the owner. If the auction is already completed or if the person
        /// is not the owner, appropriate exceptions are thrown. The method also updates the auction status
        /// in the data store through the <c>auctionDAO</c> object.
        /// </remarks>
        void EndAuction(IAuction auction, IPerson person);

        /// <summary>
        /// Adds a new bid to an auction after validating the bid's currency and amount.
        /// </summary>
        /// <param name="auction">The auction to which the bid is being added.</param>
        /// <param name="bid">The bid to be added to the auction.</param>
        void AddBid(IAuction auction, IBid bid);

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
