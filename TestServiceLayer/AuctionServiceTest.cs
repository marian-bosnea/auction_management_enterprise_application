// <copyright file="AuctionServiceTest.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace Services.Tests
{
    using System;
    using System.Collections.Generic;
    using DataMapper.Interfaces;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Unit tests for the <see cref="AuctionService"/> class.
    /// </summary>
    [TestClass]
    public class AuctionServiceTest
    {
        /// <summary>
        /// A mock implementation of the <see cref="IAuctionDAO"/> used for testing data access operations related to auctions.
        /// </summary>
        private Mock<IAuctionDAO> auctionDAOMock;

        /// <summary>
        /// A mock implementation of the <see cref="IBidDAO"/> used for testing data access operations related to bids.
        /// </summary>
        private Mock<IBidDAO> bidDAOMock;

        /// <summary>
        /// The service responsible for managing auction-related operations, including business logic and data manipulation.
        /// </summary>
        private AuctionService auctionService;

        /// <summary>
        /// Initializes the test environment by creating mocks and an instance of <see cref="AuctionService"/>.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.auctionDAOMock = new Mock<IAuctionDAO>();
            this.bidDAOMock = new Mock<IBidDAO>();
            this.auctionService = new AuctionService(this.auctionDAOMock.Object, this.bidDAOMock.Object);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.StartAuction"/> method correctly starts an auction when provided valid details.
        /// </summary>
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

        /// <summary>
        /// Tests that the <see cref="AuctionService.StartAuction"/> method does not throw an exception when no category exceeds the maximum number of active auctions.
        /// </summary>
        [TestMethod]
        public void StartAuction_ShouldNotThrowException_WhenNoCategoryExceedsMaxAuctionsPerCategory()
        {
            // Arrange
            var person = new Person("John Doe");
            var category1 = new Category("Category1");
            var category2 = new Category("Category2");
            var product = new Product(1, "Product", "Description", new List<Category> { category1, category2 });
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(2);
            var startingPrice = 100.0;
            var currency = "USD";

            // Mocking the DAO methods
            this.auctionDAOMock.Setup(dao => dao.GetActiveAuctionsForPerson(person)).Returns(new List<Auction>());
            this.auctionDAOMock.Setup(dao => dao.GetActiveAuctionsForPersonInCategory(person, category1)).Returns(new List<Auction>());
            this.auctionDAOMock.Setup(dao => dao.GetActiveAuctionsForPersonInCategory(person, category2)).Returns(new List<Auction>());

            // Act
            try
            {
                this.auctionService.StartAuction(person, product, startDate, endDate, startingPrice, currency);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception was thrown: {ex.Message}");
            }
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.StartAuction"/> method throws an exception when a category exceeds the maximum number of active auctions.
        /// </summary>
        [TestMethod]
        public void StartAuction_ShouldThrowException_WhenCategoryExceedsMaxAuctionsPerCategory()
        {
            // Arrange
            var person = new Person("John Doe");
            var category = new Category("Category1");
            var product = new Product(1, "Product", "Description", new List<Category> { category });
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(2);
            var startingPrice = 100.0;
            var currency = "USD";

            var auction = new Auction(person, new Product(1, "Product A", "Description", new List<Category> { category }), startDate, endDate, startingPrice, currency);

            this.auctionDAOMock.Setup(a => a.GetActiveAuctionsForPerson(person)).Returns(new List<Auction>());
            this.auctionDAOMock.Setup(a => a.GetActiveAuctionsForPersonInCategory(person, category)).Returns(new List<Auction> { auction });

            // Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                this.auctionService.StartAuction(person, product, startDate, endDate, startingPrice, currency));
            Assert.AreEqual($"Cannot start a new auction. Maximum of {this.auctionService.MaxActiveAuctionsPerCategory} active auctions in category '{category.Name}' reached.", ex.Message);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.StartAuction"/> method throws an exception when multiple categories exceed the maximum number of active auctions.
        /// </summary>
        [TestMethod]
        public void StartAuction_ShouldThrowException_WhenMultipleCategoriesExceedMaxAuctionsPerCategory()
        {
            // Arrange
            var person = new Person("John Doe");
            var category1 = new Category("Category1");
            var category2 = new Category("Category2");
            var product = new Product(1, "Product", "Description", new List<Category> { category1, category2 });
            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddDays(2);
            var startingPrice = 100.0;
            var currency = "USD";

            var auction = new Auction(person, new Product(1, "Product A", "Description", new List<Category> { category1 }), startDate, endDate, startingPrice, currency);

            this.auctionDAOMock.Setup(a => a.GetActiveAuctionsForPerson(person)).Returns(new List<Auction>());
            this.auctionDAOMock.Setup(a => a.GetActiveAuctionsForPersonInCategory(person, category1)).Returns(new List<Auction> { auction });
            this.auctionDAOMock.Setup(a => a.GetActiveAuctionsForPersonInCategory(person, category2)).Returns(new List<Auction> { auction });

            // Act & Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                this.auctionService.StartAuction(person, product, startDate, endDate, startingPrice, currency));
            Assert.IsTrue(ex.Message.Contains("Maximum of"));
            Assert.IsTrue(ex.Message.Contains("active auctions in category 'Category1' reached") ||
                          ex.Message.Contains("active auctions in category 'Category2' reached"));
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.FinalizeAuction"/> method finalizes an auction if it is not already completed.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_AuctionNotCompleted_FinalizesAuction()
        {
            // Arrange
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now, DateTime.Now.AddDays(1), 100, "USD")
            {
                IsCompleted = false,
            };

            // Act
            this.auctionService.FinalizeAuction(new Person("John Doe"), auction);

            // Assert
            Assert.IsTrue(auction.IsCompleted);
            this.auctionDAOMock.Verify(a => a.Update(auction), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.FinalizeAuction"/> method throws an exception if the auction is already completed.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_AuctionAlreadyCompleted_ThrowsException()
        {
            // Arrange
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now, DateTime.Now.AddDays(1), 100, "USD")
            {
                IsCompleted = true,
            };

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                this.auctionService.FinalizeAuction(new Person("John Doe"), auction));
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.GetAuctionById"/> method returns the correct auction for a valid ID.
        /// </summary>
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

        /// <summary>
        /// Tests that the <see cref="AuctionService.GetAllAuctions"/> method returns all auctions.
        /// </summary>
        [TestMethod]
        public void GetAllAuctions_ReturnsAllAuctions()
        {
            // Arrange
            var auctions = new List<Auction>
            {
                new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now, DateTime.Now.AddDays(1), 100, "USD"),
                new Auction(new Person("Jane Smith"), new Product(2, "Product B", "Description", new List<Category>()), DateTime.Now, DateTime.Now.AddDays(2), 200, "USD"),
            };
            this.auctionDAOMock.Setup(a => a.GetAll()).Returns(auctions);

            // Act
            var result = this.auctionService.GetAllAuctions();

            // Assert
            CollectionAssert.AreEqual(auctions, result);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.UpdateAuction"/> method correctly updates an auction.
        /// </summary>
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

        /// <summary>
        /// Tests that the <see cref="AuctionService.AddBid"/> method throws an exception if the bid currency does not match the auction currency.
        /// </summary>
        [TestMethod]
        public void AddBid_CurrencyMismatch_ThrowsArgumentException()
        {
            // Arrange
            var auction = new Auction(new Person("John Doe"), new Product(1, "Product A", "Description", new List<Category>()), DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 100, "USD");
            var bid = new Bid(110, "EUR");

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => this.auctionService.AddBid(auction, bid));
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.AddBid"/> method throws an exception if the bid amount is lower than the highest bid.
        /// </summary>
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

        /// <summary>
        /// Tests that the <see cref="AuctionService.AddBid"/> method adds a bid when there are no previous bids.
        /// </summary>
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

        /// <summary>
        /// Tests that the <see cref="AuctionService.AddAuction"/> method calls the DAO's <c>Add</c> method.
        /// </summary>
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

        /// <summary>
        /// Tests that the <see cref="AuctionService.AddAuction"/> method throws an exception when the DAO's <c>Add</c> method fails.
        /// </summary>
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

        /// <summary>
        /// Tests that the <see cref="AuctionService.DeleteAuction"/> method calls the DAO's <c>Delete</c> method when the auction exists.
        /// </summary>
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

        /// <summary>
        /// Tests that the <see cref="AuctionService.DeleteAuction"/> method does not call the DAO's <c>Delete</c> method when the auction does not exist.
        /// </summary>
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

        /// <summary>
        /// Tests that the <see cref="AuctionService.FinalizeAuction"/> method successfully marks an auction as completed.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_ShouldCompleteAuction_WhenAuctionIsNotCompleted()
        {
            // Arrange
            var person = new Person();
            var auction = new Auction(person, new Product(), DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD")
            {
                IsCompleted = false,
            };

            this.auctionDAOMock.Setup(dao => dao.Update(auction)).Verifiable();

            // Act
            this.auctionService.FinalizeAuction(person, auction);

            // Assert
            Assert.IsTrue(auction.IsCompleted);
            this.auctionDAOMock.Verify(dao => dao.Update(auction), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.FinalizeAuction"/> method throws an exception when attempting to finalize an already completed auction.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_ShouldThrowException_WhenAuctionAlreadyCompleted()
        {
            // Arrange
            var person = new Person();
            var auction = new Auction(person, new Product(), DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD")
            {
                IsCompleted = true,
            };

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => this.auctionService.FinalizeAuction(person, auction));
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.FinalizeAuction"/> method correctly interacts with the DAO when finalizing an auction.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_ShouldCallUpdateOnDAO_WhenAuctionIsFinalized()
        {
            // Arrange
            var person = new Person();
            var auction = new Auction(person, new Product(), DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD")
            {
                IsCompleted = false,
            };

            this.auctionDAOMock.Setup(dao => dao.Update(auction)).Verifiable();

            // Act
            this.auctionService.FinalizeAuction(person, auction);

            // Assert
            this.auctionDAOMock.Verify(dao => dao.Update(auction), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.FinalizeAuction"/> method does not call the DAO update if the auction is already completed.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_ShouldNotCallUpdateOnDAO_WhenAuctionAlreadyCompleted()
        {
            // Arrange
            var person = new Person();
            var auction = new Auction(person, new Product(), DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD")
            {
                IsCompleted = true,
            };

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => this.auctionService.FinalizeAuction(person, auction));
            this.auctionDAOMock.Verify(dao => dao.Update(It.IsAny<Auction>()), Times.Never);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.DeleteAuction"/> method successfully deletes an existing auction.
        /// </summary>
        [TestMethod]
        public void DeleteAuction_ShouldDeleteAuction_WhenAuctionExists()
        {
            // Arrange
            int auctionId = 1;
            var auction = new Auction(new Person(), new Product(), DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD");

            this.auctionDAOMock.Setup(dao => dao.Get(auctionId)).Returns(auction);
            this.auctionDAOMock.Setup(dao => dao.Delete(auctionId)).Verifiable();

            // Act
            this.auctionService.DeleteAuction(auctionId);

            // Assert
            this.auctionDAOMock.Verify(dao => dao.Delete(auctionId), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.DeleteAuction"/> method does nothing when the auction does not exist.
        /// </summary>
        [TestMethod]
        public void DeleteAuction_ShouldNotDeleteAuction_WhenAuctionDoesNotExist()
        {
            // Arrange
            int auctionId = 2;

            this.auctionDAOMock.Setup(dao => dao.Get(auctionId)).Returns((Auction)null);
            this.auctionDAOMock.Setup(dao => dao.Delete(auctionId)).Verifiable();

            // Act
            this.auctionService.DeleteAuction(auctionId);

            // Assert
            this.auctionDAOMock.Verify(dao => dao.Delete(auctionId), Times.Never);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.DeleteAuction"/> method correctly interacts with the DAO to delete an auction.
        /// </summary>
        [TestMethod]
        public void DeleteAuction_ShouldCallDeleteOnDAO_WhenAuctionExists()
        {
            // Arrange
            int auctionId = 3;
            var auction = new Auction(new Person(), new Product(), DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD");

            this.auctionDAOMock.Setup(dao => dao.Get(auctionId)).Returns(auction);
            this.auctionDAOMock.Setup(dao => dao.Delete(auctionId)).Verifiable();

            // Act
            this.auctionService.DeleteAuction(auctionId);

            // Assert
            this.auctionDAOMock.Verify(dao => dao.Delete(auctionId), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.DeleteAuction"/> method does not call Delete on DAO if the auction is null.
        /// </summary>
        [TestMethod]
        public void DeleteAuction_ShouldNotCallDeleteOnDAO_WhenAuctionDoesNotExist()
        {
            // Arrange
            int auctionId = 4;

            this.auctionDAOMock.Setup(dao => dao.Get(auctionId)).Returns((Auction)null);
            this.auctionDAOMock.Setup(dao => dao.Delete(auctionId)).Verifiable();

            // Act
            this.auctionService.DeleteAuction(auctionId);

            // Assert
            this.auctionDAOMock.Verify(dao => dao.Delete(auctionId), Times.Never);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.UpdateAuction"/> method throws an <see cref="ArgumentNullException"/> when the auction is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateAuction_ShouldThrowArgumentNullException_WhenAuctionIsNull()
        {
            // Act
            this.auctionService.UpdateAuction(null);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.UpdateAuction"/> method correctly interacts with the DAO to update an auction.
        /// </summary>
        [TestMethod]
        public void UpdateAuction_ShouldCallUpdateOnDAO_WhenAuctionIsNotNull()
        {
            // Arrange
            var auction = new Auction(new Person(), new Product(), DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD");

            this.auctionDAOMock.Setup(dao => dao.Update(auction)).Verifiable();

            // Act
            this.auctionService.UpdateAuction(auction);

            // Assert
            this.auctionDAOMock.Verify(dao => dao.Update(auction), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.UpdateAuction"/> method handles the case where the DAO's Update method is called with valid data.
        /// </summary>
        [TestMethod]
        public void UpdateAuction_ShouldUpdateAuction_WhenAuctionIsValid()
        {
            // Arrange
            var auction = new Auction(new Person(), new Product(), DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD");

            this.auctionDAOMock.Setup(dao => dao.Update(It.IsAny<Auction>())).Verifiable();

            // Act
            this.auctionService.UpdateAuction(auction);

            // Assert
            this.auctionDAOMock.Verify(dao => dao.Update(auction), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.UpdateAuction"/> method does not update if the DAO is not called (by checking for no updates).
        /// </summary>
        [TestMethod]
        public void UpdateAuction_ShouldNotCallUpdateOnDAO_WhenAuctionIsNull()
        {
            // Act
            try
            {
                this.auctionService.UpdateAuction(null);
            }
            catch (ArgumentNullException)
            {
                // Expected exception
            }

            // Assert
            this.auctionDAOMock.Verify(dao => dao.Update(It.IsAny<Auction>()), Times.Never);
        }

        /// <summary>
        /// Tests that the <see cref="AuctionService.AddBid"/> method throws an exception when the bid currency does not match the auction currency.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddBid_ShouldThrowArgumentException_WhenBidCurrencyDoesNotMatchAuctionCurrency()
        {
            // Arrange
            var person = new Person();
            var product = new Product();
            var auction = new Auction(person, product, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 100, "USD");
            var bid = new Bid(110, "EUR");

            this.auctionDAOMock.Setup(dao => dao.Get(auction.Id)).Returns(auction);

            // Act
            this.auctionService.AddBid(auction, bid);
        }
    }
}
