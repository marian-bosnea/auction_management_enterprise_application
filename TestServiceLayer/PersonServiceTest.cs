namespace TestServiceLayer
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using DataMapper.Interfaces;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ServiceLayer.Interfaces;
    using ServiceLayer.Services;
    using Services;

    /// <summary>
    /// Unit tests for the <see cref="PersonService"/> class.
    /// </summary>
    [TestClass]
    public class PersonServiceTest
    {
        private Mock<IPersonDAO> personDAO;
        private PersonService personService;

        /// <summary>
        /// Initializes the test environment by creating a mock <see cref="IPersonDAO"/> and an instance of <see cref="PersonService"/>.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.personDAO = new Mock<IPersonDAO>();
            this.personService = new PersonService(this.personDAO.Object);
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.StartAuction"/> method throws an <see cref="InvalidOperationException"/> when the person's score is below the threshold.
        /// </summary>
        /// <param name="threshold">The threshold score for serious participation.</param>
        /// <param name="shouldThrow">Indicates whether an exception is expected.</param>
        [TestMethod]
        [DataRow(4.0, true)]
        [DataRow(5.0, false)]
        public void StartAuction_ShouldThrowException_WhenScoreBelowThreshold(double threshold, bool shouldThrow)
        {
            // Arrange
            ConfigurationManager.AppSettings["SeriousnessThreshold"] = threshold.ToString();
            var person = new Person("Test Person") { Score = threshold - 1 };

            // Act & Assert
            if (shouldThrow)
            {
                Assert.ThrowsException<InvalidOperationException>(() => this.personService.StartAuction(person));
            }
            else
            {
                this.personService.StartAuction(person);
            }
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.AddBid"/> method throws an <see cref="InvalidOperationException"/> when the person's score is below the threshold.
        /// </summary>
        [TestMethod]
        public void AddBid_ShouldThrowException_WhenScoreBelowThreshold()
        {
            // Arrange
            var threshold = 4.0;
            ConfigurationManager.AppSettings["SeriousnessThreshold"] = threshold.ToString();
            var person = new Person("Test Person") { Score = threshold - 1 };
            var bid = new Bid(100, "USD");

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => this.personService.AddBid(person, bid));
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.AddBid"/> method assigns the <see cref="Bid.Bidder"/> property when the person's score is above the threshold.
        /// </summary>
        [TestMethod]
        public void AddBid_ShouldAssignBidder_WhenScoreAboveThreshold()
        {
            // Arrange
            var threshold = 4.0;
            ConfigurationManager.AppSettings["SeriousnessThreshold"] = threshold.ToString();
            var person = new Person("Test Person") { Score = threshold + 1 };
            var bid = new Bid(100, "USD");

            // Act
            this.personService.AddBid(person, bid);

            // Assert
            Assert.AreEqual(person, bid.Bidder);
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.FinalizeAuction"/> method throws an <see cref="InvalidOperationException"/> when the person is not the seller of the auction.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_ShouldThrowException_WhenPersonNotSeller()
        {
            // Arrange
            var person = new Person("Seller") { Score = 5.0 };
            var differentPerson = new Person("Not Seller") { Score = 5.0 };
            Product product = new Product(1, "Laptop", "A high-end gaming laptop.", new List<Category>());

            var auction = new Auction(person, product, DateTime.Now, DateTime.Now.AddDays(1), 10.0, "USD");

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => this.personService.FinalizeAuction(differentPerson, auction));
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.FinalizeAuction"/> method adjusts the person's score when the auction has bids.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_ShouldAdjustScore_WhenAuctionHasBids()
        {
            // Arrange
            var person = new Person("Test Person") { Score = 5.0 };
            Product product = new Product(1, "Laptop", "A high-end gaming laptop.", new List<Category>());

            var auction = new Auction(person, product, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 10.0, "USD");
            auction.AddBid(new Bid(100, "USD"));

            // Act
            this.personService.FinalizeAuction(person, auction);

            // Assert
            Assert.AreEqual(5.1, person.Score);
            this.personDAO.Verify(dao => dao.Update(person), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.ProvideFeedback"/> method updates the person's score and calls the <see cref="IPersonDAO.Update"/> method.
        /// </summary>
        [TestMethod]
        public void ProvideFeedback_ShouldUpdateScoreAndDAO()
        {
            // Arrange
            var person = new Person("Test Person") { Score = 5.0 };
            var feedbackScore = 0.1;

            // Act
            this.personService.ProvideFeedback(person, feedbackScore);

            // Assert
            Assert.AreEqual(5.1, person.Score);
            this.personDAO.Verify(dao => dao.Update(person), Times.Once);
        }
    }
}
