namespace DomainModel.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CategoryTests
    {
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullName_ShouldThrowArgumentNullException()
        {
            // Act
            var category = new Category(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EmptyName_ShouldThrowArgumentException()
        {
            // Act
            var category = new Category("");
        }

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
