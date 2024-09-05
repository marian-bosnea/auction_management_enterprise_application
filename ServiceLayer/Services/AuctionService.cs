// <copyright file="AuctionService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using DataMapper.Interfaces;
    using DomainModel;
    using ServiceLayer.Interfaces;

    /// <summary>
    /// The AuctionService class provides business logic for managing auctions, including operations
    /// such as starting, finalizing auctions, and providing feedback to users. It handles the validation
    /// and constraints on the number of active auctions a person can have, based on their seriousness score.
    /// </summary>
    public class AuctionService : IAuctionService
    {
        /// <summary>
        /// The DAO interface for managing auction-related data.
        /// </summary>
        private readonly IAuctionDAO auctionDAO;

        /// <summary>
        /// The DAO interface for managing bid-related data.
        /// </summary>
        private readonly IBidDAO bidDAO;

        /// <summary>
        /// The maximum number of active auctions a person can have at any given time.
        /// </summary>
        private readonly int maxActiveAuctions;

        /// <summary>
        /// The maximum number of active auctions a person can have within a single category.
        /// </summary>
        private readonly int maxActiveAuctionsPerCategory;

        /// <summary>
        /// The threshold score that determines the maximum number of items a person can list for auction based on their seriousness score.
        /// </summary>
        private readonly decimal seriousnessThreshold;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionService"/> class.
        /// </summary>
        /// <param name="auctionDAO">The auction DAO.</param>
        /// <param name="bidDAO">The bid DAO.</param>
        public AuctionService(IAuctionDAO auctionDAO, IBidDAO bidDAO)
        {
            this.auctionDAO = auctionDAO;
            this.bidDAO = bidDAO;

            this.maxActiveAuctions = int.Parse(ConfigurationManager.AppSettings["MaxActiveAuctions"] ?? "5");
            this.maxActiveAuctionsPerCategory = int.Parse(ConfigurationManager.AppSettings["MaxActiveAuctionsPerCategory"] ?? "3");
            this.seriousnessThreshold = decimal.Parse(ConfigurationManager.AppSettings["SeriousnessThreshold"] ?? "4.0");
        }

        /// <summary>
        /// Creates and starts a new auction for a person.
        /// </summary>
        /// <param name="person">The person starting the auction.</param>
        /// <param name="product">The product to be auctioned.</param>
        /// <param name="startDate">The start date of the auction.</param>
        /// <param name="endDate">The end date of the auction.</param>
        /// <param name="startingPrice">The starting price of the auction.</param>
        /// <param name="currency">The currency for the auction.</param>
        public void StartAuction(IPerson person, IProduct product, DateTime startDate, DateTime endDate, decimal startingPrice, string currency)
        {
            foreach (var category in product.Categories)
            {
                int activeAuctionsInCategory = this.auctionDAO.GetActiveAuctionsForPersonInCategory(person, category).Count;
                if (activeAuctionsInCategory >= this.maxActiveAuctionsPerCategory)
                {
                    throw new InvalidOperationException($"Cannot start a new auction. Maximum of {this.maxActiveAuctionsPerCategory} active auctions in category '{category.Name}' reached.");
                }
            }

            var auction = new Auction(product, startDate, endDate, startingPrice, currency);
            person.ActiveAuctions.Add(auction);
            this.auctionDAO.Add(auction);
        }

        /// <summary>
        /// Adds a new bid to an auction after validating the bid's currency and amount.
        /// The bid must match the auction's currency, and the bid amount must be at least 10% higher
        /// than the previous highest bid or the starting price if no bids exist.
        /// </summary>
        /// <param name="auction">The auction to which the bid is being added.</param>
        /// <param name="bid">The bid to be added to the auction.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if the bid currency does not match the auction's currency, or if the bid amount
        /// is less than the required minimum (10% higher than the previous highest bid or the starting price).
        /// </exception>
        public void AddBid(IAuction auction, IBid bid)
        {
            if (bid.Currency != auction.Currency)
            {
                throw new ArgumentException("Bid currency must match auction currency.");
            }

            decimal minPrice = auction.Bids.Count == 0
                                ? auction.StartingPrice
                                : auction.Bids[auction.Bids.Count - 1].Amount * 1.1m;

            if (bid.Amount < minPrice)
            {
                throw new ArgumentException("Bid amount must be at least 10% higher than the previous bid.");
            }

            auction.AddBid(bid);

            this.bidDAO.Add(bid);

            this.auctionDAO.Update(auction);
        }

        /// <summary>
        /// Adds a new auction to the system.
        /// </summary>
        /// <param name="auction">The auction to add.</param>
        public void AddAuction(IAuction auction)
        {
            this.auctionDAO.Add(auction);
        }

        /// <summary>
        /// Retrieves an auction by its ID.
        /// </summary>
        /// <param name="id">The ID of the auction to retrieve.</param>
        /// <returns>The auction with the specified ID, or null if not found.</returns>
        public IAuction GetAuctionById(int id)
        {
            return this.auctionDAO.Get(id);
        }

        /// <summary>
        /// Retrieves all auctions in the system.
        /// </summary>
        /// <returns>A list of all auctions.</returns>
        public List<IAuction> GetAllAuctions()
        {
            return this.auctionDAO.GetAll();
        }

        /// <summary>
        /// Updates an existing auction in the system.
        /// </summary>
        /// <param name="auction">The auction to update.</param>
        public void UpdateAuction(IAuction auction)
        {
            this.auctionDAO.Update(auction);
        }

        /// <summary>
        /// Deletes an auction from the system.
        /// </summary>
        /// <param name="id">The ID of the auction to delete.</param>
        public void DeleteAuction(int id)
        {
            var auction = this.auctionDAO.Get(id);
            if (auction != null)
            {
                this.auctionDAO.Delete(auction.Id);
            }
        }

        /// <summary>
        /// Calculates the maximum number of items that a person can list for auction based on their seriousness score.
        /// </summary>
        /// <param name="score">The seriousness score of the person, ranging from 0 to 10.</param>
        /// <returns>
        /// An integer representing the maximum number of items that can be listed for auction.
        /// The value is calculated such that a higher score allows more items to be listed, with a minimum of 1 item.
        /// </returns>
        /// <remarks>
        /// The formula used for the calculation is:
        /// <c>Max(1, 10 - ((10 - score) * 0.5m))</c>
        /// This ensures that the number of items decreases as the score decreases, with a minimum of 1 item.
        /// </remarks>
        private int CalculateMaxItemsBasedOnScore(decimal score)
        {
            return (int)Math.Max(1, 10 - ((10 - score) * 0.5m));
        }
    }
}
