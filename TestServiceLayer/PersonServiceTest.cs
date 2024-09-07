using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using DomainModel;
using ServiceLayer.Services;
using ServiceLayer.Interfaces;
using DataMapper.Interfaces;
using Services;

[TestClass]
public class PersonServiceTests
{
    private Mock<IPersonDAO> personDAO;
    private PersonService personService;

    [TestInitialize]
    public void SetUp()
    {
        personDAO = new Mock<IPersonDAO>();
        personService = new PersonService(personDAO.Object);
    }

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
            Assert.ThrowsException<InvalidOperationException>(() => personService.StartAuction(person));
        }
        else
        {
            personService.StartAuction(person);
        }
    }

    [TestMethod]
    public void AddBid_ShouldThrowException_WhenScoreBelowThreshold()
    {
        // Arrange
        var threshold = 4.0;
        ConfigurationManager.AppSettings["SeriousnessThreshold"] = threshold.ToString();
        var person = new Person("Test Person") { Score = threshold - 1 };
        var bid = new Bid(100, "USD");

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => personService.AddBid(person, bid));
    }

    [TestMethod]
    public void AddBid_ShouldAssignBidder_WhenScoreAboveThreshold()
    {
        // Arrange
        var threshold = 4.0;
        ConfigurationManager.AppSettings["SeriousnessThreshold"] = threshold.ToString();
        var person = new Person("Test Person") { Score = threshold + 1 };
        var bid = new Bid(100, "USD");

        // Act
        personService.AddBid(person, bid);

        // Assert
        Assert.AreEqual(person, bid.Bidder);
    }

    [TestMethod]
    public void FinalizeAuction_ShouldThrowException_WhenPersonNotSeller()
    {
        // Arrange
        var person = new Person("Seller") { Score = 5.0 };
        var differentPerson = new Person("Not Seller") { Score = 5.0 };
        Product product = new Product(1, "Laptop", "A high-end gaming laptop.", new List<Category>());

        var auction = new Auction(person,product, DateTime.Now, DateTime.Now.AddDays(1), 10.0, "USD");

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => personService.FinalizeAuction(differentPerson, auction));
    }

    [TestMethod]
    public void FinalizeAuction_ShouldAdjustScore_WhenAuctionHasBids()
    {
        // Arrange
        var person = new Person("Test Person") { Score = 5.0 };
        Product product = new Product(1, "Laptop", "A high-end gaming laptop.", new List<Category>());

        var auction = new Auction(person, product, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), 10.0, "USD");
        auction.AddBid(new Bid(100, "USD"));

        // Act
        personService.FinalizeAuction(person, auction);

        // Assert
        Assert.AreEqual(5.1, person.Score);
        personDAO.Verify(dao => dao.Update(person), Times.Once);
    }

    [TestMethod]
    public void ProvideFeedback_ShouldUpdateScoreAndDAO()
    {
        // Arrange
        var person = new Person("Test Person") { Score = 5.0 };
        var feedbackScore = 0.1;

        // Act
        personService.ProvideFeedback(person, feedbackScore);

        // Assert
        Assert.AreEqual(5.1, person.Score);
        personDAO.Verify(dao => dao.Update(person), Times.Once);
    }
}
