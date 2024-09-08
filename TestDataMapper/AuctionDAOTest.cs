namespace DataMapper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using DataMapper.DAO;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the <see cref="AuctionDAO"/> class.
    /// </summary>
    [TestClass]
    public class AuctionDAOTest
    {
        /// <summary>
        /// Represents the database context used for accessing the database.
        /// </summary>
        private DbContext context;

        /// <summary>
        /// Represents the data access object (DAO) for managing auctions.
        /// Provides methods for interacting with auction-related data in the database.
        /// </summary>
        private AuctionDAO auctionDAO;

        /// <summary>
        /// Represents the data access object (DAO) for managing people.
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
            this.auctionDAO = new AuctionDAO((AuctionManagementEfCoreDbContext)this.context);
            this.personDAO = new PersonDAO((AuctionManagementEfCoreDbContext)this.context);

            // Seed initial data
            this.SeedDatabase();
        }

        /// <summary>
        /// Tests that the <see cref="AuctionDAO.Add"/> method correctly adds a new auction to the database.
        /// </summary>
        [TestMethod]
        public void AddAuction_ShouldAddAuctionToDatabase()
        {
            // Arrange
            var person = this.context.Set<Person>().First();
            var newAuction = this.CreateValidAuction(3, person);

            // Act
            this.auctionDAO.Add(newAuction);

            // Assert
            var addedAuction = this.context.Set<Auction>().Find(3);
            Assert.IsNotNull(addedAuction);
            Assert.AreEqual(3, addedAuction.Id);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionDAO.Update"/> method correctly updates an existing auction in the database.
        /// </summary>
        [TestMethod]
        public void UpdateAuction_ShouldUpdateAuctionInDatabase()
        {
            // Arrange
            var person = this.context.Set<Person>().First();
            var auctionToAdd = this.CreateValidAuction(1, person);
            this.auctionDAO.Add(auctionToAdd);

            // Modify the auction and make sure it has the same ID
            var auctionToUpdate = this.context.Set<Auction>().Find(1);
            auctionToUpdate.StartingPrice = 15.00;

            // Act
            this.auctionDAO.Update(auctionToUpdate);

            // Assert
            var updatedAuction = this.context.Set<Auction>().Find(1);
            Assert.IsNotNull(updatedAuction);
            Assert.AreEqual(15.00, updatedAuction.StartingPrice);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionDAO.Delete"/> method correctly removes an auction from the database.
        /// </summary>
        [TestMethod]
        public void DeleteAuction_ShouldRemoveAuctionFromDatabase()
        {
            // Arrange
            var person = this.context.Set<Person>().First();
            var auctionToDelete = this.CreateValidAuction(1, person);
            this.auctionDAO.Add(auctionToDelete);

            // Act
            this.auctionDAO.Delete(1);

            // Assert
            var deletedAuction = this.context.Set<Auction>().Find(1);
            Assert.IsNull(deletedAuction);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionDAO.Get"/> method correctly retrieves an auction from the database.
        /// </summary>
        [TestMethod]
        public void GetAuction_ShouldReturnCorrectAuction()
        {
            // Arrange
            var person = this.context.Set<Person>().First();
            var expectedAuction = this.CreateValidAuction(1, person);
            this.auctionDAO.Add(expectedAuction);

            // Act
            var auction = this.auctionDAO.Get(1);

            // Assert
            Assert.IsNotNull(auction);
            Assert.AreEqual(1, auction.Id);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionDAO.GetActiveAuctionsForPerson"/> method correctly retrieves active auctions for a given person.
        /// </summary>
        [TestMethod]
        public void GetActiveAuctionsForPerson_ShouldReturnActiveAuctions()
        {
            // Arrange
            var person = this.context.Set<Person>().First();
            var activeAuction = this.CreateValidAuction(1, person);
            activeAuction.EndDate = DateTime.Now.AddDays(1); // Ensure it is active
            this.auctionDAO.Add(activeAuction);

            // Act
            var activeAuctions = this.auctionDAO.GetActiveAuctionsForPerson(person);

            // Assert
            Assert.AreEqual(1, activeAuctions.Count);
            Assert.AreEqual(1, activeAuctions[0].Id);
        }

        /// <summary>
        /// Seeds the in-memory database with initial data required for testing.
        /// </summary>
        private void SeedDatabase()
        {
            var initialPerson = new Person("John Doe") { Role = PersonRole.Seller };
            this.personDAO.Add(initialPerson);

            var initialAuctions = new List<Auction>
            {
                this.CreateValidAuction(1, initialPerson),
                this.CreateValidAuction(2, initialPerson),
            };

            foreach (var auction in initialAuctions)
            {
                this.auctionDAO.Add(auction);
            }
        }

        /// <summary>
        /// Creates a valid auction object with the specified ID and seller.
        /// </summary>
        /// <param name="id">The ID of the auction.</param>
        /// <param name="seller">The seller of the auction.</param>
        /// <returns>A new <see cref="Auction"/> object.</returns>
        private Auction CreateValidAuction(int id, Person seller)
        {
            var product = new Product(1, "Sample Product", "Description", new List<Category>());
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(1);
            double startingPrice = 10.00;
            string currency = "USD";

            var auction = new Auction(seller, product, startDate, endDate, startingPrice, currency)
            {
                Id = id,
            };

            return auction;
        }
    }
}
