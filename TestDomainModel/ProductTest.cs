// <copyright file="ProductTest.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel.Tests
{
    using System;
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
        [TestMethod]
        public void DefaultConstructor_ShouldInitializePropertiesToDefaultValues()
        {
            // Arrange & Act
            var product = new Product();

            // Assert
            Assert.AreEqual(0, product.Id, "Product Id should default to 0.");
            Assert.IsNull(product.Name, "Product Name should default to null.");
            Assert.IsNull(product.Description, "Product Description should default to null.");
        }

        [TestMethod]
        public void DefaultConstructor_ShouldNotThrowExceptions()
        {
            // Arrange & Act
            // No parameters are needed for this test.

            // Assert
            try
            {
                var product = new Product();
                Assert.IsNotNull(product, "Product instance should be created successfully.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Default constructor threw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void Constructor_WithParameters_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var id = 1;
            var name = "Test Product";
            var description = "Test Description";
            var categories = new List<Category>
            {
                new Category("Category1"),
                new Category("Category2"),
            };

            // Act
            var product = new Product(id, name, description, categories);

            // Assert
            Assert.AreEqual(id, product.Id, "Product Id should be initialized correctly.");
            Assert.AreEqual(name, product.Name, "Product Name should be initialized correctly.");
            Assert.AreEqual(description, product.Description, "Product Description should be initialized correctly.");
            Assert.AreEqual(categories.Count, product.Categories.Count, "Product Categories should be initialized correctly.");
            for (int i = 0; i < categories.Count; i++)
            {
                Assert.AreEqual(categories[i], product.Categories[i], "Product Categories should match the initialized categories.");
            }
        }

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

        /// <summary>
        /// Tests that a new category is added to the product's categories list.
        /// </summary>
        [TestMethod]
        public void AddCategory_NewCategory_CategoryAdded()
        {
            // Arrange
            var product = new Product(1, "Product A", "Description", new List<Category>());
            var newCategory = new Category("New Category");

            // Act
            product.AddCategory(newCategory);

            // Assert
            Assert.IsTrue(product.Categories.Contains(newCategory));
        }

        /// <summary>
        /// Tests that an existing category is not added again to the product's categories list.
        /// </summary>
        [TestMethod]
        public void AddCategory_ExistingCategory_CategoryNotAddedAgain()
        {
            // Arrange
            var existingCategory = new Category("Existing Category");
            var product = new Product(1, "Product A", "Description", new List<Category> { existingCategory });

            // Act
            product.AddCategory(existingCategory);

            // Assert
            var categories = product.Categories;
            Assert.AreEqual(1, categories.Count); // Should still have only one category
        }

        /// <summary>
        /// Tests that adding a null category does not throw an exception and does not modify the categories list.
        /// </summary>
        [TestMethod]
        public void AddCategory_NullCategory_DoesNotThrowException()
        {
            // Arrange
            var product = new Product(1, "Product A", "Description", new List<Category>());

            // Act
            try
            {
                product.AddCategory(null);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception was thrown: {ex.Message}");
            }

            // Assert
            Assert.AreEqual(0, product.Categories.Count); // The list should still be empty
        }
    }
}
