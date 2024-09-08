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
        private Mock<ICategoryDAO> categoryDAOMock;
        private CategoryService categoryService;
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
            string emptyName = "";

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => this.categoryService.CreateCategory(nullName));
            Assert.ThrowsException<ArgumentException>(() => this.categoryService.CreateCategory(emptyName));
        }
    }
}
