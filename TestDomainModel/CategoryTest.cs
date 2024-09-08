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

        /// <summary>
        /// Tests that the ToString method returns the expected category name.
        /// </summary>
        [TestMethod]
        public void ToString_ReturnsCategoryName()
        {
            // Arrange
            var expectedName = "Electronics";
            var category = new Category(expectedName);

            // Act
            var result = category.ToString();

            // Assert
            Assert.AreEqual(expectedName, result, "ToString did not return the expected category name.");
        }

        /// <summary>
        /// Tests that the default constructor of the <see cref="Category"/> class initializes empty lists for parents and subcategories.
        /// </summary>
        [TestMethod]
        public void Constructor_ShouldInitializeEmptyListsForParentsAndSubcategories()
        {
            // Arrange & Act
            var category = new Category();

            // Assert
            Assert.IsNotNull(category.Parents, "Parents list should not be null.");
            Assert.IsNotNull(category.Subcategories, "Subcategories list should not be null.");
            Assert.AreEqual(0, category.Parents.Count, "Parents list should be empty.");
            Assert.AreEqual(0, category.Subcategories.Count, "Subcategories list should be empty.");
        }

        /// <summary>
        /// Tests that the constructor of the <see cref="Category"/> class, when provided with a name, initializes the properties correctly.
        /// </summary>
        /// <remarks>
        /// Verifies that the Name property is set to the provided name and that the Parents and Subcategories lists are not null and empty.
        /// </remarks>
        [TestMethod]
        public void Constructor_WithParameters_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var name = "Test Category";
            var category = new Category(name);

            // Act
            // Assert
            Assert.AreEqual(name, category.Name, "Category name should be set correctly.");
            Assert.IsNotNull(category.Parents, "Parents list should not be null.");
            Assert.IsNotNull(category.Subcategories, "Subcategories list should not be null.");
            Assert.AreEqual(0, category.Parents.Count, "Parents list should be empty.");
            Assert.AreEqual(0, category.Subcategories.Count, "Subcategories list should be empty.");
        }

        /// <summary>
        /// Tests that the <see cref="AddParent"/> method correctly adds a parent category to the child's Parents list and updates the parent's Subcategories list.
        /// </summary>
        /// <remarks>
        /// Verifies that the child category has one parent and the parent category has one subcategory after calling the AddParent method.
        /// </remarks>
        [TestMethod]
        public void AddParent_ShouldAddCategoryToParentsList()
        {
            // Arrange
            var parentCategory = new Category("Parent");
            var childCategory = new Category("Child");

            // Act
            childCategory.AddParent(parentCategory);

            // Assert
            Assert.AreEqual(1, childCategory.Parents.Count, "Child category should have one parent.");
            Assert.AreEqual(parentCategory, childCategory.Parents[0], "Parent category should be correctly added to the child category.");
            Assert.AreEqual(1, parentCategory.Subcategories.Count, "Parent category should have one subcategory.");
            Assert.AreEqual(childCategory, parentCategory.Subcategories[0], "Child category should be correctly added to the parent category.");
        }

        /// <summary>
        /// Tests that the <see cref="AddSubcategory"/> method correctly adds a subcategory to the parent's Subcategories list and updates the subcategory's Parents list.
        /// </summary>
        /// <remarks>
        /// Verifies that the parent category has one subcategory and the subcategory has one parent after calling the AddSubcategory method.
        /// </remarks>
        [TestMethod]
        public void AddSubcategory_ShouldAddCategoryToSubcategoriesList()
        {
            // Arrange
            var parentCategory = new Category("Parent");
            var subCategory = new Category("Subcategory");

            // Act
            parentCategory.AddSubcategory(subCategory);

            // Assert
            Assert.AreEqual(1, parentCategory.Subcategories.Count, "Parent category should have one subcategory.");
            Assert.AreEqual(subCategory, parentCategory.Subcategories[0], "Subcategory should be correctly added to the parent category.");
            Assert.AreEqual(1, subCategory.Parents.Count, "Subcategory should have one parent.");
            Assert.AreEqual(parentCategory, subCategory.Parents[0], "Parent category should be correctly added to the subcategory.");
        }
    }
}
