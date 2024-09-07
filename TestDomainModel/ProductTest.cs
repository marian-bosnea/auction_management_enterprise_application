using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Tests
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void Constructor_ValidParameters_ShouldInitializeProductCorrectly()
        {
            // Arrange
            int id = 1;
            string name = "Laptop";
            string description = "A high-end gaming laptop.";
            var categories = new List<Category> { new Category("Electronics") };

            // Act
            var product = new Product(id, name, description, categories);

            // Assert
            Assert.AreEqual(id, product.Id);
            Assert.AreEqual(name, product.Name);
            Assert.AreEqual(description, product.Description);
            Assert.AreEqual(categories, product.Categories);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateProduct_EmptyName_ShouldThrowValidationException()
        {
            // Arrange
            var categories = new List<Category> { new Category("Electronics") };
            var product = new Product(1, string.Empty, "Description", categories);

            // Act
            var context = new ValidationContext(product);
            Validator.ValidateObject(product, context, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateProduct_NameTooLong_ShouldThrowValidationException()
        {
            // Arrange
            string longName = new string('a', 201); // Name longer than 200 characters
            var categories = new List<Category> { new Category("Electronics") };
            var product = new Product(1, longName, "Description", categories);

            // Act
            var context = new ValidationContext(product);
            Validator.ValidateObject(product, context, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateProduct_EmptyDescription_ShouldThrowValidationException()
        {
            // Arrange
            var categories = new List<Category> { new Category("Electronics") };
            var product = new Product(1, "Laptop", string.Empty, categories);

            // Act
            var context = new ValidationContext(product);
            Validator.ValidateObject(product, context, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateProduct_DescriptionTooLong_ShouldThrowValidationException()
        {
            // Arrange
            string longDescription = new string('a', 1001); // Description longer than 1000 characters
            var categories = new List<Category> { new Category("Electronics") };
            var product = new Product(1, "Laptop", longDescription, categories);

            // Act
            var context = new ValidationContext(product);
            Validator.ValidateObject(product, context, true);
        }

        [TestMethod]
        public void AddCategory_ValidCategory_ShouldAddCategory()
        {
            // Arrange
            var category = new Category("Gaming");
            var categories = new List<Category> { new Category("Electronics") };
            var product = new Product(1, "Laptop", "A high-end gaming laptop.", categories);

            // Act
            product.AddCategory(category);

            // Assert
            Assert.AreEqual(2, product.Categories.Count);
            Assert.IsTrue(product.Categories.Contains(category));
        }

        [TestMethod]
        public void AddCategory_CategoryAlreadyExists_ShouldNotAddDuplicateCategory()
        {
            // Arrange
            var category = new Category("Electronics");
            var categories = new List<Category> { category };
            var product = new Product(1, "Laptop", "A high-end gaming laptop.", categories);

            // Act
            product.AddCategory(category); // Adding the same category again

            // Assert
            Assert.AreEqual(1, product.Categories.Count);
        }

        [TestMethod]
        public void ToString_ShouldReturnProductName()
        {
            // Arrange
            var categories = new List<Category> { new Category("Electronics") };
            var product = new Product(1, "Laptop", "A high-end gaming laptop.", categories);

            // Act
            string result = product.ToString();

            // Assert
            Assert.AreEqual("Laptop", result);
        }
    }
}
