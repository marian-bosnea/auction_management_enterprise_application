using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModel;
using System;
using System.Collections.Generic;

namespace DomainModel.Tests
{
    [TestClass]
    public class AuctionTests
    {
        private Person CreateValidPerson()
        {
            return new Person("John Doe") { Role = PersonRole.Seller };
        }

        private Product CreateValidProduct()
        {
            return new Product(1, "Sample Product", "This is a sample product.", new List<Category>());
        }

        private Bid CreateValidBid(Auction auction)
        {
            return new Bid(15.00, "USD", auction)
            {
                Bidder = this.CreateValidPerson()
            };
        }

        [TestMethod]
        public void Constructor_ValidParameters_ShouldCreateAuction()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = 10.00m;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);

            // Assert
            Assert.IsNotNull(auction);
            Assert.AreEqual(seller, auction.Seller);
            Assert.AreEqual(product, auction.Product);
            Assert.AreEqual(startDate, auction.StartDate);
            Assert.AreEqual(endDate, auction.EndDate);
            Assert.AreEqual(startingPrice, auction.StartingPrice);
            Assert.AreEqual(currency, auction.Currency);
            Assert.IsFalse(auction.IsCompleted);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Start date cannot be in the past.")]
        public void Constructor_StartDateInPast_ShouldThrowArgumentException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(-1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = 10.00m;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "End date must be after the start date.")]
        public void Constructor_EndDateBeforeStartDate_ShouldThrowArgumentException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(-1);
            decimal startingPrice = 10.00m;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Starting price must be greater than zero.")]
        public void Constructor_ZeroStartingPrice_ShouldThrowArgumentException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = 0.00m;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullSeller_ShouldThrowArgumentNullException()
        {
            // Arrange
            Person seller = null;
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = 10.00m;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullProduct_ShouldThrowArgumentNullException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            Product product = null;
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = 10.00m;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
        }

        [TestMethod]
        public void AddBid_ValidBid_ShouldAddToBids()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = 10.00m;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
            var bid = this.CreateValidBid(auction);

            // Act
            auction.AddBid(bid);

            // Assert
            Assert.AreEqual(1, auction.Bids.Count);
            Assert.AreEqual(bid, auction.Bids[0]);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectFormat()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = 10.00m;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
            var expected = $"Auction for {product.Name} from {startDate} to {endDate} with starting price {startingPrice} {currency}";

            // Act
            var result = auction.ToString();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddBid_NullBid_ShouldThrowArgumentNullException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = 10.00m;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);

            // Act
            auction.AddBid(null);
        }

        [TestMethod]
        public void MarkAuctionAsCompleted_ShouldSetIsCompletedToTrue()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = 10.00m;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);

            // Act
            auction.IsCompleted = true;

            // Assert
            Assert.IsTrue(auction.IsCompleted);
        }

        [TestMethod]
        public void AddBid_AfterAuctionCompleted_ShouldNotAllowNewBids()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = 10.00m;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
            auction.IsCompleted = true;
            var bid = this.CreateValidBid(auction);

            // Act
            auction.AddBid(bid);

            // Assert
            Assert.AreEqual(0, auction.Bids.Count); // No bids should be added
        }

        [TestMethod]
        public void Constructor_StartingPriceBoundaryConditions_ShouldCreateAuction()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            string currency = "USD";

            // Act
            var auctionMinPrice = new Auction(seller, product, startDate, endDate, 0.01m, currency);
            var auctionMaxPrice = new Auction(seller, product, startDate, endDate, decimal.MaxValue, currency);

            // Assert
            Assert.IsNotNull(auctionMinPrice);
            Assert.IsNotNull(auctionMaxPrice);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidCurrency_ShouldThrowArgumentException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = 10.00m;
            string invalidCurrency = "US";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, invalidCurrency);
        }

        [TestMethod]
        public void Constructor_StartDateSameAsCurrentDate_ShouldCreateAuction()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now; // Start date is today
            var endDate = startDate.AddDays(1); // End date is tomorrow
            decimal startingPrice = 10.00m;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);

            // Assert
            Assert.IsNotNull(auction);
            Assert.AreEqual(startDate, auction.StartDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EndDateSameAsStartDate_ShouldThrowArgumentException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate; // End date is the same as start date
            decimal startingPrice = 10.00m;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
        }

        [TestMethod]
        public void Constructor_LargeStartingPrice_ShouldCreateAuction()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = decimal.MaxValue; // Very large price
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);

            // Assert
            Assert.IsNotNull(auction);
            Assert.AreEqual(startingPrice, auction.StartingPrice);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectStringRepresentation()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = 10.00m;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);

            // Act
            var result = auction.ToString();

            // Assert
            var expected = $"Auction for {product.Name} from {startDate} to {endDate} with starting price {startingPrice} {currency}";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AddBid_ShouldAddBidToBidsList()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            decimal startingPrice = 10.00m;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
            var bid = this.CreateValidBid(auction);

            // Act
            auction.AddBid(bid);

            // Assert
            Assert.AreEqual(1, auction.Bids.Count);
            Assert.AreEqual(bid, auction.Bids[0]);
        }
    }
}