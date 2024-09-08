using Effort;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataMapper.DAO;
using DomainModel;
using System.Data.Entity.Validation;

namespace DataMapper.Tests
{
    [TestClass]
    public class BidDAOTests
    {
        private DbContext _context;
        private BidDAO _bidDAO;
        private PersonDAO _personDAO;

        [TestInitialize]
        public void Setup()
        {
            // Create an in-memory database using Effort
            var connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new AuctionManagementEfCoreDbContext(connection);

            // Initialize DAO with the in-memory context
            _bidDAO = new BidDAO((AuctionManagementEfCoreDbContext)_context);
            _personDAO = new PersonDAO((AuctionManagementEfCoreDbContext)_context);

            // Seed initial data
            SeedDatabase();
        }

        [TestMethod]
        public void AddBid_ShouldAddBidToDatabase()
        {
            // Arrange
            var person = _context.Set<Person>().First();
            var newBid = new Bid(20.00, "USD") { Bidder = person, BidTime = DateTime.Now };

            // Act
            _bidDAO.Add(newBid);

            // Assert
            var addedBid = _context.Set<Bid>().Find(newBid.Id);
            Assert.IsNotNull(addedBid);
            Assert.AreEqual(newBid.Amount, addedBid.Amount);
            Assert.AreEqual(newBid.Currency, addedBid.Currency);
            Assert.AreEqual(newBid.BidTime, addedBid.BidTime);
            Assert.AreEqual(newBid.Bidder.Id, addedBid.Bidder.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddBid_ShouldThrowExceptionForInvalidAmount()
        {
            // Arrange
            var person = _context.Set<Person>().First();
            var invalidBid = new Bid(0.00, "USD") { Bidder = person, BidTime = DateTime.Now }; // Invalid amount

            // Act
            _bidDAO.Add(invalidBid); // Should throw exception
        }

        [TestMethod]
        public void AddBid_ShouldThrowExceptionForInvalidCurrency()
        {
            // Arrange
            var person = _context.Set<Person>().First();
            var invalidBid = new Bid(20.00, "US") { Bidder = person, BidTime = DateTime.Now }; // Invalid currency length

            try
            {
                // Act
                _bidDAO.Add(invalidBid); // Should throw exception
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


        [TestMethod]
        public void UpdateBid_ShouldUpdateBidInDatabase()
        {
            // Arrange
            var person = _context.Set<Person>().First();
            var bidToAdd = new Bid(10.00, "USD") { Bidder = person, BidTime = DateTime.Now };
            _bidDAO.Add(bidToAdd);

            // Modify the bid
            bidToAdd.Amount = 25.00;
            bidToAdd.Currency = "EUR";
            bidToAdd.BidTime = DateTime.Now.AddMinutes(-5); // Ensure bid time is valid

            // Act
            _bidDAO.Update(bidToAdd);

            // Assert
            var updatedBid = _context.Set<Bid>().Find(bidToAdd.Id);
            Assert.IsNotNull(updatedBid);
            Assert.AreEqual(25.00, updatedBid.Amount);
            Assert.AreEqual("EUR", updatedBid.Currency);
            Assert.AreEqual(bidToAdd.BidTime, updatedBid.BidTime);
            Assert.AreEqual(bidToAdd.Bidder.Id, updatedBid.Bidder.Id);
        }

        [TestMethod]
        public void DeleteBid_ShouldRemoveBidFromDatabase()
        {
            // Arrange
            var person = _context.Set<Person>().First();
            var bidToDelete = new Bid(10.00, "USD") { Bidder = person, BidTime = DateTime.Now };
            _bidDAO.Add(bidToDelete);

            // Act
            _bidDAO.Delete(bidToDelete.Id);

            // Assert
            var deletedBid = _context.Set<Bid>().Find(bidToDelete.Id);
            Assert.IsNull(deletedBid);
        }

        [TestMethod]
        public void GetBid_ShouldReturnCorrectBid()
        {
            // Arrange
            var person = _context.Set<Person>().First();
            var expectedBid = new Bid(10.00, "USD") { Bidder = person, BidTime = DateTime.Now };
            _bidDAO.Add(expectedBid);

            // Act
            var bid = _bidDAO.Get(expectedBid.Id);

            // Assert
            Assert.IsNotNull(bid);
            Assert.AreEqual(expectedBid.Id, bid.Id);
            Assert.AreEqual(expectedBid.Amount, bid.Amount);
            Assert.AreEqual(expectedBid.Currency, bid.Currency);
            Assert.AreEqual(expectedBid.BidTime, bid.BidTime);
            Assert.AreEqual(expectedBid.Bidder.Id, bid.Bidder.Id);
        }

        [TestMethod]
        public void GetAllBids_ShouldReturnAllBids()
        {
            // Arrange
            var person = _context.Set<Person>().First();
            _bidDAO.Add(new Bid(10.00, "USD") { Bidder = person, BidTime = DateTime.Now });
            _bidDAO.Add(new Bid(15.00, "USD") { Bidder = person, BidTime = DateTime.Now.AddMinutes(-10) });

            // Act
            var bids = _bidDAO.GetAll();

            // Assert
            Assert.AreEqual(2, bids.Count);
        }

        private void SeedDatabase()
        {
            var initialPerson = new Person("John Doe") { Role = PersonRole.Seller };
            _personDAO.Add(initialPerson);
        }
    }
}
