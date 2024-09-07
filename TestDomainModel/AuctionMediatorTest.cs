using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceLayer;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;

namespace TestDomainModel
{
    [TestClass]
    public class AuctionMediatorTest
    {
        private Mock<IAuctionService> auctionServiceMock;
        private Mock<IPersonService> personServiceMock;
        private AuctionMediator auctionMediator;

        [TestInitialize]
        public void SetUp()
        {
            auctionServiceMock = new Mock<IAuctionService>();
            personServiceMock = new Mock<IPersonService>();
            auctionMediator = new AuctionMediator(auctionServiceMock.Object, personServiceMock.Object);
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenAuctionServiceIsNull()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new AuctionMediator(null, personServiceMock.Object));
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenPersonServiceIsNull()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new AuctionMediator(auctionServiceMock.Object, null));
        }

        [TestMethod]
        public void StartAuction_ShouldCallPersonServiceStartAuction()
        {
            // Arrange
            var person = new Person("Person");
            var product = new Product(1, "Product", "Description", new List<Category>());
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(1);
            var startingPrice = 100.0;
            var currency = "USD";

            // Act
            auctionMediator.StartAuction(person, product, startDate, endDate, startingPrice, currency);

            // Assert
            personServiceMock.Verify(service => service.StartAuction(person), Times.Once);
        }

        [TestMethod]
        public void StartAuction_ShouldCallAuctionServiceStartAuction()
        {
            // Arrange
            var person = new Person("Person");
            var product = new Product(1, "Product", "Description", new List<Category>());
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(1);
            var startingPrice = 100.0;
            var currency = "USD";

            // Act
            auctionMediator.StartAuction(person, product, startDate, endDate, startingPrice, currency);

            // Assert
            auctionServiceMock.Verify(service => service.StartAuction(person, product, startDate, endDate, startingPrice, currency), Times.Once);
        }

        [TestMethod]
        public void AddBid_ShouldCallPersonServiceAddBid()
        {
            // Arrange
            var person = new Person("Person");
            var product = new Product(1, "Name", "Description", new List<Category>());

            var auction = new Auction(person, product, DateTime.Now, DateTime.Now.AddDays(1), 100.0, "USD");
            var bid = new Bid(100.0, "USD");

            // Act
            auctionMediator.AddBid(person, auction, bid);

            // Assert
            personServiceMock.Verify(service => service.AddBid(person, bid), Times.Once);
        }

        [TestMethod]
        public void AddBid_ShouldCallAuctionServiceAddBid()
        {
            // Arrange
            var person = new Person("Person");

            var product = new Product(1, "Name", "Description", new List<Category>());
            var bid = new Bid(100.0, "USD");

            var auction = new Auction(person, product, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100.0, "USD");
            // Act
            auctionMediator.AddBid(person, auction, bid);

            // Assert
            auctionServiceMock.Verify(service => service.AddBid(auction, bid), Times.Once);
        }

        [TestMethod]
        public void FinalizeAuction_ShouldCallPersonServiceFinalizeAuction()
        {
            // Arrange
            var person = new Person("Person");
            var product = new Product(1, "Name", "Description", new List<Category>());

            var auction = new Auction(person, product, DateTime.Now, DateTime.Now.AddDays(1), 100.0, "USD");

            // Act
            auctionMediator.FinalizeAuction(person, auction);

            // Assert
            personServiceMock.Verify(service => service.FinalizeAuction(person, auction), Times.Once);
        }

        [TestMethod]
        public void FinalizeAuction_ShouldCallAuctionServiceFinalizeAuction()
        {
            // Arrange
            var person = new Person("Person");
            var product = new Product(1, "Name", "Description", new List<Category>());

            var auction = new Auction(person, product, DateTime.Now, DateTime.Now.AddDays(1), 100.0, "USD");

            // Act
            auctionMediator.FinalizeAuction(person, auction);

            // Assert
            auctionServiceMock.Verify(service => service.FinalizeAuction(person, auction), Times.Once);
        }

        [TestMethod]
        public void ProvideFeedback_ShouldCallPersonServiceProvideFeedback()
        {
            // Arrange
            var person = new Person("Person");
            var feedbackScore = 0.1;

            // Act
            auctionMediator.ProvideFeedback(person, feedbackScore);

            // Assert
            personServiceMock.Verify(service => service.ProvideFeedback(person, feedbackScore), Times.Once);
        }
    }
}
