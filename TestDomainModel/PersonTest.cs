// <copyright file="PersonTest.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel.Tests
{
    using System;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the <see cref="Person"/> class.
    /// </summary>
    [TestClass]
    public class PersonTest
    {
        /// <summary>
        /// Tests that the constructor correctly creates a person with a valid name.
        /// </summary>
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

        /// <summary>
        /// Tests that the constructor throws an <see cref="ArgumentNullException"/> when the name is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullName_ShouldThrowArgumentNullException()
        {
            // Arrange & Act
            var person = new Person(null);
        }

        /// <summary>
        /// Tests that adjusting the score by a valid amount updates the score correctly.
        /// </summary>
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

        /// <summary>
        /// Tests that adjusting the score below the minimum value clamps it to zero.
        /// </summary>
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

        /// <summary>
        /// Tests that adjusting the score above the maximum value clamps it to ten.
        /// </summary>
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

        /// <summary>
        /// Tests that small adjustments to the score, either below or above, do not throw exceptions.
        /// </summary>
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

        /// <summary>
        /// Tests that the <see cref="Person.ToString"/> method returns the person's name.
        /// </summary>
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

        /// <summary>
        /// Tests that setting a valid name updates the person's name.
        /// </summary>
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

        /// <summary>
        /// Tests that setting a valid score within the allowable range updates the person's score.
        /// </summary>
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

        /// <summary>
        /// Tests that setting a score below the lower bound clamps it to zero.
        /// </summary>
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

        /// <summary>
        /// Tests that setting a score above the upper bound clamps it to ten.
        /// </summary>
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

        /// <summary>
        /// Tests that setting a valid role updates the person's role.
        /// </summary>
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

        /// <summary>
        /// Tests that setting an invalid role throws an <see cref="ArgumentException"/>.
        /// </summary>
        [TestMethod]
        public void SetRole_InvalidRole_ShouldThrowException()
        {
            // Arrange
            var person = new Person("John Doe");

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => person.Role = (PersonRole)999);
        }

        /// <summary>
        /// Tests that exceeding the maximum adjustment for a score clamps it to ten.
        /// </summary>
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

        /// <summary>
        /// Tests that a score adjustment below the minimum clamps the score to zero.
        /// </summary>
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

        /// <summary>
        /// Tests that boundary value adjustments keep the score within valid limits.
        /// </summary>
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

        /// <summary>
        /// Tests that setting the name to null throws an <see cref="ArgumentNullException"/>.
        /// </summary>
        [TestMethod]
        public void Name_SetToNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var person = new Person("John Doe");

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => person.Name = null);
        }

        /// <summary>
        /// Tests that setting the name to an empty string throws an <see cref="ArgumentException"/> with a validation error.
        /// </summary>
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

        /// <summary>
        /// Tests that setting the name to a string longer than 100 characters throws an <see cref="ArgumentException"/> with a validation error.
        /// </summary>
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

        /// <summary>
        /// Tests that the default score for a person is 5.0 upon creation.
        /// </summary>
        [TestMethod]
        public void DefaultScore_ShouldBeFive()
        {
            // Arrange & Act
            var person = new Person("John Doe");

            // Assert
            Assert.AreEqual(5.0, person.Score);
        }

        /// <summary>
        /// Tests that a valid score adjustment within the allowable range updates the score.
        /// </summary>
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

        /// <summary>
        /// Tests that adjusting the score to the maximum clamping limit results in a score of 10.0.
        /// </summary>
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

        /// <summary>
        /// Tests that adjusting the score to the minimum clamping limit results in a score of 0.0.
        /// </summary>
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
