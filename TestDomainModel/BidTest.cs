namespace DomainModel.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the <see cref="Bid"/> class.
    /// </summary>
    [TestClass]
    public class BidTest
    {
        /// <summary>
        /// Tests that a <see cref="Bid"/> is created correctly with valid parameters.
        /// </summary>
        [TestMethod]
        public void Constructor_ValidParameters_ShouldCreateBid()
        {
            // Arrange
            double amount = 100.0;
            string currency = "USD";
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddDays(1), DateTime.Now.AddDays(10), 10.0, "USD");

            // Act
            var bid = new Bid(amount, currency);

            // Assert
            Assert.AreEqual(amount, bid.Amount);
            Assert.AreEqual(currency, bid.Currency);
            Assert.IsTrue(bid.BidTime <= DateTime.Now); // BidTime should be less than or equal to now
        }

        /// <summary>
        /// Tests that an <see cref="ArgumentException"/> is thrown when an invalid (negative) amount is provided.
        /// </summary>
        [TestMethod]
        public void Constructor_InvalidAmount_ShouldThrowArgumentException()
        {
            // Arrange
            double invalidAmount = -10.0;
            string currency = "USD";
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddDays(1), DateTime.Now.AddDays(10), 10.0, "USD");

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => new Bid(invalidAmount, currency));
        }

        /// <summary>
        /// Tests that an <see cref="ArgumentNullException"/> is thrown when a <see langword="null"/> currency is provided.
        /// </summary>
        [TestMethod]
        public void Constructor_NullCurrency_ShouldThrowArgumentNullException()
        {
            // Arrange
            double amount = 100.0;
            string currency = null;
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddDays(1), DateTime.Now.AddDays(10), 10.0, "USD");

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new Bid(amount, currency));
        }

        /// <summary>
        /// Tests that setting an invalid (negative) amount to an existing <see cref="Bid"/> throws an <see cref="ArgumentException"/>.
        /// </summary>
        [TestMethod]
        public void Amount_SetToInvalidValue_ShouldThrowValidationException()
        {
            // Arrange
            var bid = new Bid(100.0, "USD");

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => bid.Amount = -1.0);
            Assert.AreEqual("Bid amount must be greater than zero.", exception.Message);
        }

        /// <summary>
        /// Tests that setting an invalid currency (not a 3-letter ISO code) throws an <see cref="ArgumentException"/>.
        /// </summary>
        [TestMethod]
        public void Currency_SetToInvalidValue_ShouldThrowValidationException()
        {
            // Arrange
            var bid = new Bid(100.0, "USD");

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => bid.Currency = "US");
            Assert.AreEqual("Currency must be a valid 3-letter ISO code.", exception.Message);
        }

        /// <summary>
        /// Tests the <see cref="Bid.ToString"/> method to ensure it returns the expected formatted string.
        /// </summary>
        [TestMethod]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var bid = new Bid(100.0, "USD");

            // Act
            string result = bid.ToString();

            // Assert
            Assert.AreEqual($"{bid.Amount} {bid.Currency} at {bid.BidTime}", result);
        }

        /// <summary>
        /// Tests that the <see cref="Bid"/> constructor sets the <see cref="Bid.Bidder"/> property correctly.
        /// </summary>
        [TestMethod]
        public void Constructor_ValidParameters_ShouldSetBidderProperty()
        {
            // Arrange
            var bidder = new Person("Jane Doe");
            double amount = 200.0;
            string currency = "USD";

            // Act
            var bid = new Bid(amount, currency) { Bidder = bidder };

            // Assert
            Assert.AreEqual(bidder, bid.Bidder);
        }

        /// <summary>
        /// Tests that setting the <see cref="Bid.BidTime"/> to the current date does not throw an exception.
        /// </summary>
        [TestMethod]
        public void BidTime_SetToCurrentDate_ShouldNotThrowException()
        {
            // Arrange
            var bid = new Bid(100.0, "USD");
            DateTime currentTime = DateTime.Now;

            // Act & Assert
            bid.BidTime = currentTime;
            Assert.AreEqual(currentTime, bid.BidTime);
        }

        /// <summary>
        /// Tests the <see cref="Bid.ToString"/> method with different currencies to ensure correct formatting.
        /// </summary>
        [TestMethod]
        public void ToString_WithDifferentCurrencies_ShouldFormatCorrectly()
        {
            // Arrange
            var bid1 = new Bid(100.0, "USD") { BidTime = DateTime.Now };
            var bid2 = new Bid(150.0, "EUR") { BidTime = DateTime.Now.AddHours(-1) };

            // Act
            string result1 = bid1.ToString();
            string result2 = bid2.ToString();

            // Assert
            Assert.AreEqual($"{bid1.Amount} {bid1.Currency} at {bid1.BidTime}", result1);
            Assert.AreEqual($"{bid2.Amount} {bid2.Currency} at {bid2.BidTime}", result2);
        }

        /// <summary>
        /// Tests the <see cref="Bid.ToString"/> method with different times to ensure correct formatting.
        /// </summary>
        [TestMethod]
        public void ToString_WithDifferentTimes_ShouldFormatCorrectly()
        {
            // Arrange
            var bid = new Bid(200.0, "USD");

            DateTime pastTime = DateTime.Now.AddDays(-1);
            DateTime futureTime = DateTime.Now.AddDays(1);

            // Act
            bid.BidTime = pastTime;
            string resultPast = bid.ToString();

            bid.BidTime = futureTime;
            string resultFuture = bid.ToString();

            // Assert
            Assert.AreEqual($"{bid.Amount} {bid.Currency} at {pastTime}", resultPast);
            Assert.AreEqual($"{bid.Amount} {bid.Currency} at {futureTime}", resultFuture);
        }

        /// <summary>
        /// Tests that a zero amount for a <see cref="Bid"/> throws an <see cref="ArgumentException"/>.
        /// </summary>
        [TestMethod]
        public void Amount_Zero_ShouldThrowArgumentException()
        {
            // Arrange
            double invalidAmount = 0.0;

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => new Bid(invalidAmount, "USD"));
            Assert.AreEqual("Bid amount must be greater than zero.", exception.Message);
        }

        /// <summary>
        /// Tests that an invalid currency length throws a <see cref="ValidationException"/> when validating a <see cref="Bid"/> object.
        /// </summary>
        [TestMethod]
        public void Currency_NotThreeCharacters_ShouldThrowValidationException()
        {
            // Arrange
            string invalidCurrency = "US";
            var bid = new Bid(100.0, invalidCurrency);
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(bid)
            {
                MemberName = nameof(Bid.Currency),
            };

            // Act & Assert
            bool isValid = Validator.TryValidateProperty(invalidCurrency, validationContext, validationResults);

            Assert.IsFalse(isValid);
            Assert.AreEqual("Currency must be a valid 3-letter ISO code.", validationResults[0].ErrorMessage);
        }

        /// <summary>
        /// Tests that setting the <see cref="Bid.Bidder"/> property to <see langword="null"/> does not throw an exception.
        /// </summary>
        [TestMethod]
        public void BidderProperty_SetToNull_ShouldNotThrowException()
        {
            // Arrange
            var bid = new Bid(100.0, "USD");

            // Act
            bid.Bidder = null;

            // Assert
            Assert.IsNull(bid.Bidder);
        }

        /// <summary>
        /// Creates a valid <see cref="Auction"/> instance for testing purposes.
        /// </summary>
        /// <returns>A <see cref="Auction"/> instance with valid parameters.</returns>
        private Auction CreateValidAuction()
        {
            return new Auction(
                new Person("John Doe"),
                new Product(1, "Product", "Description", new List<Category>()),
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(10),
                10.0,
                "USD");
        }
    }
}
