using DataMapper.Interfaces;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Services.Tests
{
    [TestClass]
    public class AuctionServiceTests
    {
        private Mock<IAuctionDAO> auctionDAOMock;
        private Mock<IBidDAO> bidDAOMock;
        private AuctionService auctionService;

        [TestInitialize]
        public void Setup()
        {
            this.auctionDAOMock = new Mock<IAuctionDAO>();
            this.bidDAOMock = new Mock<IBidDAO>();
            this.auctionService = new AuctionService(this.auctionDAOMock.Object, this.bidDAOMock.Object);
        }

        [TestMethod]
        public void StartAuction_ValidAuction_StartsAuction()
        {
            // Arrange
            var person = new Person("John Doe") { Score = 7 };
            var product = new Product(1, "Product A", "Description", new List<Category>());
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(2);
            var startingPrice = 100.0;
            var currency = "USD";

            this.auctionDAOMock.Setup(a => a.GetActiveAuctionsForPerson(person)).Returns(new List<Auction>());
            this.auctionDAOMock.Setup(a => a.GetActiveAuctionsForPersonInCategory(person, It.IsAny<Category>())).Returns(new List<Auction>());

            // Act
            this.auctionService.StartAuction(person, product, startDate, endDate, startingPrice, currency);

            // Assert
            this.auctionDAOMock.Verify(a => a.Add(It.IsAny<Auction>()), Times.Once);
        }

        [TestMethod]
        public void FinalizeAuction_AuctionNotCompleted_FinalizesAuction()
        {
            // Arrange
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now, DateTime.Now.AddDays(1), 100, "USD")
            {
                IsCompleted = false
            };

            // Act
            this.auctionService.FinalizeAuction(new Person("John Doe"), auction);

            // Assert
            Assert.IsTrue(auction.IsCompleted);
            this.auctionDAOMock.Verify(a => a.Update(auction), Times.Once);
        }

        [TestMethod]
        public void FinalizeAuction_AuctionAlreadyCompleted_ThrowsException()
        {
            // Arrange
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now, DateTime.Now.AddDays(1), 100, "USD")
            {
                IsCompleted = true
            };

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                this.auctionService.FinalizeAuction(new Person("John Doe"), auction)
            );
        }

        [TestMethod]
        public void GetAuctionById_ValidId_ReturnsAuction()
        {
            // Arrange
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now, DateTime.Now.AddDays(1), 100, "USD");
            this.auctionDAOMock.Setup(a => a.Get(1)).Returns(auction);

            // Act
            var result = this.auctionService.GetAuctionById(1);

            // Assert
            Assert.AreEqual(auction, result);
        }

        [TestMethod]
        public void GetAllAuctions_ReturnsAllAuctions()
        {
            // Arrange
            var auctions = new List<Auction>
            {
                new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now, DateTime.Now.AddDays(1), 100, "USD"),
                new Auction(new Person("Jane Smith"), new Product(2, "Product B", "Description", new List<Category>()), DateTime.Now, DateTime.Now.AddDays(2), 200, "USD")
            };
            this.auctionDAOMock.Setup(a => a.GetAll()).Returns(auctions);

            // Act
            var result = this.auctionService.GetAllAuctions();

            // Assert
            CollectionAssert.AreEqual(auctions, result);
        }

        [TestMethod]
        public void UpdateAuction_ValidAuction_UpdatesAuction()
        {
            // Arrange
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now, DateTime.Now.AddDays(1), 100, "USD");
            this.auctionDAOMock.Setup(a => a.Update(auction)).Verifiable();

            // Act
            this.auctionService.UpdateAuction(auction);

            // Assert
            this.auctionDAOMock.Verify(a => a.Update(auction), Times.Once);
        }

        [TestMethod]
        public void AddBid_CurrencyMismatch_ThrowsArgumentException()
        {
            // Arrange
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 100, "USD");
            var bid = new Bid(110, "EUR");

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => this.auctionService.AddBid(auction, bid));
        }

        [TestMethod]
        public void AddBid_AmountTooLow_ThrowsArgumentException()
        {
            // Arrange
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 100, "USD");
            var bid = new Bid(105, "USD");

            auction.Bids.Add(new Bid(110, "USD")); // Existing bid is higher

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => this.auctionService.AddBid(auction, bid));
        }

        [TestMethod]
        public void AddBid_NoPreviousBids_AddsBid()
        {
            // Arrange
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 100, "USD");
            var bid = new Bid(105, "USD");

            // Act
            this.auctionService.AddBid(auction, bid);

            // Assert
            this.auctionDAOMock.Verify(a => a.Update(auction), Times.Once);
            this.bidDAOMock.Verify(b => b.Add(bid), Times.Once);
        }

        [TestMethod]
        public void AddAuction_ShouldCallAddOnAuctionDAO()
        {
            // Arrange
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD");

            // Act
            this.auctionService.AddAuction(auction);

            // Assert
            this.auctionDAOMock.Verify(dao => dao.Add(auction), Times.Once);
        }

        [TestMethod]
        public void AddAuction_ShouldThrowException_WhenDAOAddFails()
        {
            // Arrange
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD");
            this.auctionDAOMock.Setup(dao => dao.Add(auction)).Throws(new Exception("DAO error"));

            // Act & Assert
            var ex = Assert.ThrowsException<Exception>(() => this.auctionService.AddAuction(auction));
            Assert.AreEqual("DAO error", ex.Message);
        }

        [TestMethod]
        public void DeleteAuction_ShouldCallDeleteOnAuctionDAO_WhenAuctionExists()
        {
            // Arrange
            int auctionId = 1;
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product", "Description", new List<Category>()), DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD");
            this.auctionDAOMock.Setup(dao => dao.Get(auctionId)).Returns(auction);

            // Act
            this.auctionService.DeleteAuction(auctionId);

            // Assert
            this.auctionDAOMock.Verify(dao => dao.Delete(auctionId), Times.Once);
        }

        [TestMethod]
        public void DeleteAuction_ShouldNotCallDeleteOnAuctionDAO_WhenAuctionDoesNotExist()
        {
            // Arrange
            int auctionId = 1;
            this.auctionDAOMock.Setup(dao => dao.Get(auctionId)).Returns((Auction)null);

            // Act
            this.auctionService.DeleteAuction(auctionId);

            // Assert
            this.auctionDAOMock.Verify(dao => dao.Delete(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void StartAuction_ShouldNotThrowException_WhenNoCategoryExceedsMaxAuctionsPerCategory()
        {
            // Arrange
            var person = new Person("John Doe");
            var product = new Product(1, "Product", "Description", new List<Category>
    {
        new Category("Category1"),
        new Category("Category2"),
    });

            // Mocking the DAO methods
            this.auctionDAOMock.Setup(dao => dao.GetActiveAuctionsForPerson(person)).Returns(new List<Auction>());
            this.auctionDAOMock.Setup(dao => dao.GetActiveAuctionsForPersonInCategory(person, It.IsAny<Category>())).Returns(new List<Auction>());

            // Act
            try
            {
                this.auctionService.StartAuction(person, product, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception was thrown: {ex.Message}");
            }
        }

        [TestMethod]
        public void StartAuction_ShouldThrowException_WhenCategoryExceedsMaxAuctionsPerCategory()
        {
            // Arrange
            var person = new Person("John Doe");
            var category = new Category("Category1");
            var product = new Product(1, "Product", "Description", new List<Category> { category });
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 100, "USD");
        
            this.auctionDAOMock.Setup(dao => dao.GetActiveAuctionsForPerson(person)).Returns(new List<Auction>());
            this.auctionDAOMock.Setup(dao => dao.GetActiveAuctionsForPersonInCategory(person, category))
                          .Returns(new List<Auction> { auction });

            // Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                this.auctionService.StartAuction(person, product, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD"));

            Assert.AreEqual($"Cannot start a new auction. Maximum of {this.auctionService.MaxActiveAuctionsPerCategory} active auctions in category '{category.Name}' reached.", ex.Message);
        }

        [TestMethod]
        public void StartAuction_ShouldThrowException_WhenMultipleCategoriesExceedMaxAuctionsPerCategory()
        {
            // Arrange
            var person = new Person("John Doe");
            var category1 = new Category("Category1");
            var category2 = new Category("Category2");
            var product = new Product(1, "Product", "Description", new List<Category> { category1, category2 });
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 100, "USD");

            // Mocking the DAO methods
            this.auctionDAOMock.Setup(dao => dao.GetActiveAuctionsForPerson(person)).Returns(new List<Auction>());
            this.auctionDAOMock.Setup(dao => dao.GetActiveAuctionsForPersonInCategory(person, category1))
                          .Returns(new List<Auction> { auction});
            this.auctionDAOMock.Setup(dao => dao.GetActiveAuctionsForPersonInCategory(person, category2))
                          .Returns(new List<Auction> { auction });

            // Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                this.auctionService.StartAuction(person, product, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD"));

            Assert.IsTrue(ex.Message.Contains("Maximum of"));
            Assert.IsTrue(ex.Message.Contains("active auctions in category 'Category1' reached") ||
                          ex.Message.Contains("active auctions in category 'Category2' reached"));
        }
    }
}
