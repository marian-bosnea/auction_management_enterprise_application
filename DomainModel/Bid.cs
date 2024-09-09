// <copyright file="Bid.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using log4net;

    /// <summary>
    /// Represents a Bid made in an auction.
    /// </summary>
    public class Bid
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The amount of the transaction or financial value, represented as a double.
        /// </summary>
        private double amount;

        /// <summary>
        /// The currency in which the <see cref="amount"/> is denominated, represented as a string (e.g., "USD", "EUR").
        /// </summary>
        private string currency;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bid"/> class using the empty constructor.
        /// </summary>
        public Bid()
        {
            Logger.Info("Bid object initialized using the empty constructor.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bid"/> class.
        /// </summary>
        /// <param name="amount">The amount of the Bid.</param>
        /// <param name="currency">The currency in which the Bid is made.</param>
        /// <exception cref="ArgumentException">Thrown when the Bid amount is less than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the currency is null.</exception>
        public Bid(double amount, string currency)
        {
            Logger.Info($"Initializing Bid with amount: {amount}, currency: {currency}");

            if (amount <= 0)
            {
                Logger.Error("Bid amount is less than or equal to zero.");
                throw new ArgumentException("Bid amount must be greater than zero.");
            }

            this.amount = amount;
            this.currency = currency ?? throw new ArgumentNullException(nameof(currency));

            this.BidTime = DateTime.Now;
            Logger.Info("Bid initialized successfully.");
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
                Logger.Info($"Setting Bid amount to: {value}");
                if (value < 0)
                {
                    Logger.Error("Bid amount must be greater than zero.");
                    throw new ArgumentException("Bid amount must be greater than zero.");
                }

                this.amount = value;
                Logger.Info($"Bid amount set successfully to: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the currency in which the Bid is made.
        /// </summary>
        [Required(ErrorMessage = "Currency is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a valid 3-letter ISO code.")]
        public string Currency
        {
            get
            {
                return this.currency;
            }

            set
            {
                Logger.Info($"Setting Bid currency to: {value}");
                if (value.Length != 3)
                {
                    Logger.Error("Currency must be a valid 3-letter ISO code.");
                    throw new ArgumentException("Currency must be a valid 3-letter ISO code.");
                }

                this.currency = value;
                Logger.Info($"Bid currency set successfully to: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the time when the Bid was made.
        /// </summary>
        [Required(ErrorMessage = "Bid time is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Bid time must be a valid date and time.")]
        [CustomValidation(typeof(Bid), nameof(ValidateBidTime))]
        public DateTime BidTime { get; set; }

        /// <summary>
        /// Validates that the Bid time is not in the future.
        /// </summary>
        /// <param name="bidTime">The Bid time to validate.</param>
        /// <returns>A ValidationResult indicating whether the Bid time is valid.</returns>
        public static ValidationResult ValidateBidTime(DateTime bidTime)
        {
            if (bidTime > DateTime.Now)
            {
                Logger.Warn("Bid time is in the future.");
                return new ValidationResult("Bid time cannot be in the future.");
            }

            return ValidationResult.Success;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            Logger.Info($"Converting Bid to string: {this.Amount} {this.Currency} at {this.BidTime}");
            return $"{this.Amount} {this.Currency} at {this.BidTime}";
        }
    }
}