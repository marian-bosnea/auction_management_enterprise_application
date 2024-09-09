// <copyright file="AuctionMediator.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace ServiceLayer
{
    using System;
    using DomainModel;
    using log4net;
    using ServiceLayer.Interfaces;

    /// <summary>
    /// Mediates interactions between auction and person services, providing a unified interface for managing auctions and Bids.
    /// </summary>
    public class AuctionMediator
    {
        /// <summary>
        /// Logger instance for logging operations within the <see cref="StringUtils"/> class.
        /// </summary>
        /// <remarks>
        /// This static readonly field is used to log information, warnings, errors, and other messages related to string operations.
        /// It utilizes the log4net library for logging, and the logger is configured to log messages based on the class's namespace and type.
        /// </remarks>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The service that manages auction business logic.
        /// </summary>
        private readonly IAuctionService auctionService;

        /// <summary>
        /// The service that manages person business logic.
        /// </summary>
        private readonly IPersonService personService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionMediator"/> class.
        /// </summary>
        /// <param name="auctionService">The service used to manage auction-related operations.</param>
        /// <param name="personService">The service used to manage person-related operations.</param>
        public AuctionMediator(IAuctionService auctionService, IPersonService personService)
        {
            Logger.Info("Initializing AuctionMediator.");

            this.auctionService = auctionService ?? throw new ArgumentNullException(nameof(auctionService));
            this.personService = personService ?? throw new ArgumentNullException(nameof(personService));

            Logger.Info("AuctionMediator initialized successfully.");
        }

        /// <summary>
        /// Starts a new auction for a specified product on behalf of a person.
        /// </summary>
        /// <param name="person">The person initiating the auction.</param>
        /// <param name="product">The product to be auctioned.</param>
        /// <param name="startDate">The start date of the auction.</param>
        /// <param name="endDate">The end date of the auction.</param>
        /// <param name="startingPrice">The starting price of the auction.</param>
        /// <param name="currency">The currency in which the auction is conducted.</param>
        /// <exception cref="ArgumentException">Thrown if the parameters are invalid.</exception>
        public void StartAuction(Person person, Product product, DateTime startDate, DateTime endDate, double startingPrice, string currency)
        {
            Logger.Info($"Starting auction for person with ID: {person.Id}, product with ID: {product.Id}");

            try
            {
                this.personService.StartAuction(person);
                this.auctionService.StartAuction(person, product, startDate, endDate, startingPrice, currency);

                Logger.Info("Auction started successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to start auction.", ex);
                throw;
            }
        }

        /// <summary>
        /// Adds a Bid to a specified auction on behalf of a person.
        /// </summary>
        /// <param name="person">The person placing the Bid.</param>
        /// <param name="auction">The auction to which the Bid is being added.</param>
        /// <param name="bid">The Bid being placed.</param>
        /// <exception cref="ArgumentException">Thrown if the Bid is invalid or the auction does not accept it.</exception>
        public void AddBid(Person person, Auction auction, Bid bid)
        {
            Logger.Info($"Adding bid for person with ID: {person.Id} to auction with ID: {auction.Id}");

            this.personService.AddBid(person, bid);
            this.auctionService.AddBid(auction, bid);

            Logger.Info("Bid added successfully.");
        }

        /// <summary>
        /// Finalizes an auction on behalf of a person.
        /// </summary>
        /// <param name="person">The person requesting to finalize the auction.</param>
        /// <param name="auction">The auction to be finalized.</param>
        /// <exception cref="InvalidOperationException">Thrown if the auction cannot be finalized.</exception>
        public void FinalizeAuction(Person person, Auction auction)
        {
            Logger.Info($"Finalizing auction with ID: {auction.Id} for person with ID: {person.Id}");

            try
            {
                this.personService.FinalizeAuction(person, auction);
                this.auctionService.FinalizeAuction(person, auction);

                Logger.Info("Auction finalized successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to finalize auction.", ex);
                throw;
            }
        }

        /// <summary>
        /// Provides feedback on a person's performance.
        /// </summary>
        /// <param name="person">The person receiving the feedback.</param>
        /// <param name="feedbackScore">The score representing the feedback.</param>
        /// <exception cref="ArgumentException">Thrown if the feedback score is invalid.</exception>
        public void ProvideFeedback(Person person, double feedbackScore)
        {
            Logger.Info($"Providing feedback to person with ID: {person.Id}. Feedback score: {feedbackScore}");
            this.personService.ProvideFeedback(person, feedbackScore);

            Logger.Info("Feedback provided successfully.");
        }
    }
}