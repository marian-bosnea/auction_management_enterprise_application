// <copyright file="Bid.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a bid made in an auction.
    /// </summary>;
    public class Bid : IBid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bid"/> class.
        /// </summary>
        /// <param name="amount">The amount of the bid.</param>
        /// <param name="currency">The currency in which the bid is made.</param>
        /// <param name="auction">The auction associated with this bid.</param>
        /// <exception cref="ArgumentException">Thrown when the bid amount is less than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the currency or auction is null.</exception>
        public Bid(decimal amount, string currency, Auction auction)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Bid amount must be greater than zero.");
            }

            this.Amount = amount;
            this.Currency = currency ?? throw new ArgumentNullException(nameof(currency));
            this.BidTime = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the bid.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the amount of the bid.
        /// </summary>
        [Required(ErrorMessage = "Bid amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Bid amount must be greater than zero.")]
        public decimal Amount { get;  set; }

        /// <summary>
        /// Gets or sets the currency in which the bid is made.
        /// </summary>
        [Required(ErrorMessage = "Currency is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a valid 3-letter ISO code.")]
        public string Currency { get;  set; }

        /// <summary>
        /// Gets or sets the time when the bid was made.
        /// </summary>
        [Required(ErrorMessage = "Bid time is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Bid time must be a valid date and time.")]
        [CustomValidation(typeof(Bid), nameof(ValidateBidTime))]
        public DateTime BidTime { get;  set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.Amount} {this.Currency} at {this.BidTime}";
        }

        /// <summary>
        /// Validates that the bid time is not in the future.
        /// </summary>
        /// <param name="bidTime">The bid time to validate.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A ValidationResult indicating whether the bid time is valid.</returns>
        private ValidationResult ValidateBidTime(DateTime bidTime, ValidationContext validationContext)
        {
            if (bidTime > DateTime.Now)
            {
                return new ValidationResult("Bid time cannot be in the future.");
            }

            return ValidationResult.Success;
        }
    }
}
