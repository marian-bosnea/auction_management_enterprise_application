// <copyright file="CategoryTest.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel.Tests
{
    using System;
    using System.Collections.Generic;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the <see cref="Category"/> class.
    /// </summary>
    [TestClass]
    public class CategoryTest
    {
        /// <summary>
        /// Tests that the <see cref="Category"/> constructor correctly initializes a category with a valid name.
        /// </summary>
        [TestMethod]
        public void Constructor_ValidName_ShouldInitializeCategoryCorrectly()
        {
            // Arrange
            string name = "Electronics";

            // Act
            var category = new Category(name);

            // Assert
            Assert.AreEqual(name, category.Name);
            Assert.IsNotNull(category.Parents);
            Assert.IsNotNull(category.Subcategories);
            Assert.AreEqual(0, category.Parents.Count);
            Assert.AreEqual(0, category.Subcategories.Count);
        }

        /// <summary>
        /// Tests that adding a valid parent category to a category correctly updates both categories.
        /// </summary>
        [TestMethod]
        public void AddParent_ValidParent_ShouldAddParentAndAddCurrentToParentSubcategories()
        {
            // Arrange
            var parentCategory = new Category("Parent Category");
            var childCategory = new Category("Child Category");

            // Act
            childCategory.AddParent(parentCategory);

            // Assert
            Assert.AreEqual(1, childCategory.Parents.Count);
            Assert.AreEqual(parentCategory, childCategory.Parents[0]);
            Assert.AreEqual(1, parentCategory.Subcategories.Count);
            Assert.AreEqual(childCategory, parentCategory.Subcategories[0]);
        }

        /// <summary>
        /// Tests that adding a valid subcategory to a parent category correctly updates both categories.
        /// </summary>
        [TestMethod]
        public void AddSubcategory_ValidSubcategory_ShouldAddSubcategoryAndAddCurrentToSubcategoryParents()
        {
            // Arrange
            var parentCategory = new Category("Parent Category");
            var subCategory = new Category("Subcategory");

            // Act
            parentCategory.AddSubcategory(subCategory);

            // Assert
            Assert.AreEqual(1, parentCategory.Subcategories.Count);
            Assert.AreEqual(subCategory, parentCategory.Subcategories[0]);
            Assert.AreEqual(1, subCategory.Parents.Count);
            Assert.AreEqual(parentCategory, subCategory.Parents[0]);
        }

        /// <summary>
        /// Tests that adding the same parent category more than once does not create duplicates.
        /// </summary>
        [TestMethod]
        public void AddParent_ParentAlreadyExists_ShouldNotAddDuplicateParent()
        {
            // Arrange
            var parentCategory = new Category("Parent Category");
            var childCategory = new Category("Child Category");

            // Act
            childCategory.AddParent(parentCategory);
            childCategory.AddParent(parentCategory); // Adding the same parent again

            // Assert
            Assert.AreEqual(1, childCategory.Parents.Count);
            Assert.AreEqual(1, parentCategory.Subcategories.Count);
        }

        /// <summary>
        /// Tests that adding the same subcategory more than once does not create duplicates.
        /// </summary>
        [TestMethod]
        public void AddSubcategory_SubcategoryAlreadyExists_ShouldNotAddDuplicateSubcategory()
        {
            // Arrange
            var parentCategory = new Category("Parent Category");
            var subCategory = new Category("Subcategory");

            // Act
            parentCategory.AddSubcategory(subCategory);
            parentCategory.AddSubcategory(subCategory); // Adding the same subcategory again

            // Assert
            Assert.AreEqual(1, parentCategory.Subcategories.Count);
            Assert.AreEqual(1, subCategory.Parents.Count);
        }

        /// <summary>
        /// Tests that the <see cref="Category.ToString"/> method returns the category name.
        /// </summary>
        [TestMethod]
        public void ToString_ShouldReturnName()
        {
            // Arrange
            var category = new Category("Books");

            // Act
            string result = category.ToString();

            // Assert
            Assert.AreEqual("Books", result);
        }

        /// <summary>
        /// Tests that the <see cref="Category"/> constructor throws an <see cref="ArgumentNullException"/> when the name is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullName_ShouldThrowArgumentNullException()
        {
            // Act
            var category = new Category(null);
        }

        /// <summary>
        /// Tests that the <see cref="Category"/> constructor throws an <see cref="ArgumentException"/> when the name is empty.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EmptyName_ShouldThrowArgumentException()
        {
            // Act
            var category = new Category(string.Empty);
        }

        /// <summary>
        /// Tests that the <see cref="Category"/> constructor correctly handles a name that contains only whitespace.
        /// </summary>
        [TestMethod]
        public void Constructor_WhitespaceName_ShouldAllowWhitespaceName()
        {
            // Arrange
            string name = " ";

            // Act
            var category = new Category(name);

            // Assert
            Assert.AreEqual(name, category.Name);
        }

        /// <summary>
        /// Tests that the <see cref="Category"/> constructor initializes empty lists for parents and subcategories.
        /// </summary>
        [TestMethod]
        public void Constructor_ValidName_ShouldInitializeEmptyParentsAndSubcategories()
        {
            // Arrange
            string name = "Books";

            // Act
            var category = new Category(name);

            // Assert
            Assert.IsNotNull(category.Parents);
            Assert.IsNotNull(category.Subcategories);
            Assert.AreEqual(0, category.Parents.Count);
            Assert.AreEqual(0, category.Subcategories.Count);
        }
    }
}
