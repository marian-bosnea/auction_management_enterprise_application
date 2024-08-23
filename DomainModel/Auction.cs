// <copyright file="Auction.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.Collections.Generic;

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
        public Auction(Product product, DateTime startDate, DateTime endDate, decimal startingPrice, string currency)
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
        /// Gets the product associated with this auction.
        /// </summary>
        public IProduct Product { get; private set; }

        /// <summary>
        /// Gets the start date of the auction.
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// Gets the end date of the auction.
        /// </summary>
        public DateTime EndDate { get; private set; }

        /// <summary>
        /// Gets the starting price of the auction.
        /// </summary>
        public decimal StartingPrice { get; private set; }

        /// <summary>
        /// Gets the currency in which the auction is conducted.
        /// </summary>
        public string Currency { get; private set; }

        /// <summary>
        /// Gets the list of bids made in this auction.
        /// </summary>
        public List<IBid> Bids { get; private set; }

        /// <inheritdoc/>
        public void AddBid(IBid bid)
        {
            if (bid.Currency != this.Currency)
            {
                throw new ArgumentException("Bid currency must match auction currency.");
            }

            decimal minPrice = this.Bids.Count == 0 ? this.StartingPrice : this.Bids[this.Bids.Count - 1].Amount * 1.1m;
            if (bid.Amount < minPrice)
            {
                throw new ArgumentException("Bid amount must be at least 10% higher than the previous bid.");
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
