// <copyright file="Bid.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a Bid made in an auction.
    /// </summary>;
    public class Bid
    {
        private double amount;
        private string currency;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bid"/> class.
        /// </summary>
        /// <param name="amount">The amount of the Bid.</param>
        /// <param name="currency">The currency in which the Bid is made.</param>
        /// <param name="auction">The auction associated with this Bid.</param>
        /// <exception cref="ArgumentException">Thrown when the Bid amount is less than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the currency or auction is null.</exception>
        public Bid(double amount, string currency, Auction auction)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Bid amount must be greater than zero.");
            }

            this.amount = amount;
            this.currency = currency ?? throw new ArgumentNullException(nameof(currency));
            this.BidTime = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the Bid.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the person who placed the Bid.
        /// </summary>
        public Person Bidder { get; set; }

        /// <summary>
        /// Gets or sets the amount of the Bid.
        /// </summary>
        [Required(ErrorMessage = "Bid amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Bid amount must be greater than zero.")]
        public double Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Bid amount must be greater than zero.");
                }

                this.amount = value;
            }
        }

        /// <summary>
        /// Gets or sets the currency in which the Bid is made.
        /// </summary>
        [Required(ErrorMessage = "Currency is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a valid 3-letter ISO code.")]
        public string Currency { get { return this.currency;  } set
            {
                if(value.Length != 3)
                {
                    throw new ArgumentException("Currency must be a valid 3-letter ISO code.");
                }

                this.currency = value;
            }
        }

        /// <summary>
        /// Gets or sets the time when the Bid was made.
        /// </summary>
        [Required(ErrorMessage = "Bid time is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Bid time must be a valid date and time.")]
        [CustomValidation(typeof(Bid), nameof(ValidateBidTime))]
        public DateTime BidTime { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.Amount} {this.Currency} at {this.BidTime}";
        }

        /// <summary>
        /// Validates that the Bid time is not in the future.
        /// </summary>
        /// <param name="bidTime">The Bid time to validate.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A ValidationResult indicating whether the Bid time is valid.</returns>
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
