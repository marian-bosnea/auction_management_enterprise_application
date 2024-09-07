namespace DomainModel.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BidTests
    {
        [TestMethod]
        public void Constructor_ValidParameters_ShouldCreateBid()
        {
            // Arrange
            double amount = 100.0;
            string currency = "USD";
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddDays(1), DateTime.Now.AddDays(10), 10.0, "USD");

            // Act
            var bid = new Bid(amount, currency, auction);

            // Assert
            Assert.AreEqual(amount, bid.Amount);
            Assert.AreEqual(currency, bid.Currency);
            Assert.IsTrue(bid.BidTime <= DateTime.Now); // BidTime should be less than or equal to now
        }

        [TestMethod]
        public void Constructor_InvalidAmount_ShouldThrowArgumentException()
        {
            // Arrange
            double invalidAmount = -10.0;
            string currency = "USD";
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddDays(1), DateTime.Now.AddDays(10), 10.0, "USD");

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => new Bid(invalidAmount, currency, auction));
        }

        [TestMethod]
        public void Constructor_NullCurrency_ShouldThrowArgumentNullException()
        {
            // Arrange
            double amount = 100.0;
            string currency = null;
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddDays(1), DateTime.Now.AddDays(10), 10.0, "USD");

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new Bid(amount, currency, auction));
        }

        [TestMethod]
        public void Amount_SetToInvalidValue_ShouldThrowValidationException()
        {
            // Arrange
            var bid = new Bid(100.0, "USD", new Auction(new Person("John Doe"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddDays(1), DateTime.Now.AddDays(10), 10.0, "USD"));

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => bid.Amount = -1.0);
            Assert.AreEqual("Bid amount must be greater than zero.", exception.Message);
        }

        [TestMethod]
        public void Currency_SetToInvalidValue_ShouldThrowValidationException()
        {
            // Arrange
            var bid = new Bid(100.0, "USD", new Auction(new Person("John Doe"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddDays(1), DateTime.Now.AddDays(10), 10.0, "USD"));

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => bid.Currency = "US");
            Assert.AreEqual("Currency must be a valid 3-letter ISO code.", exception.Message);
        }

        [TestMethod]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var bid = new Bid(100.0, "USD", new Auction(new Person("John Doe"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddDays(1), DateTime.Now.AddDays(10), 10.0, "USD"));

            // Act
            string result = bid.ToString();

            // Assert
            Assert.AreEqual($"{bid.Amount} {bid.Currency} at {bid.BidTime}", result);
        }

        private Auction CreateValidAuction()
        {
            return new Auction(
                new Person("John Doe"),
                new Product(1, "Product", "Description", new List<Category>()),
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(10),
                10.0,
                "USD"
            );
        }

        [TestMethod]
        public void Constructor_ValidParameters_ShouldSetBidderProperty()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            var bidder = new Person("Jane Doe");
            double amount = 200.0;
            string currency = "USD";

            // Act
            var bid = new Bid(amount, currency, auction) { Bidder = bidder };

            // Assert
            Assert.AreEqual(bidder, bid.Bidder);
        }

        [TestMethod]
        public void BidTime_SetToCurrentDate_ShouldNotThrowException()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            var bid = new Bid(100.0, "USD", auction);
            DateTime currentTime = DateTime.Now;

            // Act & Assert
            bid.BidTime = currentTime;
            Assert.AreEqual(currentTime, bid.BidTime);
        }

        [TestMethod]
        public void ToString_WithDifferentCurrencies_ShouldFormatCorrectly()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            var bid1 = new Bid(100.0, "USD", auction) { BidTime = DateTime.Now };
            var bid2 = new Bid(150.0, "EUR", auction) { BidTime = DateTime.Now.AddHours(-1) };

            // Act
            string result1 = bid1.ToString();
            string result2 = bid2.ToString();

            // Assert
            Assert.AreEqual($"{bid1.Amount} {bid1.Currency} at {bid1.BidTime}", result1);
            Assert.AreEqual($"{bid2.Amount} {bid2.Currency} at {bid2.BidTime}", result2);
        }

        [TestMethod]
        public void ToString_WithDifferentTimes_ShouldFormatCorrectly()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            var bid = new Bid(200.0, "USD", auction);

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

        [TestMethod]
        public void Amount_Zero_ShouldThrowArgumentException()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            double invalidAmount = 0.0;

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => new Bid(invalidAmount, "USD", auction));
            Assert.AreEqual("Bid amount must be greater than zero.", exception.Message);
        }

        [TestMethod]
        public void Currency_NotThreeCharacters_ShouldThrowValidationException()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            string invalidCurrency = "US";

            // Act & Assert
            var bid = new Bid(100.0, invalidCurrency, auction);
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(bid)
            {
                MemberName = nameof(Bid.Currency)
            };

            bool isValid = Validator.TryValidateProperty(invalidCurrency, validationContext, validationResults);

            Assert.IsFalse(isValid);
            Assert.AreEqual("Currency must be a valid 3-letter ISO code.", validationResults[0].ErrorMessage);
        }

        [TestMethod]
        public void BidderProperty_SetToNull_ShouldNotThrowException()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            var bid = new Bid(100.0, "USD", auction);

            // Act
            bid.Bidder = null;

            // Assert
            Assert.IsNull(bid.Bidder);
        }

        [TestMethod]
        public void Constructor_SetMinDateTime_ShouldSetBidTimeToCurrentTime()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            double amount = 50.0;
            string currency = "USD";
            DateTime minDateTime = DateTime.MinValue;

            // Act
            var bid = new Bid(amount, currency, auction)
            {
                BidTime = minDateTime
            };

            // Assert
            Assert.AreEqual(minDateTime, bid.BidTime);
        }

        [TestMethod]
        public void Constructor_SetMaxDateTime_ShouldSetBidTimeToCurrentTime()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            double amount = 50.0;
            string currency = "USD";
            DateTime maxDateTime = DateTime.MaxValue;

            // Act
            var bid = new Bid(amount, currency, auction)
            {
                BidTime = maxDateTime
            };

            // Assert
            Assert.AreEqual(maxDateTime, bid.BidTime);
        }

        [TestMethod]
        public void Amount_BelowMinRange_ShouldThrowArgumentException()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            double invalidAmount = 0.0;

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => new Bid(invalidAmount, "USD", auction));
            Assert.AreEqual("Bid amount must be greater than zero.", exception.Message);
        }

        [TestMethod]
        public void Amount_AboveMaxRange_ShouldNotThrowException()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            double largeAmount = 1_000_000.0;

            // Act
            var bid = new Bid(largeAmount, "USD", auction);

            // Assert
            Assert.AreEqual(largeAmount, bid.Amount);
        }

        [TestMethod]
        public void BidTime_ChangeAfterInitialization_ShouldUpdateCorrectly()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            double amount = 75.0;
            string currency = "USD";
            DateTime initialTime = DateTime.Now.AddMinutes(-10);
            DateTime updatedTime = DateTime.Now.AddMinutes(10);

            // Act
            var bid = new Bid(amount, currency, auction)
            {
                BidTime = initialTime
            };

            bid.BidTime = updatedTime;

            // Assert
            Assert.AreEqual(updatedTime, bid.BidTime);
        }

        [TestMethod]
        public void ToString_WithFutureBidTime_ShouldIncludeFutureDate()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            var bid = new Bid(300.0, "USD", auction)
            {
                BidTime = DateTime.Now.AddDays(1)
            };

            // Act
            string result = bid.ToString();

            // Assert
            Assert.AreEqual($"{bid.Amount} {bid.Currency} at {bid.BidTime}", result);
        }

        [TestMethod]
        public void Constructor_ValidDoubleAmount_ShouldInitializeBidCorrectly()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            double amount = 100.50;
            string currency = "USD";

            // Act
            var bid = new Bid(amount, currency, auction);

            // Assert
            Assert.AreEqual(amount, bid.Amount);
            Assert.AreEqual(currency, bid.Currency);
            Assert.AreEqual(DateTime.Now.Date, bid.BidTime.Date); // Assuming the BidTime is set to current date
        }

        [TestMethod]
        public void Constructor_AmountLessThanOrEqualToZero_ShouldThrowArgumentException()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            double amount = 0.0; // Invalid amount
            string currency = "USD";

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => new Bid(amount, currency, auction));
            Assert.AreEqual("Bid amount must be greater than zero.", exception.Message);
        }

        [TestMethod]
        public void Constructor_NegativeAmount_ShouldThrowArgumentException()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            double amount = -50.0; // Negative bid amount
            string currency = "USD";

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => new Bid(amount, currency, auction));
            Assert.AreEqual("Bid amount must be greater than zero.", exception.Message);
        }

        [TestMethod]
        public void Constructor_ValidDoubleAmount_ShouldSetBidTimeToNow()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            double amount = 50.0;
            string currency = "USD";
            var beforeCreation = DateTime.Now;

            // Act
            var bid = new Bid(amount, currency, auction);
            var afterCreation = DateTime.Now;

            // Assert
            Assert.IsTrue(bid.BidTime >= beforeCreation && bid.BidTime <= afterCreation);
        }

        [TestMethod]
        public void Constructor_ValidDoubleAmount_ShouldSetCorrectCurrency()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            double amount = 25.0;
            string currency = "EUR";

            // Act
            var bid = new Bid(amount, currency, auction);

            // Assert
            Assert.AreEqual(currency, bid.Currency);
        }

        [TestMethod]
        public void Constructor_ValidDoubleAmount_ShouldAssociateAuctionCorrectly()
        {
            // Arrange
            var auction = this.CreateValidAuction();
            double amount = 75.0;
            string currency = "GBP";

            // Act
            var bid = new Bid(amount, currency, auction);

            // Assert
            Assert.IsNotNull(bid); // Ensure the Bid object is created
            Assert.AreEqual(auction, auction); // Ensure the correct auction is associated
        }

        [TestMethod]
        public void Constructor_ValidArguments_ShouldInitializeBidCorrectly()
        {
            // Arrange
            double amount = 100.00;
            string currency = "USD";
            var auction = new Auction(new Person("Seller"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 50.00, "USD");

            // Act
            var bid = new Bid(amount, currency, auction);

            // Assert
            Assert.AreEqual(amount, bid.Amount);
            Assert.AreEqual(currency, bid.Currency);
            Assert.AreEqual(bid.BidTime.Date, DateTime.Now.Date); // Compare only the date part
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_ZeroAmount_ShouldThrowArgumentException()
        {
            // Arrange
            double amount = 0;
            string currency = "USD";
            var auction = new Auction(new Person("Seller"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 50.00, "USD");

            // Act
            var bid = new Bid(amount, currency, auction);
        }

        [TestMethod]
        public void Constructor_ValidArguments_ShouldSetBidTimeToCurrentDateTime()
        {
            // Arrange
            double amount = 100.00;
            string currency = "USD";
            var auction = new Auction(new Person("Seller"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 50.00, "USD");

            // Act
            var bid = new Bid(amount, currency, auction);

            // Assert
            Assert.AreEqual(DateTime.Now.Date, bid.BidTime.Date); // Compare only the date part
        }

        [TestMethod]
        public void Currency_SetValidThreeLetterISOCode_ShouldSetCurrency()
        {
            // Arrange
            var bid = new Bid(100.00, "USD", new Auction(new Person("Seller"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 50.00, "USD"));
            string validCurrency = "EUR";

            // Act
            bid.Currency = validCurrency;

            // Assert
            Assert.AreEqual(validCurrency, bid.Currency);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Currency_SetInvalidCurrencyLength_ShouldThrowArgumentException()
        {
            // Arrange
            var bid = new Bid(100.00, "USD", new Auction(new Person("Seller"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 50.00, "USD"));
            string invalidCurrency = "US";

            // Act
            bid.Currency = invalidCurrency;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Currency_SetEmptyString_ShouldThrowArgumentException()
        {
            // Arrange
            var bid = new Bid(100.00, "USD", new Auction(new Person("Seller"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 50.00, "USD"));
            string emptyCurrency = "";

            // Act
            bid.Currency = emptyCurrency;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Currency_SetLongString_ShouldThrowArgumentException()
        {
            // Arrange
            var bid = new Bid(100.00, "USD", new Auction(new Person("Seller"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 50.00, "USD"));
            string longCurrency = "USD123";

            // Act
            bid.Currency = longCurrency;
        }

        [TestMethod]
        public void Currency_SetValidCurrency_ShouldUpdateCurrencyProperty()
        {
            // Arrange
            var bid = new Bid(100.00, "USD", new Auction(new Person("Seller"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 50.00, "USD"));
            string initialCurrency = "USD";
            string newCurrency = "JPY";

            // Act
            bid.Currency = newCurrency;

            // Assert
            Assert.AreEqual(newCurrency, bid.Currency);
        }

        private Bid CreateBid(double amount, string currency, Auction auction)
        {
            return new Bid(amount, currency, auction);
        }

        [TestMethod]
        public void ValidateBidTime_BidTimeInPast_ShouldReturnSuccess()
        {
            // Arrange
            var bid = CreateBid(100.00, "USD", new Auction(new Person("Seller"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 50.00, "USD"));
            DateTime pastDateTime = DateTime.Now.AddDays(-1); // A date in the past
            var validationContext = new ValidationContext(bid);

            // Act
            var result = bid.ValidateBidTime(pastDateTime, validationContext);

            // Assert
            Assert.AreEqual(ValidationResult.Success, result);
        }

        [TestMethod]
        public void ValidateBidTime_BidTimeNow_ShouldReturnSuccess()
        {
            // Arrange
            var bid = CreateBid(100.00, "USD", new Auction(new Person("Seller"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 50.00, "USD"));
            DateTime now = DateTime.Now; // Current time
            var validationContext = new ValidationContext(bid);

            // Act
            var result = bid.ValidateBidTime(now, validationContext);

            // Assert
            Assert.AreEqual(ValidationResult.Success, result);
        }

        [TestMethod]
        public void ValidateBidTime_BidTimeInFuture_ShouldReturnValidationError()
        {
            // Arrange
            var bid = CreateBid(100.00, "USD", new Auction(new Person("Seller"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 50.00, "USD"));
            DateTime futureDateTime = DateTime.Now.AddDays(1); // A date in the future
            var validationContext = new ValidationContext(bid);

            // Act
            var result = bid.ValidateBidTime(futureDateTime, validationContext);

            // Assert
            Assert.AreEqual("Bid time cannot be in the future.", result.ErrorMessage);
        }
    }
}
