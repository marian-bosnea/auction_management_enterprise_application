// <copyright file="BidDAOTest.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper.Tests
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using DataMapper.DAO;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the <see cref="BidDAO"/> class.
    /// </summary>
    [TestClass]
    public class BidDAOTest
    {
        /// <summary>
        /// Represents the database context used for accessing the database.
        /// Provides methods for querying and saving data to the database.
        /// </summary>
        private DbContext context;

        /// <summary>
        /// Represents the data access object (DAO) for managing bids.
        /// Provides methods for interacting with bid-related data in the database.
        /// </summary>
        private BidDAO bidDAO;

        /// <summary>
        /// Represents the data access object (DAO) for managing persons.
        /// Provides methods for interacting with person-related data in the database.
        /// </summary>
        private PersonDAO personDAO;

        /// <summary>
        /// Initializes the test environment before each test method is run.
        /// This includes setting up an in-memory database and initializing DAOs.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Create an in-memory database using Effort
            var connection = Effort.DbConnectionFactory.CreateTransient();
            this.context = new AuctionManagementEfCoreDbContext(connection);

            // Initialize DAO with the in-memory context
            this.bidDAO = new BidDAO((AuctionManagementEfCoreDbContext)this.context);
            this.personDAO = new PersonDAO((AuctionManagementEfCoreDbContext)this.context);

            // Seed initial data
            this.SeedDatabase();
        }

        /// <summary>
        /// Tests that the <see cref="BidDAO.Add"/> method correctly adds a new bid to the database.
        /// </summary>
        [TestMethod]
        public void AddBid_ShouldAddBidToDatabase()
        {
            // Arrange
            var person = this.context.Set<Person>().First();
            var newBid = new Bid(20.00, "USD") { Bidder = person, BidTime = DateTime.Now };

            // Act
            this.bidDAO.Add(newBid);

            // Assert
            var addedBid = this.context.Set<Bid>().Find(newBid.Id);
            Assert.IsNotNull(addedBid);
            Assert.AreEqual(newBid.Amount, addedBid.Amount);
            Assert.AreEqual(newBid.Currency, addedBid.Currency);
            Assert.AreEqual(newBid.BidTime, addedBid.BidTime);
            Assert.AreEqual(newBid.Bidder.Id, addedBid.Bidder.Id);
        }

        /// <summary>
        /// Tests that the <see cref="BidDAO.Add"/> method throws an <see cref="ArgumentException"/> when adding a bid with an invalid amount.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddBid_ShouldThrowExceptionForInvalidAmount()
        {
            // Arrange
            var person = this.context.Set<Person>().First();
            var invalidBid = new Bid(0.00, "USD") { Bidder = person, BidTime = DateTime.Now }; // Invalid amount

            // Act
            this.bidDAO.Add(invalidBid); // Should throw exception
        }

        /// <summary>
        /// Tests that the <see cref="BidDAO.Add"/> method throws a <see cref="DbEntityValidationException"/> when adding a bid with an invalid currency.
        /// </summary>
        [TestMethod]
        public void AddBid_ShouldThrowExceptionForInvalidCurrency()
        {
            // Arrange
            var person = this.context.Set<Person>().First();
            var invalidBid = new Bid(20.00, "US") { Bidder = person, BidTime = DateTime.Now }; // Invalid currency length

            try
            {
                // Act
                this.bidDAO.Add(invalidBid); // Should throw exception
                Assert.Fail("Expected DbEntityValidationException was not thrown.");
            }
            catch (DbEntityValidationException ex)
            {
                // Assert
                var validationErrors = ex.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                Assert.IsTrue(validationErrors.Any(e => e.Contains("Currency must be a valid 3-letter ISO code.")));
            }
        }

        /// <summary>
        /// Tests that the <see cref="BidDAO.Update"/> method correctly updates an existing bid in the database.
        /// </summary>
        [TestMethod]
        public void UpdateBid_ShouldUpdateBidInDatabase()
        {
            // Arrange
            var person = this.context.Set<Person>().First();
            var bidToAdd = new Bid(10.00, "USD") { Bidder = person, BidTime = DateTime.Now };
            this.bidDAO.Add(bidToAdd);

            // Modify the bid
            bidToAdd.Amount = 25.00;
            bidToAdd.Currency = "EUR";
            bidToAdd.BidTime = DateTime.Now.AddMinutes(-5); // Ensure bid time is valid

            // Act
            this.bidDAO.Update(bidToAdd);

            // Assert
            var updatedBid = this.context.Set<Bid>().Find(bidToAdd.Id);
            Assert.IsNotNull(updatedBid);
            Assert.AreEqual(25.00, updatedBid.Amount);
            Assert.AreEqual("EUR", updatedBid.Currency);
            Assert.AreEqual(bidToAdd.BidTime, updatedBid.BidTime);
            Assert.AreEqual(bidToAdd.Bidder.Id, updatedBid.Bidder.Id);
        }

        /// <summary>
        /// Tests that the <see cref="BidDAO.Delete"/> method correctly removes a bid from the database.
        /// </summary>
        [TestMethod]
        public void DeleteBid_ShouldRemoveBidFromDatabase()
        {
            // Arrange
            var person = this.context.Set<Person>().First();
            var bidToDelete = new Bid(10.00, "USD") { Bidder = person, BidTime = DateTime.Now };
            this.bidDAO.Add(bidToDelete);

            // Act
            this.bidDAO.Delete(bidToDelete.Id);

            // Assert
            var deletedBid = this.context.Set<Bid>().Find(bidToDelete.Id);
            Assert.IsNull(deletedBid);
        }

        /// <summary>
        /// Tests that the <see cref="BidDAO.Get"/> method correctly retrieves a bid from the database.
        /// </summary>
        [TestMethod]
        public void GetBid_ShouldReturnCorrectBid()
        {
            // Arrange
            var person = this.context.Set<Person>().First();
            var expectedBid = new Bid(10.00, "USD") { Bidder = person, BidTime = DateTime.Now };
            this.bidDAO.Add(expectedBid);

            // Act
            var bid = this.bidDAO.Get(expectedBid.Id);

            // Assert
            Assert.IsNotNull(bid);
            Assert.AreEqual(expectedBid.Id, bid.Id);
            Assert.AreEqual(expectedBid.Amount, bid.Amount);
            Assert.AreEqual(expectedBid.Currency, bid.Currency);
            Assert.AreEqual(expectedBid.BidTime, bid.BidTime);
            Assert.AreEqual(expectedBid.Bidder.Id, bid.Bidder.Id);
        }

        /// <summary>
        /// Tests that the <see cref="BidDAO.GetAll"/> method correctly retrieves all bids from the database.
        /// </summary>
        [TestMethod]
        public void GetAllBids_ShouldReturnAllBids()
        {
            // Arrange
            var person = this.context.Set<Person>().First();
            this.bidDAO.Add(new Bid(10.00, "USD") { Bidder = person, BidTime = DateTime.Now });
            this.bidDAO.Add(new Bid(15.00, "USD") { Bidder = person, BidTime = DateTime.Now.AddMinutes(-10) });

            // Act
            var bids = this.bidDAO.GetAll();

            // Assert
            Assert.AreEqual(2, bids.Count);
        }

        /// <summary>
        /// Seeds the in-memory database with initial data required for testing.
        /// </summary>
        private void SeedDatabase()
        {
            var initialPerson = new Person("John Doe") { Role = PersonRole.Seller };
            this.personDAO.Add(initialPerson);
        }
    }
}
