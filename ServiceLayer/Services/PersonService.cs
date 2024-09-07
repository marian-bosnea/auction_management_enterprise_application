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
        /// <remarks>
        /// The constructor initializes the <see cref="PersonService"/> with the specified DAOs, which are used to perform data operations related to persons, auctions, and bids.
        /// It also reads the seriousness threshold from the application configuration. This threshold is used to determine the criteria for certain operations, such as starting auctions.
        /// If the configuration value is not available, a default value of 4.0 is used.
        /// </remarks>
        public PersonService(IPersonDAO personDAO)
        {
            this.personDAO = personDAO;
            this.seriousnessThreshold = decimal.Parse(ConfigurationManager.AppSettings["SeriousnessThreshold"] ?? "4.0");
        }

        /// <summary>
        /// Creates and starts a new auction for a person.
        /// </summary>
        /// <param name="person">The person starting the auction.</param>
        public void StartAuction(IPerson person)
        {
            if (person.Score < this.seriousnessThreshold)
            {
                throw new InvalidOperationException($"Cannot start a new auction. Seriousness score is below the required threshold of {this.seriousnessThreshold}.");
            }
        }

        /// <summary>
        /// Adds a bid to the auction, provided the person meets the seriousness threshold required for bidding.
        /// </summary>
        /// <param name="person">The person placing the bid.</param>
        /// <param name="bid">The bid to be added to the auction.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the person's seriousness score is below the required threshold, preventing them from placing a bid.
        /// </exception>
        /// <remarks>
        /// This method checks the seriousness score of the person attempting to place a bid. If the person's score is below
        /// the predefined threshold (`seriousnessThreshold`), an exception is thrown, indicating that the bid cannot be placed.
        /// This ensures that only individuals with a seriousness score meeting or exceeding the threshold are allowed to place bids.
        /// </remarks>
        public void AddBid(IPerson person, IBid bid)
        {
            if (person.Score < this.seriousnessThreshold)
            {
                throw new InvalidOperationException($"Cannot place a bid. Seriousness score is below the required threshold of {this.seriousnessThreshold}.");
            }

            bid.Bidder = person;
        }

        /// <summary>
        /// Finalizes an auction and adjusts the person's score if applicable.
        /// </summary>
        /// <param name="person">The person who owns the auction.</param>
        /// <param name="auction">The auction to finalize.</param>
        public void FinalizeAuction(IPerson person, IAuction auction)
        {
            if (auction.Seller != person)
            {
                throw new InvalidOperationException("Cannot finalize an auction that is not active or was not initiated by this person.");
            }

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
    }
}
