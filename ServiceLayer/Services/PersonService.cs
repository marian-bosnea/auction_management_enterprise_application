// <copyright file="PersonService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace Services
{
    using System;
    using System.Configuration;
    using System.Linq;
    using DataMapper.Interfaces;
    using DomainModel;
    using ServiceLayer.Interfaces;

    /// <summary>
    /// Provides business logic for managing persons, including operations such as starting auctions,
    /// providing feedback, and adjusting scores based on auction activities.
    /// </summary>
    public class PersonService : IPersonService
    {
        /// <summary>
        /// The data access object (DAO) responsible for managing person-related data.
        /// </summary>
        /// <remarks>
        /// This DAO provides methods for retrieving and persisting person records, and is used throughout the service to perform operations related to persons.
        /// </remarks>
        private readonly IPersonDAO personDAO;

        /// <summary>
        /// The data access object (DAO) responsible for managing auction-related data.
        /// </summary>
        /// <remarks>
        /// This DAO provides methods for interacting with auction records, allowing the service to perform operations related to auctions, such as starting and finalizing auctions.
        /// </remarks>
        private readonly IAuctionDAO auctionDAO;

        /// <summary>
        /// The data access object (DAO) responsible for managing bid-related data.
        /// </summary>
        /// <remarks>
        /// This DAO provides methods for handling bid records, including adding and retrieving bids. It is used by the service to manage bids associated with auctions.
        /// </remarks>
        private readonly IBidDAO bidDAO;

        /// <summary>
        /// The seriousness threshold used for determining if a person's score meets the criteria for certain operations.
        /// </summary>
        /// <remarks>
        /// This threshold is a decimal value that is used to enforce criteria related to the seriousness of a person when performing actions such as starting auctions.
        /// The value is configurable through the application settings, with a default of 4.0 if not specified.
        /// </remarks>
        private readonly decimal seriousnessThreshold;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonService"/> class.
        /// </summary>
        /// <param name="personDAO">The data access object (DAO) responsible for managing person-related data. This DAO provides methods for retrieving and persisting person records.</param>
        /// <param name="auctionDAO">The data access object (DAO) responsible for managing auction-related data. This DAO provides methods for interacting with auction records.</param>
        /// <param name="bidDAO">The data access object (DAO) responsible for managing bid-related data. This DAO provides methods for handling bid records.</param>
        /// <remarks>
        /// The constructor initializes the <see cref="PersonService"/> with the specified DAOs, which are used to perform data operations related to persons, auctions, and bids.
        /// It also reads the seriousness threshold from the application configuration. This threshold is used to determine the criteria for certain operations, such as starting auctions.
        /// If the configuration value is not available, a default value of 4.0 is used.
        /// </remarks>
        public PersonService(IPersonDAO personDAO, IAuctionDAO auctionDAO, IBidDAO bidDAO)
        {
            this.personDAO = personDAO;
            this.auctionDAO = auctionDAO;
            this.bidDAO = bidDAO;
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
            if (person.Score < this.seriousnessThreshold)
            {
                throw new InvalidOperationException($"Cannot start a new auction. Seriousness score is below the required threshold of {this.seriousnessThreshold}.");
            }

            int maxItemsBasedOnScore = this.CalculateMaxItemsBasedOnScore(person.Score);

            if (person.ActiveAuctions.Count >= maxItemsBasedOnScore)
            {
                throw new InvalidOperationException($"Cannot start a new auction. Maximum of {maxItemsBasedOnScore} active auctions allowed based on seriousness score.");
            }

            var auctionService = new AuctionService(this.auctionDAO, this.bidDAO);
            auctionService.StartAuction(person, product, startDate, endDate, startingPrice, currency);
        }

        /// <summary>
        /// Finalizes an auction and adjusts the person's score if applicable.
        /// </summary>
        /// <param name="person">The person who owns the auction.</param>
        /// <param name="auction">The auction to finalize.</param>
        public void FinalizeAuction(IPerson person, IAuction auction)
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

            this.personDAO.Update(person);
        }

        /// <summary>
        /// Provides feedback to this person, adjusting their score.
        /// </summary>
        /// <param name="person">The person to receive feedback.</param>
        /// <param name="feedbackScore">The feedback score to adjust, between -0.1 and 0.1.</param>
        public void ProvideFeedback(IPerson person, decimal feedbackScore)
        {
            person.AdjustScore(feedbackScore);
            this.personDAO.Update(person);
        }

        /// <summary>
        /// Calculates the maximum number of items that a person can list for auction based on their seriousness score.
        /// </summary>
        /// <param name="score">The seriousness score of the person, ranging from 0 to 10.</param>
        /// <returns>
        /// An integer representing the maximum number of items that can be listed for auction.
        /// The value is calculated such that a higher seriousness score allows listing more items, with a minimum of 1 item.
        /// </returns>
        /// <remarks>
        /// The calculation is based on the following formula:
        /// <c>Max(1, 10 - ((10 - score) * 0.5m))</c>
        /// This formula ensures that as the seriousness score increases, the maximum number of items that can be listed increases linearly.
        /// The minimum number of items that can be listed is capped at 1, regardless of the score.
        /// </remarks>
        private int CalculateMaxItemsBasedOnScore(decimal score)
        {
            return (int)Math.Max(1, 10 - ((10 - score) * 0.5m));
        }
    }
}
