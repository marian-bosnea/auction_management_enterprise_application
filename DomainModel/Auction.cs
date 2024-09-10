// <copyright file="Auction.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using log4net;

    /// <summary>
    /// Represents an auction associated with a specific product.
    /// </summary>
    public class Auction
    {
        /// <summary>
        /// The logger for logging actions in the class.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="Auction"/> class using the default constructor.
        /// </summary>
        public Auction()
        {
            Logger.Info("Auction instance created with default constructor.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Auction"/> class.
        /// </summary>
        /// <param name="seller">The seller that initiates this auction.</param>
        /// <param name="product">The product associated with this auction.</param>
        /// <param name="startDate">The start date of the auction.</param>
        /// <param name="endDate">The end date of the auction.</param>
        /// <param name="startingPrice">The starting price of the auction.</param>
        /// <param name="currency">The currency in which the auction is conducted.</param>
        public Auction(Person seller, Product product, DateTime startDate, DateTime endDate, double startingPrice, string currency)
        {
            Logger.Info("Initializing Auction instance with provided parameters.");

            try
            {
                if (startDate < DateTime.Now)
                {
                    Logger.Error("Invalid start date: Start date cannot be in the past.");
                    throw new ArgumentException("Start date cannot be in the past.");
                }

                if (endDate < DateTime.Now)
                {
                    Logger.Error("Invalid end date: End date cannot be in the past.");
                    throw new ArgumentException("End date cannot be in the past.");
                }

                if (endDate <= startDate)
                {
                    Logger.Error("Invalid date range: End date must be after the start date.");
                    throw new ArgumentException("End date must be after the start date.");
                }

                if (startingPrice <= 0)
                {
                    Logger.Error("Invalid starting price: Starting price must be greater than zero.");
                    throw new ArgumentException("Starting price must be greater than zero.");
                }

                if (currency.Length != 3)
                {
                    Logger.Error("Invalid currency: Currency ISO code must be 3 characters long.");
                    throw new ArgumentException("Currency ISO code must be 3 characters long.");
                }

                this.Seller = seller ?? throw new ArgumentNullException("Seller must not be null.");
                this.Product = product ?? throw new ArgumentNullException("Product must not be null");
                this.StartDate = startDate;
                this.EndDate = endDate;
                this.StartingPrice = startingPrice;
                this.Currency = currency;
                this.Bids = new List<Bid>();
                this.IsCompleted = false;

                Logger.Info($"Auction created successfully for product {product.Name}, starting from {startDate} to {endDate}.");
            }
            catch (Exception ex)
            {
                Logger.Error("Error during auction initialization.", ex);
                throw;
            }
        }

        /// <summary>
        /// Gets or sets the unique identifier for the auction.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the seller who initiated the auction.
        /// </summary>
        [Required(ErrorMessage = "Seller is required")]
        public Person Seller { get; set; }

        /// <summary>
        /// Gets or sets the product associated with this auction.
        /// </summary>
        [Required(ErrorMessage = "Product is required.")]
        public Product Product { get; set; }

        /// <summary>
        /// Gets or sets the start date of the auction.
        /// </summary>
        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.DateTime)]
        [CurrentOrFutureDate(ErrorMessage = "The start date cannot be earlier than the current date.")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the auction.
        /// </summary>
        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the starting price of the auction.
        /// </summary>
        [Required(ErrorMessage = "Starting price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Starting price must be greater than zero.")]
        public double StartingPrice { get; set; }

        /// <summary>
        /// Gets or sets the currency in which the auction is conducted.
        /// </summary>
        [Required(ErrorMessage = "Currency is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a valid 3-letter ISO code.")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the list of Bids made in this auction.
        /// </summary>
        public List<Bid> Bids { get; set; } = new List<Bid>();

        /// <summary>
        /// Gets or sets a value indicating whether the auction is completed.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Adds a Bid to the list of Bids associated with the auction.
        /// </summary>
        /// <param name="bid">The Bid to be added to the auction.</param>
        public void AddBid(Bid bid)
        {
            if (bid == null)
            {
                Logger.Error("Attempted to add a null bid.");
                throw new ArgumentNullException(nameof(bid));
            }

            if (this.IsCompleted)
            {
                Logger.Warn("Attempted to add a bid to a completed auction.");
                return;
            }

            Logger.Info($"Adding bid for auction on product {this.Product.Name}.");
            this.Bids.Add(bid);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            Logger.Debug($"ToString called for Auction with product {this.Product.Name}.");
            return $"Auction for {this.Product.Name} from {this.StartDate} to {this.EndDate} with starting price {this.StartingPrice} {this.Currency}";
        }
    }
}