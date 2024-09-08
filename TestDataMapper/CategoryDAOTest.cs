using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataMapper.DAO;
using DomainModel;

namespace DataMapper.Tests
{
    [TestClass]
    public class CategoryDAOTests
    {
        private DbContext _context;
        private CategoryDAO _categoryDAO;

        [TestInitialize]
        public void Setup()
        {
            // Create an in-memory database using Effort
            var connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new AuctionManagementEfCoreDbContext(connection);

            // Initialize DAO with the in-memory context
            _categoryDAO = new CategoryDAO((AuctionManagementEfCoreDbContext)_context);

            // Seed initial data
            SeedDatabase();
        }

        [TestMethod]
        public void AddCategory_ShouldAddCategoryToDatabase()
        {
            // Arrange
            var newCategory = new Category("Electronics");

            // Act
            _categoryDAO.Add(newCategory);

            // Assert
            var addedCategory = _context.Set<Category>().Find(newCategory.Id);
            Assert.IsNotNull(addedCategory);
            Assert.AreEqual("Electronics", addedCategory.Name);
        }

        [TestMethod]
        public void UpdateCategory_ShouldUpdateCategoryInDatabase()
        {
            // Arrange
            var categoryToAdd = new Category("Electronics");
            _categoryDAO.Add(categoryToAdd);

            // Modify the category
            categoryToAdd.Name = "Updated Electronics";

            // Act
            _categoryDAO.Update(categoryToAdd);

            // Assert
            var updatedCategory = _context.Set<Category>().Find(categoryToAdd.Id);
            Assert.IsNotNull(updatedCategory);
            Assert.AreEqual("Updated Electronics", updatedCategory.Name);
        }

        [TestMethod]
        public void DeleteCategory_ShouldRemoveCategoryFromDatabase()
        {
            // Arrange
            var categoryToDelete = new Category("Electronics");
            _categoryDAO.Add(categoryToDelete);

            // Act
            _categoryDAO.Delete(categoryToDelete.Id);

            // Assert
            var deletedCategory = _context.Set<Category>().Find(categoryToDelete.Id);
            Assert.IsNull(deletedCategory);
        }

        [TestMethod]
        public void GetCategory_ShouldReturnCorrectCategory()
        {
            // Arrange
            var expectedCategory = new Category("Electronics");
            _categoryDAO.Add(expectedCategory);

            // Act
            var category = _categoryDAO.Get(expectedCategory.Id);

            // Assert
            Assert.IsNotNull(category);
            Assert.AreEqual("Electronics", category.Name);
        }

        [TestMethod]
        public void GetCategoryByName_ShouldReturnCorrectCategory()
        {
            // Arrange
            var categoryToFind = new Category("Electronics");
            _categoryDAO.Add(categoryToFind);

            // Act
            var foundCategory = _categoryDAO.GetByName("Electronics");

            // Assert
            Assert.IsNotNull(foundCategory);
            Assert.AreEqual("Electronics", foundCategory.Name);
        }

        private void SeedDatabase()
        {
            var initialCategories = new List<Category>
            {
                new Category("Electronics"),
                new Category("Furniture")
            };

            foreach (var category in initialCategories)
            {
                _categoryDAO.Add(category);
            }
        }
    }
}