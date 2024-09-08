using Effort;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataMapper.DAO;
using DomainModel;

namespace DataMapper.Tests
{
    [TestClass]
    public class AuctionDAOTests
    {
        private DbContext _context;
        private AuctionDAO _auctionDAO;
        private PersonDAO _personDAO;

        [TestInitialize]
        public void Setup()
        {
            // Create an in-memory database using Effort
            var connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new AuctionManagementEfCoreDbContext(connection);

            // Initialize DAO with the in-memory context
            _auctionDAO = new AuctionDAO((AuctionManagementEfCoreDbContext)_context);
            _personDAO = new PersonDAO((AuctionManagementEfCoreDbContext)_context);

            // Seed initial data
            SeedDatabase();
        }

        [TestMethod]
        public void AddAuction_ShouldAddAuctionToDatabase()
        {
            // Arrange
            var person = _context.Set<Person>().First();
            var newAuction = CreateValidAuction(3, person);

            // Act
            _auctionDAO.Add(newAuction);

            // Assert
            var addedAuction = _context.Set<Auction>().Find(3);
            Assert.IsNotNull(addedAuction);
            Assert.AreEqual(3, addedAuction.Id);
        }

        [TestMethod]
        public void UpdateAuction_ShouldUpdateAuctionInDatabase()
        {
            // Arrange
            var person = _context.Set<Person>().First();
            var auctionToAdd = CreateValidAuction(1, person);
            _auctionDAO.Add(auctionToAdd);

            // Modify the auction and make sure it has the same ID
            var auctionToUpdate = _context.Set<Auction>().Find(1);
            auctionToUpdate.StartingPrice = 15.00;

            // Act
            _auctionDAO.Update(auctionToUpdate);

            // Assert
            var updatedAuction = _context.Set<Auction>().Find(1);
            Assert.IsNotNull(updatedAuction);
            Assert.AreEqual(15.00, updatedAuction.StartingPrice);
        }

        [TestMethod]
        public void DeleteAuction_ShouldRemoveAuctionFromDatabase()
        {
            // Arrange
            var person = _context.Set<Person>().First();
            var auctionToDelete = CreateValidAuction(1, person);
            _auctionDAO.Add(auctionToDelete);

            // Act
            _auctionDAO.Delete(1);

            // Assert
            var deletedAuction = _context.Set<Auction>().Find(1);
            Assert.IsNull(deletedAuction);
        }

        [TestMethod]
        public void GetAuction_ShouldReturnCorrectAuction()
        {
            // Arrange
            var person = _context.Set<Person>().First();
            var expectedAuction = CreateValidAuction(1, person);
            _auctionDAO.Add(expectedAuction);

            // Act
            var auction = _auctionDAO.Get(1);

            // Assert
            Assert.IsNotNull(auction);
            Assert.AreEqual(1, auction.Id);
        }

        [TestMethod]
        public void GetActiveAuctionsForPerson_ShouldReturnActiveAuctions()
        {
            // Arrange
            var person = _context.Set<Person>().First();
            var activeAuction = CreateValidAuction(1, person);
            activeAuction.EndDate = DateTime.Now.AddDays(1); // Ensure it is active
            _auctionDAO.Add(activeAuction);

            // Act
            var activeAuctions = _auctionDAO.GetActiveAuctionsForPerson(person);

            // Assert
            Assert.AreEqual(1, activeAuctions.Count);
            Assert.AreEqual(1, activeAuctions[0].Id);
        }

        private void SeedDatabase()
        {
            var initialPerson = new Person("John Doe") { Role = PersonRole.Seller };
            _personDAO.Add(initialPerson);

            var initialAuctions = new List<Auction>
            {
                CreateValidAuction(1, initialPerson),
                CreateValidAuction(2, initialPerson)
            };

            foreach (var auction in initialAuctions)
            {
                _auctionDAO.Add(auction);
            }
        }

        private Auction CreateValidAuction(int id, Person seller)
        {
            var product = new Product(1, "Sample Product", "Description", new List<Category>());
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
            string currency = "USD";

            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency)
            {
                Id = id
            };

            return auction;
        }
    }
}