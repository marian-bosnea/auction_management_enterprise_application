namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    /// <summary>
    /// Represents a person who can initiate and manage auctions, with a score reflecting their reliability.
    /// </summary>
    public class Person
    {
        private readonly int maxActiveAuctions;
        private readonly int maxActiveAuctionsPerCategory;
        private readonly decimal seriousnessThreshold;
        private readonly List<Auction> activeAuctions;
        private int maxItemsBasedOnScore;

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        /// <param name="name">The name of the person.</param>
        public Person(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));

            // Read configuration values or set default values if not configured.
            this.maxActiveAuctions = int.Parse(ConfigurationManager.AppSettings["MaxActiveAuctions"] ?? "5");
            this.maxActiveAuctionsPerCategory = int.Parse(ConfigurationManager.AppSettings["MaxActiveAuctionsPerCategory"] ?? "3");
            this.seriousnessThreshold = decimal.Parse(ConfigurationManager.AppSettings["SeriousnessThreshold"] ?? "4.0");

            // Initialize the score to 5.0 and the list of active auctions.
            this.Score = 5.0m;
            this.activeAuctions = new List<Auction>();

            // Initialize maxItemsBasedOnScore based on initial seriousnessThreshold.
            UpdateMaxItemsBasedOnScore();
        }

        /// <summary>
        /// Gets the name of the person.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the score of the person, representing their reliability.
        /// </summary>
        public decimal Score { get; private set; }

        /// <summary>
        /// Gets the list of active auctions initiated by this person.
        /// </summary>
        public IReadOnlyList<Auction> ActiveAuctions => this.activeAuctions.AsReadOnly();

        /// <summary>
        /// Starts a new auction for the specified product.
        /// </summary>
        /// <param name="product">The product to be auctioned.</param>
        /// <param name="startDate">The start date of the auction.</param>
        /// <param name="endDate">The end date of the auction.</param>
        /// <param name="startingPrice">The starting price of the auction.</param>
        /// <param name="currency">The currency in which the auction is conducted.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the person has reached the maximum number of active auctions or the maximum number of active auctions in the product's categories.
        /// </exception>
        public void StartAuction(Product product, DateTime startDate, DateTime endDate, decimal startingPrice, string currency)
        {
            // Ensure the person's seriousness score allows them to start a new auction.
            if (this.Score < this.seriousnessThreshold)
            {
                throw new InvalidOperationException($"Cannot start a new auction. Seriousness score is below the required threshold of {this.seriousnessThreshold}.");
            }

            // Ensure the person has not exceeded the maximum number of active auctions.
            if (this.activeAuctions.Count >= this.maxItemsBasedOnScore)
            {
                throw new InvalidOperationException($"Cannot start a new auction. Maximum of {this.maxItemsBasedOnScore} active auctions allowed based on seriousness score.");
            }

            // Ensure the person has not exceeded the maximum number of active auctions per category.
            foreach (var category in product.Categories)
            {
                int activeAuctionsInCategory = this.activeAuctions.Count(a => a.Product.Categories.Contains(category));
                if (activeAuctionsInCategory >= this.maxActiveAuctionsPerCategory)
                {
                    throw new InvalidOperationException($"Cannot start a new auction. Maximum of {this.maxActiveAuctionsPerCategory} active auctions in category '{category.Name}' reached.");
                }
            }

            // Create a new Auction instance and add it to the active auctions list.
            var auction = new Auction(product, startDate, endDate, startingPrice, currency);
            this.activeAuctions.Add(auction);
        }

        /// <summary>
        /// Finalizes the auction by marking it as closed and removing it from the active auctions list.
        /// </summary>
        /// <param name="auction">The auction to finalize.</param>
        /// <exception cref="InvalidOperationException">Thrown if the auction is not found in the active auctions list or if the person is not the initiator.</exception>
        public void FinalizeAuction(Auction auction)
        {
            if (!this.activeAuctions.Contains(auction))
            {
                throw new InvalidOperationException("Cannot finalize an auction that is not active or was not initiated by this person.");
            }

            // Remove the auction from the active auctions list.
            this.activeAuctions.Remove(auction);

            // Increase the score by 0.1 if the auction had at least one bid.
            if (auction.Bids.Any())
            {
                AdjustScore(0.1m);
            }
        }

        /// <summary>
        /// Adjusts the person's score based on feedback or auction completion.
        /// </summary>
        /// <param name="amount">The amount to adjust the score by, between -0.1 and 0.1.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the amount is not within the valid range [-0.1, 0.1].</exception>
        public void AdjustScore(decimal amount)
        {
            if (amount < -0.1m || amount > 0.1m)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Score adjustment must be between -0.1 and 0.1.");
            }

            // Adjust the score and clamp it between 0 and 10.
            this.Score = Math.Max(0, Math.Min(10, this.Score + amount));

            // Recalculate maxItemsBasedOnScore based on the updated score.
            UpdateMaxItemsBasedOnScore();
        }

        /// <summary>
        /// Updates the maximum number of items that can be auctioned based on the current score.
        /// </summary>
        private void UpdateMaxItemsBasedOnScore()
        {
            // Calculate maxItemsBasedOnScore dynamically based on current seriousness score.
            this.maxItemsBasedOnScore = (int)Math.Max(1, 10 - (10 - this.Score) * 0.5m);
        }

        /// <summary>
        /// Provides feedback to this person, adjusting their score.
        /// </summary>
        /// <param name="feedbackScore">The feedback score to add, between -0.1 and 0.1.</param>
        public void ReceiveFeedback(decimal feedbackScore)
        {
            AdjustScore(feedbackScore);
        }
    }
}
