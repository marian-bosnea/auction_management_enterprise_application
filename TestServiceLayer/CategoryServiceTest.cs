namespace TestServiceLayer
{
    using System;
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
        /// Tests that the <see cref="CategoryService.CreateCategory"/> method creates and caches a new category when it does not already exist.
        /// </summary>
        [TestMethod]
        public void CreateCategory_ShouldCreateAndCacheNewCategory_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryDAO = new Mock<ICategoryDAO>();
            var categoryService = new CategoryService(categoryDAO.Object);
            string categoryName = "NewCategory";

            categoryDAO.Setup(dao => dao.GetByName(categoryName)).Returns((Category)null);
            categoryDAO.Setup(dao => dao.Add(It.IsAny<Category>()));

            // Act
            var category = categoryService.CreateCategory(categoryName);

            // Assert
            Assert.IsNotNull(category);
            Assert.AreEqual(categoryName, category.Name);
            categoryDAO.Verify(dao => dao.Add(It.IsAny<Category>()), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="CategoryService.CreateCategory"/> method returns an existing category when it already exists.
        /// </summary>
        [TestMethod]
        public void CreateCategory_ShouldReturnExistingCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryDAO = new Mock<ICategoryDAO>();
            var categoryService = new CategoryService(categoryDAO.Object);
            string categoryName = "ExistingCategory";
            var existingCategory = new Category(categoryName);

            categoryDAO.Setup(dao => dao.GetByName(categoryName)).Returns(existingCategory);
            categoryDAO.Setup(dao => dao.Add(It.IsAny<Category>())).Verifiable();

            // Act
            var category = categoryService.CreateCategory(categoryName);

            // Assert
            Assert.AreSame(existingCategory, category);
            categoryDAO.Verify(dao => dao.Add(It.IsAny<Category>()), Times.Never);
            Assert.IsTrue(categoryService.Categories.ContainsKey(categoryName));
        }

        /// <summary>
        /// Tests that the <see cref="CategoryService.CreateCategory"/> method creates a new category when the DAO returns null.
        /// </summary>
        [TestMethod]
        public void CreateCategory_ShouldCreateCategory_WhenDAOReturnsNull()
        {
            // Arrange
            var categoryDAO = new Mock<ICategoryDAO>();
            var categoryService = new CategoryService(categoryDAO.Object);
            string categoryName = "CategoryToCreate";

            categoryDAO.Setup(dao => dao.GetByName(categoryName)).Returns((Category)null);
            categoryDAO.Setup(dao => dao.Add(It.IsAny<Category>()));

            // Act
            var category = categoryService.CreateCategory(categoryName);

            // Assert
            Assert.IsNotNull(category);
            Assert.AreEqual(categoryName, category.Name);
            categoryDAO.Verify(dao => dao.Add(It.IsAny<Category>()), Times.Once);
            Assert.IsTrue(categoryService.Categories.ContainsKey(categoryName));
        }

        /// <summary>
        /// Tests that the <see cref="CategoryService.CreateCategory"/> method caches the category when it is created.
        /// </summary>
        [TestMethod]
        public void CreateCategory_ShouldCacheCategory_WhenCreated()
        {
            // Arrange
            var categoryDAO = new Mock<ICategoryDAO>();
            var categoryService = new CategoryService(categoryDAO.Object);
            string categoryName = "CachedCategory";
            var newCategory = new Category(categoryName);

            categoryDAO.Setup(dao => dao.GetByName(categoryName)).Returns(newCategory);
            categoryDAO.Setup(dao => dao.Add(It.IsAny<Category>()));

            // Act
            var category1 = categoryService.CreateCategory(categoryName);
            var category2 = categoryService.CreateCategory(categoryName);

            // Assert
            Assert.AreSame(category1, category2);
            Assert.IsTrue(categoryService.Categories.ContainsKey(categoryName));
        }
    }
}
