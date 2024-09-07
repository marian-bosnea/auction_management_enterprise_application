using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModel;
using System;

namespace DomainModel.Tests
{
    [TestClass]
    public class PersonTests
    {
        [TestMethod]
        public void Constructor_ValidName_ShouldCreatePerson()
        {
            // Arrange
            string name = "John Doe";

            // Act
            var person = new Person(name);

            // Assert
            Assert.IsNotNull(person);
            Assert.AreEqual(name, person.Name);
            Assert.AreEqual(5.0, person.Score); // Default score
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullName_ShouldThrowArgumentNullException()
        {
            // Arrange & Act
            var person = new Person(null);
        }

        [TestMethod]
        public void AdjustScore_ValidAdjustment_ShouldUpdateScore()
        {
            // Arrange
            var person = new Person("John Doe");
            double adjustment = 0.5;

            // Act
            person.AdjustScore(adjustment);

            // Assert
            Assert.AreEqual(5.5, person.Score);
        }

        [TestMethod]
        public void AdjustScore_AdjustBelowMinimum_ShouldSetScoreToZero()
        {
            // Arrange
            var person = new Person("John Doe");
            double adjustment = -10.0;

            // Act
            person.AdjustScore(adjustment);

            // Assert
            Assert.AreEqual(0, person.Score);
        }

        [TestMethod]
        public void AdjustScore_AdjustAboveMaximum_ShouldSetScoreToTen()
        {
            // Arrange
            var person = new Person("John Doe");
            double adjustment = 100.0;

            // Act
            person.AdjustScore(adjustment);

            // Assert
            Assert.AreEqual(10, person.Score);
        }

        [TestMethod]
        public void AdjustScore_InvalidScoreValues_ShouldNotThrowExceptions()
        {
            // Arrange
            var person = new Person("John Doe");

            // Act & Assert
            person.AdjustScore(-0.1); // Lower bound
            Assert.AreEqual(4.9, person.Score);

            person.AdjustScore(0.1); // Upper bound
            Assert.AreEqual(5.0, person.Score);
        }

        [TestMethod]
        public void ToString_ShouldReturnName()
        {
            // Arrange
            var person = new Person("John Doe");

            // Act
            var result = person.ToString();

            // Assert
            Assert.AreEqual("John Doe", result);
        }

        [TestMethod]
        public void SetName_ValidName_ShouldUpdateName()
        {
            // Arrange
            var person = new Person("John Doe");
            string newName = "Jane Smith";

            // Act
            person.Name = newName;

            // Assert
            Assert.AreEqual(newName, person.Name);
        }

        [TestMethod]
        public void SetScore_ValidScoreWithinRange_ShouldUpdateScore()
        {
            // Arrange
            var person = new Person("John Doe");
            double newScore = 7.5;

            // Act
            person.Score = newScore;

            // Assert
            Assert.AreEqual(newScore, person.Score);
        }

        [TestMethod]
        public void SetScore_ExactLowerBound_ShouldSetScoreToZero()
        {
            // Arrange
            var person = new Person("John Doe");
            double newScore = -1.0;

            // Act
            person.Score = newScore;

            // Assert
            Assert.AreEqual(0, person.Score);
        }

        [TestMethod]
        public void SetScore_ExactUpperBound_ShouldSetScoreToTen()
        {
            // Arrange
            var person = new Person("John Doe");
            double newScore = 11.0;

            // Act
            person.Score = newScore;

            // Assert
            Assert.AreEqual(10, person.Score);
        }

        [TestMethod]
        public void SetRole_ValidRole_ShouldUpdateRole()
        {
            // Arrange
            var person = new Person("John Doe");
            PersonRole newRole = PersonRole.Bidder;

            // Act
            person.Role = newRole;

            // Assert
            Assert.AreEqual(newRole, person.Role);
        }

        [TestMethod]
        public void SetRole_InvalidRole_ShouldThrowException()
        {
            // Arrange
            var person = new Person("John Doe");

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => person.Role = (PersonRole)999);
        }

        [TestMethod]
        public void AdjustScore_ExceedingAdjustment_ShouldBeClampedToMaximum()
        {
            // Arrange
            var person = new Person("John Doe");
            double adjustment = 100.0;

            // Act
            person.AdjustScore(adjustment);

            // Assert
            Assert.AreEqual(10, person.Score);
        }

        [TestMethod]
        public void AdjustScore_MinimumAdjustment_ShouldBeClampedToMinimum()
        {
            // Arrange
            var person = new Person("John Doe");
            double adjustment = -100.0;

            // Act
            person.AdjustScore(adjustment);

            // Assert
            Assert.AreEqual(0, person.Score);
        }

        [TestMethod]
        public void AdjustScore_BoundaryValues_ShouldStayWithinLimits()
        {
            // Arrange
            var person = new Person("John Doe");

            // Act
            person.AdjustScore(-0.1);
            Assert.AreEqual(4.9, person.Score);

            person.AdjustScore(0.1);
            Assert.AreEqual(5.0, person.Score);
        }

        [TestMethod]
        public void Name_SetToNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var person = new Person("John Doe");

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => person.Name = null);
        }

        [TestMethod]
        public void Name_SetToEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var person = new Person("John Doe");
            string emptyName = string.Empty;

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => person.Name = emptyName);
            Assert.AreEqual("Name must be between 1 and 100 characters long.", exception.Message);
        }

        [TestMethod]
        public void Name_SetToTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var person = new Person("John Doe");
            string longName = new string('A', 101); // 101 characters long

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() => person.Name = longName);
            Assert.AreEqual("Name must be between 1 and 100 characters long.", exception.Message);
        }

        [TestMethod]
        public void DefaultScore_ShouldBeFive()
        {
            // Arrange & Act
            var person = new Person("John Doe");

            // Assert
            Assert.AreEqual(5.0, person.Score);
        }

        [TestMethod]
        public void AdjustScore_AdjustWithinValidRange_ShouldUpdateScore()
        {
            // Arrange
            var person = new Person("John Doe");
            double adjustment = 0.5;

            // Act
            person.AdjustScore(adjustment);

            // Assert
            Assert.AreEqual(5.5, person.Score);
        }

        [TestMethod]
        public void AdjustScore_AdjustToMaximum_ShouldClampAtTen()
        {
            // Arrange
            var person = new Person("John Doe");
            person.AdjustScore(4.0);
            person.AdjustScore(2.0);

            // Act
            double result = person.Score;

            // Assert
            Assert.AreEqual(10.0, result);
        }

        [TestMethod]
        public void AdjustScore_AdjustToMinimum_ShouldClampAtZero()
        {
            // Arrange
            var person = new Person("John Doe");
            person.AdjustScore(-6.0);

            // Act
            double result = person.Score;

            // Assert
            Assert.AreEqual(0.0, result);
        }
    }
}
