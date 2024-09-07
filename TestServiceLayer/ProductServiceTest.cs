using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DomainModel;
using ServiceLayer.Services;
using ServiceLayer.Interfaces;
using DataMapper.Interfaces;

[TestClass]
public class ProductServiceTests
{
    private Mock<IProductDAO> productDAO;
    private Mock<ICategoryService> categoryService;
    private ProductService productService;

    [TestInitialize]
    public void SetUp()
    {
        this.productDAO = new Mock<IProductDAO>();
        this.categoryService = new Mock<ICategoryService>();
        this.productService = new ProductService(this.productDAO.Object, this.categoryService.Object);
    }

    [TestMethod]
    public void CreateProduct_ShouldCreateAndAddProduct_WhenNoSimilarProductExists()
    {
        // Arrange
        string name = "NewProduct";
        string description = "This is a new product.";
        var categoryNames = new List<string> { "Category1", "Category2" };
        var categories = new List<Category>
        {
            new Category("Category1"),
            new Category("Category2")
        };
        var expectedProduct = new Product(0, name, description, categories);

        this.categoryService.Setup(cs => cs.CreateCategory(It.IsAny<string>())).Returns((string catName) =>
            categories.First(cat => cat.Name == catName));
        this.productDAO.Setup(dao => dao.Add(It.IsAny<Product>()));

        // Act
        var product = this.productService.CreateProduct(name, description, categoryNames);

        // Assert
        Assert.IsNotNull(product);
        Assert.AreEqual(name, product.Name);
        Assert.AreEqual(description, product.Description);
        Assert.AreEqual(2, product.Categories.Count);
        Assert.IsTrue(this.productService.Products.Contains(product));
        this.productDAO.Verify(dao => dao.Add(It.IsAny<Product>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CreateProduct_ShouldThrowException_WhenSimilarProductExists()
    {
        // Arrange
        string name = "ExistingProduct";
        string description = "This is an existing product.";
        var existingProduct = new Product(1, name, description, new List<Category>());
        this.productService.Products.Add(existingProduct);

        string newProductDescription = "This is an existing product.";
        var categoryNames = new List<string>();

        // Act
        this.productService.CreateProduct(name, newProductDescription, categoryNames);
    }

    [TestMethod]
    public void AddProduct_ShouldAddProductToCollectionAndDAO()
    {
        // Arrange
        var product = new Product(1, "ProductName", "ProductDescription", new List<Category>());

        // Act
        this.productService.AddProduct(product);

        // Assert
        Assert.IsTrue(this.productService.Products.Contains(product));
        this.productDAO.Verify(dao => dao.Add(product), Times.Once);
    }

    [TestMethod]
    public void GetProductById_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product(1, "ProductName", "ProductDescription", new List<Category>());
        this.productService.Products.Add(product);

        // Act
        var retrievedProduct = this.productService.GetProductById(1);

        // Assert
        Assert.IsNotNull(retrievedProduct);
        Assert.AreEqual(product, retrievedProduct);
    }

    [TestMethod]
    public void GetProductById_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Act
        var retrievedProduct = this.productService.GetProductById(999);

        // Assert
        Assert.IsNull(retrievedProduct);
    }

    [TestMethod]
    public void DeleteProduct_ShouldRemoveProductFromCollection()
    {
        // Arrange
        var product = new Product(1, "ProductName", "ProductDescription", new List<Category>());
        this.productService.Products.Add(product);

        // Act
        this.productService.DeleteProduct(product);

        // Assert
        Assert.IsFalse(this.productService.Products.Contains(product));
    }

    [TestMethod]
    public void ToString_ShouldReturnCorrectRepresentation()
    {
        // Arrange
        var category = new Category("Category1");
        this.productService.Categories.Add(category.Name, category);
        var product = new Product(1, "ProductName", "ProductDescription", new List<Category> { category });
        this.productService.Products.Add(product);

        // Act
        var result = this.productService.ToString();

        // Assert
        Assert.IsTrue(result.Contains("Categories: Category1"));
        Assert.IsTrue(result.Contains("Products: ProductName"));
    }

    [TestMethod]
    public void GetSimilarityThresholdFromConfig_ShouldReturnDefault_WhenConfigValueInvalid()
    {
        // Arrange
        ConfigurationManager.AppSettings["SimilarityThreshold"] = "Invalid";

        // Act
        var threshold = this.productService.GetSimilarityThresholdFromConfig();

        // Assert
        Assert.AreEqual(ProductService.DefaultSimilarityThreshold, threshold);
    }

    [TestMethod]
    public void GetSimilarityThresholdFromConfig_ShouldReturnConfigValue_WhenValid()
    {
        // Arrange
        ConfigurationManager.AppSettings["SimilarityThreshold"] = "7";

        // Act
        var threshold = this.productService.GetSimilarityThresholdFromConfig();

        // Assert
        Assert.AreEqual(7, threshold);
    }

    [TestMethod]
    public void GetAllProducts_ShouldReturnAllProductsFromDAO()
    {
        // Arrange
        var productsFromDAO = new List<Product>
        {
            new Product(1, "Product1", "Description1", new List<Category>()),
            new Product(2, "Product2", "Description2", new List<Category>())
        };
        this.productDAO.Setup(dao => dao.GetAll()).Returns(productsFromDAO);

        // Act
        var products = this.productService.GetAllProducts();

        // Assert
        Assert.IsNotNull(products);
        Assert.AreEqual(2, products.Count);
        Assert.AreEqual("Product1", products[0].Name);
        Assert.AreEqual("Product2", products[1].Name);
        this.productDAO.Verify(dao => dao.GetAll(), Times.Once);
    }

    [TestMethod]
    public void UpdateProduct_ShouldNotAddProduct_WhenProductDoesNotExistInCollection()
    {
        // Arrange
        var nonExistingProduct = new Product(999, "NonExistingName", "NonExistingDescription", new List<Category>());
        var updatedProduct = new Product(999, "NewName", "NewDescription", new List<Category>());
        this.productDAO.Setup(dao => dao.Update(updatedProduct));

        // Act
        this.productService.UpdateProduct(updatedProduct);

        // Assert
        Assert.IsFalse(this.productService.Products.Contains(nonExistingProduct));
        this.productDAO.Verify(dao => dao.Update(updatedProduct), Times.Once);
    }
}
