// <copyright file="AuctionMediator.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace ServiceLayer
{
    using System;
    using DomainModel;
    using ServiceLayer.Interfaces;

    /// <summary>
    /// Mediates interactions between auction and person services, providing a unified interface for managing auctions and bids.
    /// </summary>
    public class AuctionMediator
    {
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
            this.auctionService = auctionService ?? throw new ArgumentNullException(nameof(auctionService));
            this.personService = personService ?? throw new ArgumentNullException(nameof(personService));
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
        public void StartAuction(IPerson person, IProduct product, DateTime startDate, DateTime endDate, decimal startingPrice, string currency)
        {
            this.personService.StartAuction(person);
            this.auctionService.StartAuction(person, product, startDate, endDate, startingPrice, currency);
        }

        /// <summary>
        /// Adds a bid to a specified auction on behalf of a person.
        /// </summary>
        /// <param name="person">The person placing the bid.</param>
        /// <param name="auction">The auction to which the bid is being added.</param>
        /// <param name="bid">The bid being placed.</param>
        /// <exception cref="ArgumentException">Thrown if the bid is invalid or the auction does not accept it.</exception>
        public void AddBid(IPerson person, IAuction auction, IBid bid)
        {
            this.personService.AddBid(person, bid);
            this.auctionService.AddBid(auction, bid);
        }

        /// <summary>
        /// Finalizes an auction on behalf of a person.
        /// </summary>
        /// <param name="person">The person requesting to finalize the auction.</param>
        /// <param name="auction">The auction to be finalized.</param>
        /// <exception cref="InvalidOperationException">Thrown if the auction cannot be finalized.</exception>
        public void FinalizeAuction(IPerson person, IAuction auction)
        {
            this.personService.FinalizeAuction(person, auction);
            this.auctionService.FinalizeAuction(person, auction);
        }

        /// <summary>
        /// Provides feedback on a person's performance.
        /// </summary>
        /// <param name="person">The person receiving the feedback.</param>
        /// <param name="feedbackScore">The score representing the feedback.</param>
        /// <exception cref="ArgumentException">Thrown if the feedback score is invalid.</exception>
        public void ProvideFeedback(IPerson person, decimal feedbackScore)
        {
            this.personService.ProvideFeedback(person, feedbackScore);
        }
    }
}
