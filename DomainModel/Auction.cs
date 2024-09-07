// <copyright file="Auction.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents an auction associated with a specific product.
    /// </summary>
    public class Auction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Auction"/> class.
        /// </summary>
        /// <param name="seller">The seller that initiates this auction.</param>
        /// <param name="product">The product associated with this auction.</param>
        /// <param name="startDate">The start date of the auction.</param>
        /// <param name="endDate">The end date of the auction.</param>
        /// <param name="startingPrice">The starting price of the auction.</param>
        /// <param name="currency">The currency in which the auction is conducted.</param>
        /// <exception cref="ArgumentException">Thrown when the start date is in the past, the end date is in the past, or the end date is before the start date.</exception>
        /// <exception cref="ArgumentException">Thrown when the starting price is less than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the product is null.</exception>
        public Auction(Person seller, Product product, DateTime startDate, DateTime endDate, double startingPrice, string currency)
        {
            if (startDate < DateTime.Now)
            {
                throw new ArgumentException("Start date cannot be in the past.");
            }

            if (endDate < DateTime.Now)
            {
                throw new ArgumentException("End date cannot be in the past.");
            }

            if (endDate <= startDate)
            {
                throw new ArgumentException("End date must be after the start date.");
            }

            if (startingPrice <= 0)
            {
                throw new ArgumentException("Starting price must be greater than zero.");
            }

            if (currency.Length != 3)
            {
                throw new ArgumentException("Currency ISO code must be 3 characters long.");

            }

            this.Seller = seller ?? throw new ArgumentNullException(nameof(seller));
            this.Product = product ?? throw new ArgumentNullException(nameof(product));
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.StartingPrice = startingPrice;
            this.Currency = currency;
            this.Bids = new List<Bid>();
            this.IsCompleted = false;
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
        [EndDateLaterThanStartDate("StartDate", ErrorMessage = "End date must be later than the start date.")]
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
        /// <exception cref="ArgumentNullException">
        /// Thrown if the Bid is null.
        /// </exception>
        public void AddBid(Bid bid)
        {
            if (bid == null)
            {
                throw new ArgumentNullException(nameof(bid));
            }

            if (this.IsCompleted)
            {
                return;
            }

            this.Bids.Add(bid);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Auction for {this.Product.Name} from {this.StartDate} to {this.EndDate} with starting price {this.StartingPrice} {this.Currency}";
        }
    }
}
