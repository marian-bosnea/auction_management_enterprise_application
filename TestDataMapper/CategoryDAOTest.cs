// <copyright file="CategoryDAOTest.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper.Tests
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using DataMapper.DAO;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the <see cref="CategoryDAO"/> class.
    /// </summary>
    [TestClass]
    public class CategoryDAOTest
    {
        /// <summary>
        /// Represents the database context used for accessing the database.
        /// </summary>
        private DbContext context;

        /// <summary>
        /// Represents the data access object (DAO) for managing categories.
        /// Provides methods for interacting with category-related data in the database.
        /// </summary>
        private CategoryDAO categoryDAO;

        /// <summary>
        /// Initializes the test environment before each test method is run.
        /// This includes setting up an in-memory database and initializing DAOs.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Create an in-memory database using Effort
            var connection = Effort.DbConnectionFactory.CreateTransient();
            this.context = new AuctionManagementEfCoreDbContext(connection);

            // Initialize DAO with the in-memory context
            this.categoryDAO = new CategoryDAO((AuctionManagementEfCoreDbContext)this.context);

            // Seed initial data
            this.SeedDatabase();
        }

        /// <summary>
        /// Tests that the <see cref="CategoryDAO.Add"/> method correctly adds a new category to the database.
        /// </summary>
        [TestMethod]
        public void AddCategory_ShouldAddCategoryToDatabase()
        {
            // Arrange
            var newCategory = new Category("Electronics");

            // Act
            this.categoryDAO.Add(newCategory);

            // Assert
            var addedCategory = this.context.Set<Category>().Find(newCategory.Id);
            Assert.IsNotNull(addedCategory);
            Assert.AreEqual("Electronics", addedCategory.Name);
        }

        /// <summary>
        /// Tests that the <see cref="CategoryDAO.Update"/> method correctly updates an existing category in the database.
        /// </summary>
        [TestMethod]
        public void UpdateCategory_ShouldUpdateCategoryInDatabase()
        {
            // Arrange
            var categoryToAdd = new Category("Electronics");
            this.categoryDAO.Add(categoryToAdd);

            // Modify the category
            categoryToAdd.Name = "Updated Electronics";

            // Act
            this.categoryDAO.Update(categoryToAdd);

            // Assert
            var updatedCategory = this.context.Set<Category>().Find(categoryToAdd.Id);
            Assert.IsNotNull(updatedCategory);
            Assert.AreEqual("Updated Electronics", updatedCategory.Name);
        }

        /// <summary>
        /// Tests that the <see cref="CategoryDAO.Delete"/> method correctly removes a category from the database.
        /// </summary>
        [TestMethod]
        public void DeleteCategory_ShouldRemoveCategoryFromDatabase()
        {
            // Arrange
            var categoryToDelete = new Category("Electronics");
            this.categoryDAO.Add(categoryToDelete);

            // Act
            this.categoryDAO.Delete(categoryToDelete.Id);

            // Assert
            var deletedCategory = this.context.Set<Category>().Find(categoryToDelete.Id);
            Assert.IsNull(deletedCategory);
        }

        /// <summary>
        /// Tests that the <see cref="CategoryDAO.Get"/> method correctly retrieves a category by its ID from the database.
        /// </summary>
        [TestMethod]
        public void GetCategory_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedCategory = new Category("Electronics");
            this.categoryDAO.Add(expectedCategory);

            // Act
            var category = this.categoryDAO.Get(expectedCategory.Id);

            // Assert
            Assert.IsNotNull(category);
            Assert.AreEqual("Electronics", category.Name);
        }

        /// <summary>
        /// Tests that the <see cref="CategoryDAO.GetByName"/> method correctly retrieves a category by its name from the database.
        /// </summary>
        [TestMethod]
        public void GetCategoryByName_ShouldReturnCorrectCategory()
        {
            // Arrange
            var categoryToFind = new Category("Electronics");
            this.categoryDAO.Add(categoryToFind);

            // Act
            var foundCategory = this.categoryDAO.GetByName("Electronics");

            // Assert
            Assert.IsNotNull(foundCategory);
            Assert.AreEqual("Electronics", foundCategory.Name);
        }

        /// <summary>
        /// Seeds the in-memory database with initial category data required for testing.
        /// </summary>
        private void SeedDatabase()
        {
            var initialCategories = new List<Category>
            {
                new Category("Electronics"),
                new Category("Furniture"),
            };

            foreach (var category in initialCategories)
            {
                this.categoryDAO.Add(category);
            }
        }
    }
}
