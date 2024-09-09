// <copyright file="Category.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using log4net;

    /// <summary>
    /// Represents a category that can be part of a hierarchy, with parent and subcategory relationships.
    /// </summary>
    public class Category
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class with empty lists for parents and subcategories.
        /// </summary>
        public Category()
        {
            logger.Info("Initializing Category with empty parents and subcategories lists.");
            this.Parents = new List<Category>();
            this.Subcategories = new List<Category>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        public Category(string name)
        {
            logger.Info($"Initializing Category with name: {name}");

            if (name == null)
            {
                logger.Error("Category name is null.");
                throw new ArgumentNullException("Name must not be null.");
            }

            if (name.Length == 0)
            {
                logger.Error("Category name is an empty string.");
                throw new ArgumentException("Name must be a non-empty string");
            }

            this.Name = name;
            this.Parents = new List<Category>();
            this.Subcategories = new List<Category>();
            logger.Info($"Category '{name}' initialized successfully.");
        }

        /// <summary>
        /// Gets or sets the unique identifier for the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Category name must be between 1 and 100 characters long.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of parent categories to which this category belongs.
        /// </summary>
        [Required(ErrorMessage = "Parents list is required.")]
        public List<Category> Parents { get; set; }

        /// <summary>
        /// Gets or sets the list of subcategories that belong to this category.
        /// </summary>
        [Required(ErrorMessage = "Subcategories list is required.")]
        public List<Category> Subcategories { get; set; }

        /// <summary>
        /// Adds a parent category to this category.
        /// </summary>
        /// <param name="parentCategory">The parent category to add.</param>
        public void AddParent(Category parentCategory)
        {
            logger.Info($"Adding parent category: {parentCategory.Name} to category: {this.Name}");
            if (!this.Parents.Contains(parentCategory))
            {
                this.Parents.Add(parentCategory);
                parentCategory.AddSubcategory(this);
                logger.Info($"Parent category: {parentCategory.Name} added to category: {this.Name}");
            }
        }

        /// <summary>
        /// Adds a subcategory to this category.
        /// </summary>
        /// <param name="subcategory">The subcategory to add.</param>
        public void AddSubcategory(Category subcategory)
        {
            logger.Info($"Adding subcategory: {subcategory.Name} to category: {this.Name}");
            if (!this.Subcategories.Contains(subcategory))
            {
                this.Subcategories.Add(subcategory);
                subcategory.AddParent(this);
                logger.Info($"Subcategory: {subcategory.Name} added to category: {this.Name}");
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that contains the name of the current object.
        /// </returns>
        public override string ToString()
        {
            logger.Info($"Converting Category to string: {this.Name}");
            return this.Name;
        }
    }
}
