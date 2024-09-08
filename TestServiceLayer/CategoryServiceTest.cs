// <copyright file="CategoryServiceTest.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace TestServiceLayer
{
    using System;
    using System.Collections.Generic;
    using DataMapper.Interfaces;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ServiceLayer.Services;

    /// <summary>
    /// Unit tests for the <see cref="CategoryService"/> class.
    /// </summary>
    [TestClass]
    public class CategoryServiceTest
    {
        /// <summary>
        /// Represents a mock of the <see cref="ICategoryDAO"/> interface used for testing purposes.
        /// </summary>
        /// <remarks>
        /// This field is initialized with an instance of <see cref="Mock{ICategoryDAO}"/> to simulate the behavior of the <see cref="ICategoryDAO"/>
        /// interface. It is used to test the interaction of the <see cref="CategoryService"/> with the data access layer without requiring a real
        /// database.
        /// </remarks>
        private Mock<ICategoryDAO> categoryDAOMock;

        /// <summary>
        /// Represents the service for managing operations related to <see cref="Category"/> entities in the tests.
        /// </summary>
        /// <remarks>
        /// This field is initialized with an instance of <see cref="CategoryService"/> and is used to perform business logic operations related to
        /// <see cref="Category"/> entities. It interacts with the <see cref="categoryDAOMock"/> to perform various actions and operations.
        /// </remarks>
        private CategoryService categoryService;

        /// <summary>
        /// Represents a dictionary of <see cref="Category"/> objects used for test data management.
        /// </summary>
        /// <remarks>
        /// This field is a dictionary where the keys are category names and the values are instances of <see cref="Category"/>. It is used to
        /// easily manage and access categories for testing purposes, allowing quick setup and retrieval of test data.
        /// </remarks>
        private Dictionary<string, Category> categories;

        /// <summary>
        /// Initializes the test environment before each test method is run.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.categoryDAOMock = new Mock<ICategoryDAO>();
            this.categories = new Dictionary<string, Category>();
            this.categoryService = new CategoryService(this.categoryDAOMock.Object);
        }

        /// <summary>
        /// Tests that the <see cref="CategoryService.CreateCategory"/> method returns an existing category when it already exists.
        /// </summary>
        [TestMethod]
        public void CreateCategory_ShouldReturnExistingCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryName = "ExistingCategory";
            var existingCategory = new Category(categoryName);

            this.categories[categoryName] = existingCategory;
            this.categoryDAOMock.Setup(dao => dao.GetByName(categoryName)).Returns(existingCategory);
            this.categoryDAOMock.Setup(dao => dao.Add(It.IsAny<Category>())).Verifiable();

            // Act
            var category = this.categoryService.CreateCategory(categoryName);

            // Assert
            Assert.AreSame(existingCategory, category);
            this.categoryDAOMock.Verify(dao => dao.Add(It.IsAny<Category>()), Times.Never);
            Assert.IsTrue(this.categories.ContainsKey(categoryName));
        }

        /// <summary>
        /// Tests that the <see cref="CategoryService.CreateCategory"/> method throws an <see cref="ArgumentException"/> when the category name is null or empty.
        /// </summary>
        [TestMethod]
        public void CreateCategory_ShouldThrowArgumentException_WhenCategoryNameIsNullOrEmpty()
        {
            // Arrange
            string nullName = null;
            string emptyName = string.Empty;

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => this.categoryService.CreateCategory(nullName));
            Assert.ThrowsException<ArgumentException>(() => this.categoryService.CreateCategory(emptyName));
        }
    }
}
