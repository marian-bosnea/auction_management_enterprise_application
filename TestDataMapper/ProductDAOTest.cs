// <copyright file="ProductDAOTest.cs" company="Transilvania University of Brasov">
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
    /// Unit tests for the <see cref="ProductDAO"/> class.
    /// </summary>
    [TestClass]
    public class ProductDAOTest
    {
        /// <summary>
        /// Represents the database context used for interacting with the database in the tests.
        /// </summary>
        /// <remarks>
        /// This field is typically initialized with an instance of <see cref="DbContext"/> and is used to manage the database connection
        /// and perform CRUD operations in the tests.
        /// </remarks>
        private DbContext context;

        /// <summary>
        /// Represents the data access object (DAO) for managing operations related to <see cref="Product"/> entities in the tests.
        /// </summary>
        /// <remarks>
        /// This field is typically initialized with an instance of <see cref="ProductDAO"/> and is used to perform database operations such as
        /// adding, updating, deleting, and retrieving <see cref="Product"/> entities.
        /// </remarks>
        private ProductDAO productDAO;

        /// <summary>
        /// Initializes the test environment before each test method is run.
        /// This includes setting up an in-memory database and initializing DAOs.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Create an in-memory database using Effort or any suitable provider
            var connection = Effort.DbConnectionFactory.CreateTransient();
            this.context = new AuctionManagementEfCoreDbContext(connection);

            // Initialize DAO with the in-memory context
            this.productDAO = new ProductDAO((AuctionManagementEfCoreDbContext)this.context);

            // Seed initial data
            this.SeedDatabase();
        }

        /// <summary>
        /// Tests that the <see cref="ProductDAO.Add"/> method correctly adds a new product to the database.
        /// </summary>
        [TestMethod]
        public void AddProduct_ShouldAddProductToDatabase()
        {
            // Arrange
            var newProduct = new Product(1, "New Product", "Description of New Product", new List<Category>());

            // Act
            this.productDAO.Add(newProduct);

            // Assert
            var addedProduct = this.context.Set<Product>().Find(newProduct.Id);
            Assert.IsNotNull(addedProduct);
            Assert.AreEqual("New Product", addedProduct.Name);
            Assert.AreEqual("Description of New Product", addedProduct.Description);
            Assert.AreEqual(0, addedProduct.Categories.Count);
        }

        /// <summary>
        /// Tests that the <see cref="ProductDAO.Delete"/> method correctly removes a product from the database.
        /// </summary>
        [TestMethod]
        public void DeleteProduct_ShouldRemoveProductFromDatabase()
        {
            // Arrange
            var productToDelete = new Product(1, "Product to Delete", "Description", new List<Category>());
            this.productDAO.Add(productToDelete);

            // Act
            this.productDAO.Delete(productToDelete.Id);

            // Assert
            var deletedProduct = this.context.Set<Product>().Find(productToDelete.Id);
            Assert.IsNull(deletedProduct);
        }

        /// <summary>
        /// Tests that the <see cref="ProductDAO.Get"/> method correctly retrieves a product by its ID from the database.
        /// </summary>
        [TestMethod]
        public void GetProduct_ShouldReturnCorrectProduct()
        {
            // Arrange
            var expectedProduct = new Product(1, "Expected Product", "Description", new List<Category>());
            this.productDAO.Add(expectedProduct);

            // Act
            var product = this.productDAO.Get(expectedProduct.Id);

            // Assert
            Assert.IsNotNull(product);
            Assert.AreEqual("Expected Product", product.Name);
            Assert.AreEqual("Description", product.Description);
        }

        /// <summary>
        /// Seeds the in-memory database with initial product data required for testing.
        /// </summary>
        private void SeedDatabase()
        {
            var initialProducts = new List<Product>
            {
                new Product(1, "Seed Product 1", "Description of Seed Product 1", new List<Category>()),
                new Product(2, "Seed Product 2", "Description of Seed Product 2", new List<Category>()),
            };

            foreach (var product in initialProducts)
            {
                this.productDAO.Add(product);
            }
        }
    }
}
