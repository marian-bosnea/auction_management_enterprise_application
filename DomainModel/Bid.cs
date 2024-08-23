namespace DomainModel
{
    using System;

    /// <summary>
    /// Represents a bid made in an auction.
    /// </summary>
    public class Bid
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
            this.Auction = auction ?? throw new ArgumentNullException(nameof(auction));
            this.BidTime = DateTime.Now;
        }

        /// <summary>
        /// Gets the amount of the bid.
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Gets the currency in which the bid is made.
        /// </summary>
        public string Currency { get; private set; }

        /// <summary>
        /// Gets the time when the bid was made.
        /// </summary>
        public DateTime BidTime { get; private set; }

        /// <summary>
        /// Gets the auction associated with this bid.
        /// </summary>
        public Auction Auction { get; private set; }

        /// <summary>
        /// Returns a string representation of the bid.
        /// </summary>
        /// <returns>A string that represents the current bid.</returns>
        public override string ToString()
        {
            return $"{Amount} {Currency} at {BidTime}";
        }
    }
}
