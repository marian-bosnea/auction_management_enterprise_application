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
    using log4net;

    /// <summary>
    /// Provides business logic for managing persons, including operations such as starting auctions,
    /// providing feedback, and adjusting scores based on auction activities.
    /// </summary>
    public class PersonService : IPersonService
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The data access object (DAO) responsible for managing person-related data.
        /// </summary>
        private readonly IPersonDAO personDAO;

        /// <summary>
        /// The seriousness threshold used for determining if a person's score meets the criteria for certain operations.
        /// </summary>
        private readonly double seriousnessThreshold;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonService"/> class.
        /// </summary>
        /// <param name="personDAO">The data access object (DAO) responsible for managing person-related data. This DAO provides methods for retrieving and persisting person records.</param>
        public PersonService(IPersonDAO personDAO)
        {
            logger.Info("Initializing PersonService.");

            this.personDAO = personDAO;
            this.seriousnessThreshold = double.Parse(ConfigurationManager.AppSettings["SeriousnessThreshold"] ?? "4.0");

            logger.Info($"PersonService initialized with seriousness threshold: {this.seriousnessThreshold}");
        }

        /// <summary>
        /// Creates and starts a new auction for a person.
        /// </summary>
        /// <param name="person">The person starting the auction.</param>
        public void StartAuction(Person person)
        {
            logger.Info($"Attempting to start auction for person with ID: {person.Id}");

            if (person.Score < this.seriousnessThreshold)
            {
                var errorMessage = $"Cannot start a new auction. Seriousness score is below the required threshold of {this.seriousnessThreshold}.";
                logger.Error(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            logger.Info($"Auction started successfully for person with ID: {person.Id}");
        }

        /// <summary>
        /// Adds a Bid to the auction, provided the person meets the seriousness threshold required for Bidding.
        /// </summary>
        /// <param name="person">The person placing the Bid.</param>
        /// <param name="bid">The Bid to be added to the auction.</param>
        public void AddBid(Person person, Bid bid)
        {
            logger.Info($"Attempting to add bid for person with ID: {person.Id}");

            if (person.Score < this.seriousnessThreshold)
            {
                var errorMessage = $"Cannot place a Bid. Seriousness score is below the required threshold of {this.seriousnessThreshold}.";
                logger.Error(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            bid.Bidder = person;
            logger.Info($"Bid added for person with ID: {person.Id}");
        }

        /// <summary>
        /// Finalizes an auction and adjusts the person's score if applicable.
        /// </summary>
        /// <param name="person">The person who owns the auction.</param>
        /// <param name="auction">The auction to finalize.</param>
        public void FinalizeAuction(Person person, Auction auction)
        {
            logger.Info($"Attempting to finalize auction with ID: {auction.Id} for person with ID: {person.Id}");

            if (auction.Seller != person)
            {
                var errorMessage = "Cannot finalize an auction that is not active or was not initiated by this person.";
                logger.Error(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            if (auction.Bids.Any())
            {
                person.AdjustScore(0.1);
                logger.Info($"Auction with ID: {auction.Id} finalized. Person's score adjusted.");
            }

            this.personDAO.Update(person);
            logger.Info($"Person with ID: {person.Id} updated successfully.");
        }

        /// <summary>
        /// Provides feedback to this person, adjusting their score.
        /// </summary>
        /// <param name="person">The person to receive feedback.</param>
        /// <param name="feedbackScore">The feedback score to adjust, between -0.1 and 0.1.</param>
        public void ProvideFeedback(Person person, double feedbackScore)
        {
            logger.Info($"Providing feedback to person with ID: {person.Id}. Feedback score: {feedbackScore}");

            person.AdjustScore(feedbackScore);
            this.personDAO.Update(person);

            logger.Info($"Feedback provided to person with ID: {person.Id}. Person's score adjusted.");
        }
    }
}