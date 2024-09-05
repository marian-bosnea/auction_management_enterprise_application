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
    public class Auction : IAuction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Auction"/> class.
        /// </summary>
        /// <param name="product">The product associated with this auction.</param>
        /// <param name="startDate">The start date of the auction.</param>
        /// <param name="endDate">The end date of the auction.</param>
        /// <param name="startingPrice">The starting price of the auction.</param>
        /// <param name="currency">The currency in which the auction is conducted.</param>
        /// <exception cref="ArgumentException">Thrown when the start date is in the past, the end date is in the past, or the end date is before the start date.</exception>
        /// <exception cref="ArgumentException">Thrown when the starting price is less than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the product is null.</exception>
        public Auction(IProduct product, DateTime startDate, DateTime endDate, decimal startingPrice, string currency)
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

            this.Product = product ?? throw new ArgumentNullException(nameof(product));
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.StartingPrice = startingPrice;
            this.Currency = currency;
            this.Bids = new List<IBid>();
        }

        /// <summary>
        /// Gets or sets the unique identifier for the auction.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the product associated with this auction.
        /// </summary>
        [Required(ErrorMessage = "Product is required.")]
        public IProduct Product { get; set; }

        /// <summary>
        /// Gets or sets the start date of the auction.
        /// </summary>
        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.DateTime)]
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
        public decimal StartingPrice { get; set; }

        /// <summary>
        /// Gets or sets the currency in which the auction is conducted.
        /// </summary>
        [Required(ErrorMessage = "Currency is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a valid 3-letter ISO code.")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the list of bids made in this auction.
        /// </summary>
        public List<IBid> Bids { get; set; } = new List<IBid>();

        /// <summary>
        /// Adds a bid to the list of bids associated with the auction.
        /// </summary>
        /// <param name="bid">The bid to be added to the auction.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the bid is null.
        /// </exception>
        public void AddBid(IBid bid)
        {
            this.Bids.Add(bid);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Auction for {this.Product.Name} from {this.StartDate} to {this.EndDate} with starting price {this.StartingPrice} {this.Currency}";
        }
    }
}
