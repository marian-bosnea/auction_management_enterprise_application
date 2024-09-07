// <copyright file="ProductTest.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the <see cref="Product"/> class.
    /// </summary>
    [TestClass]
    public class ProductTest
    {
        /// <summary>
        /// Tests that the constructor correctly initializes a product with valid parameters.
        /// </summary>
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

        /// <summary>
        /// Tests that validating a product with an empty name throws a <see cref="ValidationException"/>.
        /// </summary>
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

        /// <summary>
        /// Tests that validating a product with a name longer than 200 characters throws a <see cref="ValidationException"/>.
        /// </summary>
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

        /// <summary>
        /// Tests that validating a product with an empty description throws a <see cref="ValidationException"/>.
        /// </summary>
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

        /// <summary>
        /// Tests that validating a product with a description longer than 1000 characters throws a <see cref="ValidationException"/>.
        /// </summary>
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

        /// <summary>
        /// Tests that adding a valid category to a product increases the number of categories.
        /// </summary>
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

        /// <summary>
        /// Tests that adding a category that already exists in the product's category list does not add a duplicate.
        /// </summary>
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

        /// <summary>
        /// Tests that the <see cref="Product.ToString"/> method returns the product's name.
        /// </summary>
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
