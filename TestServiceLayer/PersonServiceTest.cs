// <copyright file="PersonServiceTest.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace TestServiceLayer
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using DataMapper.Interfaces;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Services;

    /// <summary>
    /// Unit tests for the <see cref="PersonService"/> class.
    /// </summary>
    [TestClass]
    public class PersonServiceTest
    {
        /// <summary>
        /// A mock implementation of the <see cref="IPersonDAO"/> used for testing data access operations related to people.
        /// </summary>
        private Mock<IPersonDAO> personDAO;

        /// <summary>
        /// The service responsible for managing person-related operations, including business logic and data manipulation.
        /// </summary>
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

        /// <summary>
        /// Tests that the <see cref="PersonService.AddBid"/> method allows placing a bid when the person's seriousness score meets the threshold.
        /// </summary>
        [TestMethod]
        public void AddBid_ShouldAllowBid_WhenPersonScoreMeetsThreshold()
        {
            // Arrange
            var person = new Person { Score = 5.0 }; // Assuming seriousnessThreshold is 4.0
            var bid = new Bid();

            // Act
            this.personService.AddBid(person, bid);

            // Assert
            Assert.AreEqual(person, bid.Bidder);
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.AddBid"/> method throws an <see cref="InvalidOperationException"/> when the person's seriousness score is below the threshold.
        /// </summary>
        [TestMethod]
        public void AddBid_ShouldThrowInvalidOperationException_WhenPersonScoreBelowThreshold()
        {
            // Arrange
            var person = new Person { Score = 3.0 }; // Assuming seriousnessThreshold is 4.0
            var bid = new Bid();

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => this.personService.AddBid(person, bid));
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.AddBid"/> method does not modify the bid when the person does not meet the threshold.
        /// </summary>
        [TestMethod]
        public void AddBid_ShouldNotModifyBid_WhenPersonScoreBelowThreshold()
        {
            // Arrange
            var person = new Person { Score = 3.0 }; // Assuming seriousnessThreshold is 4.0
            var bid = new Bid();

            try
            {
                // Act
                this.personService.AddBid(person, bid);
            }
            catch (InvalidOperationException)
            {
                // Assert
                Assert.IsNull(bid.Bidder);
            }
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.AddBid"/> method does not modify the bid if the person score is exactly at the threshold.
        /// </summary>
        [TestMethod]
        public void AddBid_ShouldNotModifyBid_WhenPersonScoreIsExactlyAtThreshold()
        {
            // Arrange
            var person = new Person { Score = 4.0 }; // Assuming seriousnessThreshold is 4.0
            var bid = new Bid();

            // Act
            this.personService.AddBid(person, bid);

            // Assert
            Assert.AreEqual(person, bid.Bidder);
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.AddBid"/> method works with multiple bids from the same person.
        /// </summary>
        [TestMethod]
        public void AddBid_ShouldAllowMultipleBidsFromSamePerson_WhenScoreMeetsThreshold()
        {
            // Arrange
            var person = new Person { Score = 5.0 }; // Assuming seriousnessThreshold is 4.0
            var bid1 = new Bid();
            var bid2 = new Bid();

            // Act
            this.personService.AddBid(person, bid1);
            this.personService.AddBid(person, bid2);

            // Assert
            Assert.AreEqual(person, bid1.Bidder);
            Assert.AreEqual(person, bid2.Bidder);
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.FinalizeAuction"/> method does not adjust the person's score if there are no bids in the auction.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_ShouldNotAdjustScore_WhenAuctionHasNoBids()
        {
            // Arrange
            var person = new Person();
            var auction = new Auction
            {
                Seller = person,
                Bids = new List<Bid>(), // Auction has no bids
            };

            // Act
            this.personService.FinalizeAuction(person, auction);

            // Assert
            Assert.AreEqual(0.0, person.Score); // Score should remain unchanged
            this.personDAO.Verify(dao => dao.Update(person), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.FinalizeAuction"/> method throws an <see cref="InvalidOperationException"/> if the auction was not started by the given person.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_ShouldThrowInvalidOperationException_WhenAuctionNotStartedByPerson()
        {
            // Arrange
            var person = new Person();
            var anotherPerson = new Person();
            var auction = new Auction
            {
                Seller = anotherPerson, // Auction was started by another person
                Bids = new List<Bid> { new Bid() },
            };

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => this.personService.FinalizeAuction(person, auction));
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.FinalizeAuction"/> method does not adjust the person's score if the auction is finalized by someone else.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_ShouldNotAdjustScore_WhenAuctionFinalizedBySomeoneElse()
        {
            // Arrange
            var person = new Person();
            var auction = new Auction
            {
                Seller = person,
                Bids = new List<Bid> { new Bid() },
            };

            // Act
            this.personService.FinalizeAuction(person, auction);

            // Assert
            Assert.AreEqual(0.1, person.Score);
            this.personDAO.Verify(dao => dao.Update(person), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.FinalizeAuction"/> method does not update the person in the DAO if the auction was not started by the given person.
        /// </summary>
        [TestMethod]
        public void FinalizeAuction_ShouldNotUpdatePersonInDAO_WhenAuctionNotStartedByPerson()
        {
            // Arrange
            var person = new Person();
            var anotherPerson = new Person();
            var auction = new Auction
            {
                Seller = anotherPerson,
                Bids = new List<Bid> { new Bid() },
            };

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => this.personService.FinalizeAuction(person, auction));
            this.personDAO.Verify(dao => dao.Update(It.IsAny<Person>()), Times.Never);
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.ProvideFeedback"/> method correctly adjusts the person's score based on feedback.
        /// </summary>
        [TestMethod]
        public void ProvideFeedback_ShouldAdjustScore_WhenValidFeedbackIsProvided()
        {
            // Arrange
            var person = new Person();
            double initialScore = 10.0;
            person.Score = initialScore;
            double feedbackScore = 0.05;

            // Act
            this.personService.ProvideFeedback(person, feedbackScore);

            // Assert
            Assert.AreEqual(initialScore, person.Score);
            this.personDAO.Verify(dao => dao.Update(person), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.ProvideFeedback"/> method correctly updates the person's score with feedback at the lower boundary.
        /// </summary>
        [TestMethod]
        public void ProvideFeedback_ShouldAdjustScoreCorrectly_WhenFeedbackAtLowerBoundary()
        {
            // Arrange
            var person = new Person();
            double initialScore = 10.0;
            person.Score = initialScore;
            double feedbackScore = -0.10;

            // Act
            this.personService.ProvideFeedback(person, feedbackScore);

            // Assert
            Assert.AreEqual(initialScore + feedbackScore, person.Score);
            this.personDAO.Verify(dao => dao.Update(person), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.ProvideFeedback"/> method correctly updates the person's score with feedback at the upper boundary.
        /// </summary>
        [TestMethod]
        public void ProvideFeedback_ShouldAdjustScoreCorrectly_WhenFeedbackAtUpperBoundary()
        {
            // Arrange
            var person = new Person();
            double initialScore = 10.0;
            person.Score = initialScore;
            double feedbackScore = 0.10;

            // Act
            this.personService.ProvideFeedback(person, feedbackScore);

            // Assert
            Assert.AreEqual(initialScore, person.Score);
            this.personDAO.Verify(dao => dao.Update(person), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="PersonService.ProvideFeedback"/> method updates the person's score and the DAO when valid feedback is provided.
        /// </summary>
        [TestMethod]
        public void ProvideFeedback_ShouldUpdatePersonInDAO_WhenValidFeedbackIsProvided()
        {
            // Arrange
            var person = new Person();
            double initialScore = 50.0;
            person.Score = initialScore;
            double feedbackScore = 0.05;

            // Act
            this.personService.ProvideFeedback(person, feedbackScore);

            // Assert
            Assert.AreEqual(10.0, person.Score);
            this.personDAO.Verify(dao => dao.Update(person), Times.Once);
        }
    }
}
