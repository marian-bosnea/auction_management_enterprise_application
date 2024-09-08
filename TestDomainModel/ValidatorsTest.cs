// <copyright file="ValidatorsTest.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the <see cref="CurrentOrFutureDateAttribute"/> custom validation attribute.
    /// </summary>
    [TestClass]
    public class ValidatorsTest
    {
        /// <summary>
        /// Tests that a future date is considered valid by the <see cref="CurrentOrFutureDateAttribute"/>.
        /// </summary>
        [TestMethod]
        public void IsValid_DateIsFutureDate_ReturnsTrue()
        {
            // Arrange
            var attribute = this.CreateAttribute();
            DateTime futureDate = DateTime.Now.AddDays(1);

            // Act
            bool result = attribute.IsValid(futureDate);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests that a past date is considered invalid by the <see cref="CurrentOrFutureDateAttribute"/>.
        /// </summary>
        [TestMethod]
        public void IsValid_DateIsPastDate_ReturnsFalse()
        {
            // Arrange
            var attribute = this.CreateAttribute();
            DateTime pastDate = DateTime.Now.AddDays(-1);

            // Act
            bool result = attribute.IsValid(pastDate);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests that a non-date value is considered invalid by the <see cref="CurrentOrFutureDateAttribute"/>.
        /// </summary>
        [TestMethod]
        public void IsValid_ValueIsNotDate_ReturnsFalse()
        {
            // Arrange
            var attribute = this.CreateAttribute();
            string nonDateValue = "Not a Date";

            // Act
            bool result = attribute.IsValid(nonDateValue);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests that the <see cref="CurrentOrFutureDateAttribute.FormatErrorMessage"/> method returns the correct error message for a property.
        /// </summary>
        [TestMethod]
        public void FormatErrorMessage_ReturnsCorrectMessage()
        {
            // Arrange
            var attribute = this.CreateAttribute();
            string propertyName = "TestDate";

            // Act
            string result = attribute.FormatErrorMessage(propertyName);

            // Assert
            string expectedMessage = $"The {propertyName} cannot be earlier than the current date.";
            Assert.AreEqual(expectedMessage, result);
        }

        /// <summary>
        /// Tests that a date exactly at midnight in the future is considered valid by the <see cref="CurrentOrFutureDateAttribute"/>.
        /// </summary>
        [TestMethod]
        public void IsValid_DateIsExactlyMidnightInFuture_ReturnsTrue()
        {
            // Arrange
            var attribute = this.CreateAttribute();
            DateTime dateExactlyMidnightInFuture = DateTime.Today.AddDays(1);

            // Act
            bool result = attribute.IsValid(dateExactlyMidnightInFuture);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests that a null date is considered invalid by the <see cref="CurrentOrFutureDateAttribute"/>.
        /// </summary>
        [TestMethod]
        public void IsValid_NullDate_ReturnsFalse()
        {
            // Arrange
            var attribute = this.CreateAttribute();
            DateTime? nullDate = null;

            // Act
            bool result = attribute.IsValid(nullDate);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests that a future date in a different time zone is considered valid by the <see cref="CurrentOrFutureDateAttribute"/>.
        /// </summary>
        [TestMethod]
        public void IsValid_FutureDateInDifferentTimeZone_ReturnsTrue()
        {
            // Arrange
            var attribute = this.CreateAttribute();
            DateTime futureDateInDifferentTimeZone = DateTime.UtcNow.AddDays(1);

            // Act
            bool result = attribute.IsValid(futureDateInDifferentTimeZone);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests that a past date in a different time zone is considered invalid by the <see cref="CurrentOrFutureDateAttribute"/>.
        /// </summary>
        [TestMethod]
        public void IsValid_PastDateInDifferentTimeZone_ReturnsFalse()
        {
            // Arrange
            var attribute = this.CreateAttribute();
            DateTime pastDateInDifferentTimeZone = DateTime.UtcNow.AddDays(-1);

            // Act
            bool result = attribute.IsValid(pastDateInDifferentTimeZone);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests that the <see cref="CurrentOrFutureDateAttribute.FormatErrorMessage"/> method returns the correct error messages for different property names.
        /// </summary>
        [TestMethod]
        public void FormatErrorMessage_WithDifferentPropertyNames_ReturnsCorrectMessage()
        {
            // Arrange
            var attribute = this.CreateAttribute();
            string propertyName1 = "StartDate";
            string propertyName2 = "EndDate";

            // Act
            string result1 = attribute.FormatErrorMessage(propertyName1);
            string result2 = attribute.FormatErrorMessage(propertyName2);

            // Assert
            Assert.AreEqual($"The {propertyName1} cannot be earlier than the current date.", result1);
            Assert.AreEqual($"The {propertyName2} cannot be earlier than the current date.", result2);
        }

        /// <summary>
        /// Creates an instance of the <see cref="CurrentOrFutureDateAttribute"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="CurrentOrFutureDateAttribute"/>.</returns>
        private CurrentOrFutureDateAttribute CreateAttribute()
        {
            return new CurrentOrFutureDateAttribute();
        }
    }
}
