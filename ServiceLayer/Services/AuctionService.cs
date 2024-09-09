// <copyright file="AuctionService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using DataMapper.Interfaces;
    using DomainModel;
    using log4net;
    using ServiceLayer.Interfaces;

    /// <summary>
    /// The AuctionService class provides business logic for managing auctions, including operations
    /// such as starting, finalizing auctions, and providing feedback to users. It handles the validation
    /// and constraints on the number of active auctions a person can have, based on their seriousness score.
    /// </summary>
    public class AuctionService : IAuctionService
    {
        /// <summary>
        /// The logger for logging actions in the class.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AuctionService));

        /// <summary>
        /// The DAO interface for managing auction-related data.
        /// </summary>
        private readonly IAuctionDAO auctionDAO;

        /// <summary>
        /// The DAO interface for managing Bid-related data.
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
        /// Initializes a new instance of the <see cref="AuctionService"/> class.
        /// </summary>
        /// <param name="auctionDAO">The auction DAO.</param>
        /// <param name="bidDAO">The Bid DAO.</param>
        public AuctionService(IAuctionDAO auctionDAO, IBidDAO bidDAO)
        {
            this.auctionDAO = auctionDAO;
            this.bidDAO = bidDAO;

            this.maxActiveAuctions = int.Parse(ConfigurationManager.AppSettings["MaxActiveAuctions"] ?? "5");
            this.maxActiveAuctionsPerCategory = int.Parse(ConfigurationManager.AppSettings["MaxActiveAuctionsPerCategory"] ?? "1");

            Logger.Info($"AuctionService initialized with maxActiveAuctions: {this.maxActiveAuctions} and maxActiveAuctionsPerCategory: {this.maxActiveAuctionsPerCategory}.");
        }

        /// <summary>
        /// Gets the maximum number of active auctions a person can have within a single category.
        /// </summary>
        public int MaxActiveAuctionsPerCategory
        {
            get
            {
                return this.maxActiveAuctionsPerCategory;
            }
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
        public void StartAuction(Person person, Product product, DateTime startDate, DateTime endDate, double startingPrice, string currency)
        {
            Logger.Info($"Attempting to start auction for person {person.Name}, product {product.Name}, from {startDate} to {endDate}.");

            int maxItemsBasedOnScore = this.CalculateMaxItemsBasedOnScore(person.Score);

            if (this.auctionDAO.GetActiveAuctionsForPerson(person).Count >= maxItemsBasedOnScore)
            {
                Logger.Warn($"Cannot start auction for {person.Name}. Maximum of {maxItemsBasedOnScore} active auctions allowed based on seriousness score.");
                throw new InvalidOperationException($"Cannot start a new auction. Maximum of {maxItemsBasedOnScore} active auctions allowed based on seriousness score.");
            }

            int activeAuctions = this.auctionDAO.GetActiveAuctionsForPerson(person).Count;

            if (activeAuctions >= this.maxActiveAuctions)
            {
                Logger.Warn($"Cannot start auction for {person.Name}. Maximum of {this.maxActiveAuctions} active auctions reached.");
                throw new InvalidOperationException($"Cannot start a new auction. Maximum of {this.maxActiveAuctions} active auctions.");
            }

            foreach (var category in product.Categories)
            {
                int activeAuctionsInCategory = this.auctionDAO.GetActiveAuctionsForPersonInCategory(person, category).Count;
                if (activeAuctionsInCategory >= this.maxActiveAuctionsPerCategory)
                {
                    Logger.Warn($"Cannot start auction for {person.Name}. Maximum of {this.maxActiveAuctionsPerCategory} active auctions in category '{category.Name}' reached.");
                    throw new InvalidOperationException($"Cannot start a new auction. Maximum of {this.maxActiveAuctionsPerCategory} active auctions in category '{category.Name}' reached.");
                }
            }

            var auction = new Auction(person, product, startDate, endDate, startingPrice, currency);
            this.auctionDAO.Add(auction);

            Logger.Info($"Auction started successfully for product {product.Name}. Auction ID: {auction.Id}");
        }

        /// <summary>
        /// Finalizes the specified auction by marking it as completed.
        /// </summary>
        /// <param name="person">The person attempting to finalize the auction. This parameter is currently not used in the method but may be used for authorization or logging in the future.</param>
        /// <param name="auction">The auction to be finalized.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the auction has already been completed.
        /// </exception>
        public void FinalizeAuction(Person person, Auction auction)
        {
            Logger.Info($"Attempting to finalize auction ID: {auction.Id}");

            if (auction.IsCompleted)
            {
                Logger.Warn("Cannot finalize auction. The auction has already been completed.");
                throw new InvalidOperationException("The auction has already been completed.");
            }

            auction.IsCompleted = true;
            this.auctionDAO.Update(auction);

            Logger.Info($"Auction ID: {auction.Id} finalized successfully.");
        }

        /// <summary>
        /// Adds a new Bid to an auction after validating the Bid's currency and amount.
        /// </summary>
        /// <param name="auction">The auction to which the Bid is being added.</param>
        /// <param name="bid">The Bid to be added to the auction.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if the Bid currency does not match the auction's currency, or if the Bid amount
        /// is less than the required minimum (10% higher than the previous highest Bid or the starting price).
        /// </exception>
        public void AddBid(Auction auction, Bid bid)
        {
            Logger.Info($"Attempting to add bid to auction ID: {auction.Id}. Bid amount: {bid.Amount}, currency: {bid.Currency}.");

            if (DateTime.Now >= auction.EndDate)
            {
                Logger.Warn("Cannot add bid. The auction has ended.");
                throw new InvalidOperationException("The auction has ended. No more Bids can be placed.");
            }

            if (bid.Currency != auction.Currency)
            {
                Logger.Error("Bid currency does not match auction currency.");
                throw new ArgumentException("Bid currency must match auction currency.");
            }

            double minPrice = auction.Bids.Count == 0
                                ? auction.StartingPrice
                                : auction.Bids[auction.Bids.Count - 1].Amount * 1.1;

            if (bid.Amount < minPrice)
            {
                Logger.Error("Bid amount is less than required minimum.");
                throw new ArgumentException("Bid amount must be at least 10% higher than the previous Bid.");
            }

            auction.AddBid(bid);
            this.bidDAO.Add(bid);
            this.auctionDAO.Update(auction);

            Logger.Info($"Bid added successfully to auction ID: {auction.Id}. New highest bid amount: {bid.Amount}");
        }

        /// <summary>
        /// Adds a new auction to the system.
        /// </summary>
        /// <param name="auction">The auction to add.</param>
        public void AddAuction(Auction auction)
        {
            Logger.Info($"Adding new auction with ID: {auction.Id}.");
            this.auctionDAO.Add(auction);
        }

        /// <summary>
        /// Retrieves an auction by its ID.
        /// </summary>
        /// <param name="id">The ID of the auction to retrieve.</param>
        /// <returns>The auction with the specified ID, or null if not found.</returns>
        public Auction GetAuctionById(int id)
        {
            Logger.Info($"Retrieving auction with ID: {id}.");
            return this.auctionDAO.Get(id);
        }

        /// <summary>
        /// Retrieves all auctions in the system.
        /// </summary>
        /// <returns>A list of all auctions.</returns>
        public List<Auction> GetAllAuctions()
        {
            Logger.Info("Retrieving all auctions.");
            return this.auctionDAO.GetAll();
        }

        /// <summary>
        /// Updates an existing auction in the system.
        /// </summary>
        /// <param name="auction">The auction to update.</param>
        public void UpdateAuction(Auction auction)
        {
            if (auction == null)
            {
                Logger.Error("Auction update failed. Auction cannot be null.");
                throw new ArgumentNullException("Auction must not be null");
            }

            Logger.Info($"Updating auction ID: {auction.Id}.");
            this.auctionDAO.Update(auction);
        }

        /// <summary>
        /// Deletes an auction from the system.
        /// </summary>
        /// <param name="id">The ID of the auction to delete.</param>
        public void DeleteAuction(int id)
        {
            Logger.Info($"Attempting to delete auction with ID: {id}.");
            var auction = this.auctionDAO.Get(id);
            if (auction != null)
            {
                this.auctionDAO.Delete(id);
                Logger.Info($"Auction ID: {id} deleted successfully.");
            }
            else
            {
                Logger.Warn($"Auction ID: {id} not found for deletion.");
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
        private int CalculateMaxItemsBasedOnScore(double score)
        {
            int maxItems = (int)Math.Max(1, 10 - ((10 - score) * 0.5));
            Logger.Debug($"Calculated max items based on score {score}: {maxItems}");
            return maxItems;
        }
    }
}