namespace TestServiceLayer
{
    using System;
    using System.Collections.Generic;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ServiceLayer;
    using ServiceLayer.Interfaces;

    /// <summary>
    /// Unit tests for the <see cref="AuctionMediator"/> class.
    /// These tests verify that the <see cref="AuctionMediator"/> correctly interacts with
    /// <see cref="IAuctionService"/> and <see cref="IPersonService"/> for managing auctions and bids.
    /// </summary>
    [TestClass]
    public class AuctionMediatorTest
    {
        private Mock<IAuctionService> auctionServiceMock;
        private Mock<IPersonService> personServiceMock;
        private AuctionMediator auctionMediator;

        /// <summary>
        /// Initializes the test environment by creating mocks for <see cref="IAuctionService"/> and
        /// <see cref="IPersonService"/> and initializing an instance of <see cref="AuctionMediator"/>.
        /// This method is called before each test method is executed.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.auctionServiceMock = new Mock<IAuctionService>();
            this.personServiceMock = new Mock<IPersonService>();
            this.auctionMediator = new AuctionMediator(this.auctionServiceMock.Object, this.personServiceMock.Object);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionMediator"/> constructor throws an
        /// <see cref="ArgumentNullException"/> when the <see cref="IAuctionService"/> parameter is null.
        /// </summary>
        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenAuctionServiceIsNull()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new AuctionMediator(null, this.personServiceMock.Object));
        }

        /// <summary>
        /// Tests that the <see cref="AuctionMediator"/> constructor throws an
        /// <see cref="ArgumentNullException"/> when the <see cref="IPersonService"/> parameter is null.
        /// </summary>
        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenPersonServiceIsNull()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new AuctionMediator(this.auctionServiceMock.Object, null));
        }

        /// <summary>
        /// Tests that <see cref="AuctionMediator.StartAuction"/> calls the <see cref="IPersonService.StartAuction"/>
        /// method with the correct parameters.
        /// </summary>
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
            this.auctionMediator.StartAuction(person, product, startDate, endDate, startingPrice, currency);

            // Assert
            this.personServiceMock.Verify(service => service.StartAuction(person), Times.Once);
        }

        /// <summary>
        /// Tests that <see cref="AuctionMediator.StartAuction"/> calls the <see cref="IAuctionService.StartAuction"/>
        /// method with the correct parameters.
        /// </summary>
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
            this.auctionMediator.StartAuction(person, product, startDate, endDate, startingPrice, currency);

            // Assert
            this.auctionServiceMock.Verify(service => service.StartAuction(person, product, startDate, endDate, startingPrice, currency), Times.Once);
        }

        /// <summary>
        /// Tests that <see cref="AuctionMediator.AddBid"/> calls the <see cref="IPersonService.AddBid"/>
        /// method with the correct parameters.
        /// </summary>
        [TestMethod]
        public void AddBid_ShouldCallPersonServiceAddBid()
        {
            // Arrange
            var person = new Person("Person");
            var product = new Product(1, "Name", "Description", new List<Category>());
            var auction = new Auction(person, product, DateTime.Now, DateTime.Now.AddDays(1), 100.0, "USD");
            var bid = new Bid(100.0, "USD");

            // Act
            this.auctionMediator.AddBid(person, auction, bid);

            // Assert
            this.personServiceMock.Verify(service => service.AddBid(person, bid), Times.Once);
        }

        /// <summary>
        /// Tests that <see cref="AuctionMediator.AddBid"/> calls the <see cref="IAuctionService.AddBid"/>
        /// method with the correct parameters.
        /// </summary>
        [TestMethod]
        public void AddBid_ShouldCallAuctionServiceAddBid()
        {
            // Arrange
            var person = new Person("Person");
            var product = new Product(1, "Name", "Description", new List<Category>());
            var auction = new Auction(person, product, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100.0, "USD");
            var bid = new Bid(100.0, "USD");

            // Act
            this.auctionMediator.AddBid(person, auction, bid);

            // Assert
            this.auctionServiceMock.Verify(service => service.AddBid(auction, bid), Times.Once);
        }

        /// <summary>
        /// Tests that <see cref="AuctionMediator.FinalizeAuction"/> calls the <see cref="IPersonService.FinalizeAuction"/>
        /// method with the correct parameters.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_ShouldCallPersonServiceFinalizeAuction()
        {
            // Arrange
            var person = new Person("Person");
            var product = new Product(1, "Name", "Description", new List<Category>());
            var auction = new Auction(person, product, DateTime.Now, DateTime.Now.AddDays(1), 100.0, "USD");

            // Act
            this.auctionMediator.FinalizeAuction(person, auction);

            // Assert
            this.personServiceMock.Verify(service => service.FinalizeAuction(person, auction), Times.Once);
        }

        /// <summary>
        /// Tests that <see cref="AuctionMediator.FinalizeAuction"/> calls the <see cref="IAuctionService.FinalizeAuction"/>
        /// method with the correct parameters.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_ShouldCallAuctionServiceFinalizeAuction()
        {
            // Arrange
            var person = new Person("Person");
            var product = new Product(1, "Name", "Description", new List<Category>());
            var auction = new Auction(person, product, DateTime.Now, DateTime.Now.AddDays(1), 100.0, "USD");

            // Act
            this.auctionMediator.FinalizeAuction(person, auction);

            // Assert
            this.auctionServiceMock.Verify(service => service.FinalizeAuction(person, auction), Times.Once);
        }

        /// <summary>
        /// Tests that <see cref="AuctionMediator.ProvideFeedback"/> calls the <see cref="IPersonService.ProvideFeedback"/>
        /// method with the correct parameters.
        /// </summary>
        [TestMethod]
        public void ProvideFeedback_ShouldCallPersonServiceProvideFeedback()
        {
            // Arrange
            var person = new Person("Person");
            var feedbackScore = 0.1;

            // Act
            this.auctionMediator.ProvideFeedback(person, feedbackScore);

            // Assert
            this.personServiceMock.Verify(service => service.ProvideFeedback(person, feedbackScore), Times.Once);
        }
    }
}
