// <copyright file="AuctionTest.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel.Tests
{
    using System;
    using System.Collections.Generic;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the <see cref="Auction"/> class.
    /// </summary>
    [TestClass]
    public class AuctionTest
    {
        /// <summary>
        /// Tests that the <see cref="Auction"/> constructor correctly initializes an auction with valid parameters.
        /// </summary>
        [TestMethod]
        public void Constructor_ValidParameters_ShouldCreateAuction()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
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

        /// <summary>
        /// Tests that the <see cref="Auction"/> constructor throws an <see cref="ArgumentException"/> when the start date is in the past.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Start date cannot be in the past.")]
        public void Constructor_StartDateInPast_ShouldThrowArgumentException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(-1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
        }

        /// <summary>
        /// Tests that the <see cref="Auction"/> constructor throws an <see cref="ArgumentException"/> when the end date is before the start date.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "End date must be after the start date.")]
        public void Constructor_EndDateBeforeStartDate_ShouldThrowArgumentException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(-1);
            double startingPrice = 10.00;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
        }

        /// <summary>
        /// Tests that the <see cref="Auction"/> constructor throws an <see cref="ArgumentException"/> when the starting price is zero.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Starting price must be greater than zero.")]
        public void Constructor_ZeroStartingPrice_ShouldThrowArgumentException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 0.00;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
        }

        /// <summary>
        /// Tests that the <see cref="Auction"/> constructor throws an <see cref="ArgumentNullException"/> when the seller is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullSeller_ShouldThrowArgumentNullException()
        {
            // Arrange
            Person seller = null;
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
        }

        /// <summary>
        /// Tests that the <see cref="Auction"/> constructor throws an <see cref="ArgumentNullException"/> when the product is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullProduct_ShouldThrowArgumentNullException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            Product product = null;
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
        }

        /// <summary>
        /// Tests that a valid <see cref="Bid"/> can be added to the <see cref="Auction"/>'s list of bids.
        /// </summary>
        [TestMethod]
        public void AddBid_ValidBid_ShouldAddToBids()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
            var bid = this.CreateValidBid(auction);

            // Act
            auction.AddBid(bid);

            // Assert
            Assert.AreEqual(1, auction.Bids.Count);
            Assert.AreEqual(bid, auction.Bids[0]);
        }

        /// <summary>
        /// Tests that the <see cref="Auction.ToString"/> method returns the correct string representation of the auction.
        /// </summary>
        [TestMethod]
        public void ToString_ShouldReturnCorrectFormat()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
            var expected = $"Auction for {product.Name} from {startDate} to {endDate} with starting price {startingPrice} {currency}";

            // Act
            var result = auction.ToString();

            // Assert
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests that the <see cref="Auction.AddBid"/> method throws an <see cref="ArgumentNullException"/> when the bid is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddBid_NullBid_ShouldThrowArgumentNullException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);

            // Act
            auction.AddBid(null);
        }

        /// <summary>
        /// Tests that setting the <see cref="Auction.IsCompleted"/> property to true correctly updates the auction status.
        /// </summary>
        [TestMethod]
        public void MarkAuctionAsCompleted_ShouldSetIsCompletedToTrue()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);

            // Act
            auction.IsCompleted = true;

            // Assert
            Assert.IsTrue(auction.IsCompleted);
        }

        /// <summary>
        /// Tests that no new bids can be added to an auction that has been marked as completed.
        /// </summary>
        [TestMethod]
        public void AddBid_AfterAuctionCompleted_ShouldNotAllowNewBids()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
            auction.IsCompleted = true;
            var bid = this.CreateValidBid(auction);

            // Act
            auction.AddBid(bid);

            // Assert
            Assert.AreEqual(0, auction.Bids.Count); // No bids should be added
        }

        /// <summary>
        /// Tests that an auction can be created with boundary conditions for the starting price (minimum and maximum values).
        /// </summary>
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
            var auctionMinPrice = new Auction(seller, product, startDate, endDate, 0.01, currency);
            var auctionMaxPrice = new Auction(seller, product, startDate, endDate, double.MaxValue, currency);

            // Assert
            Assert.IsNotNull(auctionMinPrice);
            Assert.IsNotNull(auctionMaxPrice);
        }

        /// <summary>
        /// Tests that the <see cref="Auction"/> constructor throws an <see cref="ArgumentException"/> when an invalid currency is provided.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidCurrency_ShouldThrowArgumentException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
            string invalidCurrency = "US";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, invalidCurrency);
        }

        /// <summary>
        /// Tests that an auction can be created with the start date set to the current date.
        /// </summary>
        [TestMethod]
        public void Constructor_StartDateSameAsCurrentDate_ShouldCreateAuction()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(2);
            double startingPrice = 10.00;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);

            // Assert
            Assert.IsNotNull(auction);
            Assert.AreEqual(startDate, auction.StartDate);
        }

        /// <summary>
        /// Tests that the <see cref="Auction"/> constructor throws an <see cref="ArgumentException"/> when the end date is the same as the start date.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EndDateSameAsStartDate_ShouldThrowArgumentException()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate; // End date is the same as start date
            double startingPrice = 10.00;
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
        }

        /// <summary>
        /// Tests that an auction can be created with a very large starting price.
        /// </summary>
        [TestMethod]
        public void Constructor_LargeStartingPrice_ShouldCreateAuction()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = double.MaxValue; // Very large price
            string currency = "USD";

            // Act
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);

            // Assert
            Assert.IsNotNull(auction);
            Assert.AreEqual(startingPrice, auction.StartingPrice);
        }

        /// <summary>
        /// Tests that the <see cref="Auction.ToString"/> method returns the correct string representation of the auction.
        /// </summary>
        [TestMethod]
        public void ToString_ShouldReturnCorrectStringRepresentation()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);

            // Act
            var result = auction.ToString();

            // Assert
            var expected = $"Auction for {product.Name} from {startDate} to {endDate} with starting price {startingPrice} {currency}";
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests that a valid <see cref="Bid"/> is added to the <see cref="Auction"/>'s bids list.
        /// </summary>
        [TestMethod]
        public void AddBid_ShouldAddBidToBidsList()
        {
            // Arrange
            var seller = this.CreateValidPerson();
            var product = this.CreateValidProduct();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
            string currency = "USD";
            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency);
            var bid = this.CreateValidBid(auction);

            // Act
            auction.AddBid(bid);

            // Assert
            Assert.AreEqual(1, auction.Bids.Count);
            Assert.AreEqual(bid, auction.Bids[0]);
        }

        /// <summary>
        /// Tests that setting a valid start date works as expected.
        /// </summary>
        [TestMethod]
        public void StartDate_SetValidDate_UpdatesStartDate()
        {
            // Arrange
            var person = new Person("John Doe");
            var product = new Product(1, "Product A", "Description", new List<Category>());
            var endDate = DateTime.Now.AddDays(1);
            var auction = new Auction(person, product, DateTime.Now, endDate, 100, "USD");

            var newStartDate = DateTime.Now.AddHours(-1); // 1 hour ago

            // Act
            auction.StartDate = newStartDate;

            // Assert
            Assert.AreEqual(newStartDate, auction.StartDate);
        }

        /// <summary>
        /// Tests that setting a start date that is later than the end date throws an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartDate_SetDateLaterThanEndDate_ThrowsArgumentException()
        {
            // Arrange
            var person = new Person("John Doe");
            var product = new Product(1, "Product A", "Description", new List<Category>());
            var startDate = DateTime.Now;
            var endDate = startDate.AddDays(1);
            var auction = new Auction(person, product, startDate, endDate, 100, "USD");

            // Act
            auction.StartDate = endDate.AddDays(1); // Setting start date to be later than end date
        }

        /// <summary>
        /// Tests that setting a valid end date works as expected.
        /// </summary>
        [TestMethod]
        public void EndDate_SetValidDate_UpdatesEndDate()
        {
            // Arrange
            var person = new Person("John Doe");
            var product = new Product(1, "Product A", "Description", new List<Category>());
            var startDate = DateTime.Now.AddDays(1);
            var auction = new Auction(person, product, startDate, DateTime.Now.AddDays(2), 100, "USD");

            var newEndDate = DateTime.Now.AddDays(3); // 1 day later

            // Act
            auction.EndDate = newEndDate;

            // Assert
            Assert.AreEqual(newEndDate, auction.EndDate);
        }

        /// <summary>
        /// Tests that setting an end date that is earlier than the current date throws an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EndDate_SetDateEarlierThanNow_ThrowsArgumentException()
        {
            // Arrange
            var person = new Person("John Doe");
            var product = new Product(1, "Product A", "Description", new List<Category>());
            var startDate = DateTime.Now.AddDays(1);
            var auction = new Auction(person, product, startDate, DateTime.Now.AddDays(2), 100, "USD");

            // Act
            auction.EndDate = DateTime.Now.AddDays(-1); // Setting end date to the past
        }

        /// <summary>
        /// Tests that setting an end date that is earlier than the start date throws an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EndDate_SetDateEarlierThanStartDate_ThrowsArgumentException()
        {
            // Arrange
            var person = new Person("John Doe");
            var product = new Product(1, "Product A", "Description", new List<Category>());
            var startDate = DateTime.Now.AddDays(1);
            var auction = new Auction(person, product, startDate, DateTime.Now.AddDays(2), 100, "USD");

            // Act
            auction.EndDate = startDate.AddHours(-1); // Setting end date to before start date
        }

        /// <summary>
        /// Tests that setting an end date to null throws an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EndDate_SetNullDate_ThrowsArgumentNullException()
        {
            // Arrange
            var person = new Person("John Doe");
            var product = new Product(1, "Product A", "Description", new List<Category>());
            var startDate = DateTime.Now.AddDays(1);
            var auction = new Auction(person, product, startDate, DateTime.Now.AddDays(2), 100, "USD");

            // Act
            auction.EndDate = default(DateTime); // Setting end date to null
        }

        /// <summary>
        /// Creates a valid <see cref="Person"/> instance with a default role of Seller.
        /// </summary>
        /// <returns>A valid <see cref="Person"/> instance.</returns>
        private Person CreateValidPerson()
        {
            return new Person("John Doe") { Role = PersonRole.Seller };
        }

        /// <summary>
        /// Creates a valid <see cref="Product"/> instance with default values.
        /// </summary>
        /// <returns>A valid <see cref="Product"/> instance.</returns>
        private Product CreateValidProduct()
        {
            return new Product(1, "Sample Product", "This is a sample product.", new List<Category>());
        }

        /// <summary>
        /// Creates a valid <see cref="Bid"/> instance for a given <see cref="Auction"/>.
        /// </summary>
        /// <param name="auction">The auction to associate with the bid.</param>
        /// <returns>A valid <see cref="Bid"/> instance.</returns>
        private Bid CreateValidBid(Auction auction)
        {
            return new Bid(15.00, "USD")
            {
                Bidder = this.CreateValidPerson(),
            };
        }
    }
}
