using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataMapper.DAO;
using DomainModel;

namespace DataMapper.Tests
{
    [TestClass]
    public class ProductDAOTests
    {
        private DbContext _context;
        private ProductDAO _productDAO;

        [TestInitialize]
        public void Setup()
        {
            // Create an in-memory database using Effort or any suitable provider
            var connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new AuctionManagementEfCoreDbContext(connection);

            // Initialize DAO with the in-memory context
            _productDAO = new ProductDAO((AuctionManagementEfCoreDbContext)_context);

            // Seed initial data
            SeedDatabase();
        }

        [TestMethod]
        public void AddProduct_ShouldAddProductToDatabase()
        {
            // Arrange
            var newProduct = new Product(1, "New Product", "Description of New Product", new List<Category>());

            // Act
            _productDAO.Add(newProduct);

            // Assert
            var addedProduct = _context.Set<Product>().Find(newProduct.Id);
            Assert.IsNotNull(addedProduct);
            Assert.AreEqual("New Product", addedProduct.Name);
            Assert.AreEqual("Description of New Product", addedProduct.Description);
            Assert.AreEqual(0, addedProduct.Categories.Count);
        }

        [TestMethod]
        public void DeleteProduct_ShouldRemoveProductFromDatabase()
        {
            // Arrange
            var productToDelete = new Product(1, "Product to Delete", "Description", new List<Category>());
            _productDAO.Add(productToDelete);

            // Act
            _productDAO.Delete(productToDelete.Id);

            // Assert
            var deletedProduct = _context.Set<Product>().Find(productToDelete.Id);
            Assert.IsNull(deletedProduct);
        }

        [TestMethod]
        public void GetProduct_ShouldReturnCorrectProduct()
        {
            // Arrange
            var expectedProduct = new Product(1, "Expected Product", "Description", new List<Category>());
            _productDAO.Add(expectedProduct);

            // Act
            var product = _productDAO.Get(expectedProduct.Id);

            // Assert
            Assert.IsNotNull(product);
            Assert.AreEqual("Expected Product", product.Name);
            Assert.AreEqual("Description", product.Description);
        }

        private void SeedDatabase()
        {
            var initialProducts = new List<Product>
            {
                new Product(1, "Seed Product 1", "Description of Seed Product 1", new List<Category>()),
                new Product(2, "Seed Product 2", "Description of Seed Product 2", new List<Category>())
            };

            foreach (var product in initialProducts)
            {
                _productDAO.Add(product);
            }
        }
    }
}
