// <copyright file="AuctionService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace Services
{
    using System;
    using System.Configuration;
    using System.Linq;
    using DomainModel;
    using ServiceLayer;

    /// <summary>
    /// The AuctionService class provides business logic for managing auctions, including operations
    /// such as starting, finalizing auctions, and providing feedback to users. It handles the validation
    /// and constraints on the number of active auctions a person can have, based on their seriousness score.
    /// </summary>
    public class AuctionService
    {
        /// <summary>
        /// The repository interface for managing auction-related data.
        /// </summary>
        private readonly IAuctionRepository auctionRepository;

        /// <summary>
        /// The repository interface for managing person-related data.
        /// </summary>
        private readonly IPersonRepository personRepository;

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
        /// <param name="auctionRepo">The auction repository.</param>
        /// <param name="personRepo">The person repository.</param>
        public AuctionService(IAuctionRepository auctionRepo, IPersonRepository personRepo)
        {
            this.auctionRepository = auctionRepo;
            this.personRepository = personRepo;

            // Read configuration values or set default values if not configured.
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
        public void StartAuction(Person person, Product product, DateTime startDate, DateTime endDate, decimal startingPrice, string currency)
        {
            if (person.Score < this.seriousnessThreshold)
            {
                throw new InvalidOperationException($"Cannot start a new auction. Seriousness score is below the required threshold of {this.seriousnessThreshold}.");
            }

            int maxItemsBasedOnScore = this.CalculateMaxItemsBasedOnScore(person.Score);

            if (person.ActiveAuctions.Count >= maxItemsBasedOnScore)
            {
                throw new InvalidOperationException($"Cannot start a new auction. Maximum of {maxItemsBasedOnScore} active auctions allowed based on seriousness score.");
            }

            foreach (var category in product.Categories)
            {
                int activeAuctionsInCategory = this.auctionRepository.GetActiveAuctionsForPersonInCategory(person, category).Count;
                if (activeAuctionsInCategory >= this.maxActiveAuctionsPerCategory)
                {
                    throw new InvalidOperationException($"Cannot start a new auction. Maximum of {this.maxActiveAuctionsPerCategory} active auctions in category '{category.Name}' reached.");
                }
            }

            var auction = new Auction(product, startDate, endDate, startingPrice, currency);
            person.ActiveAuctions.Add(auction);
            this.auctionRepository.Save(auction);
            this.personRepository.Update(person);
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

            decimal minPrice = auction.Bids.Count == 0 ? auction.StartingPrice : auction.Bids[auction.Bids.Count - 1].Amount * 1.1m;
            if (bid.Amount < minPrice)
            {
                throw new ArgumentException("Bid amount must be at least 10% higher than the previous bid.");
            }

            auction.AddBid(bid);
        }

        /// <summary>
        /// Finalizes an auction and adjusts the person's score if applicable.
        /// </summary>
        /// <param name="person">The person who owns the auction.</param>
        /// <param name="auction">The auction to finalize.</param>
        public void FinalizeAuction(Person person, Auction auction)
        {
            if (!person.ActiveAuctions.Contains(auction))
            {
                throw new InvalidOperationException("Cannot finalize an auction that is not active or was not initiated by this person.");
            }

            person.ActiveAuctions.Remove(auction);

            if (auction.Bids.Any())
            {
                person.AdjustScore(0.1m);
            }

            this.personRepository.Update(person);
        }

        /// <summary>
        /// Provides feedback to this person, adjusting their score.
        /// </summary>
        /// <param name="person">The person to receive feedback.</param>
        /// <param name="feedbackScore">The feedback score to adjust, between -0.1 and 0.1.</param>
        public void ProvideFeedback(Person person, decimal feedbackScore)
        {
            person.AdjustScore(feedbackScore);
            this.personRepository.Update(person);
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
