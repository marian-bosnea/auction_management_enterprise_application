// <copyright file="Category.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a category that can be part of a hierarchy, with parent and subcategory relationships.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        public Category(string name)
        {
            this.Name = name;
            this.Parents = new List<Category>();
            this.Subcategories = new List<Category>();
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
        public string Name { get;  set; }

        /// <summary>
        /// Gets or sets the list of parent categories to which this category belongs.
        /// </summary>
        [Required(ErrorMessage = "Parents list is required.")]
        [MinLength(1, ErrorMessage = "Category must have at least one parent category.")]
        public List<Category> Parents { get;  set; }

        /// <summary>
        /// Gets or sets the list of subcategories that belong to this category.
        /// </summary>
        [Required(ErrorMessage = "Subcategories list is required.")]
        [MinLength(1, ErrorMessage = "Category must have at least one subcategory.")]
        public List<Category> Subcategories { get;  set; }

        /// <summary>
        /// Adds a parent category to this category.
        /// </summary>
        /// <param name="parentCategory">The parent category to add.</param>
        public void AddParent(Category parentCategory)
        {
            if (!this.Parents.Contains(parentCategory))
            {
                this.Parents.Add(parentCategory);
                parentCategory.AddSubcategory(this);
            }
        }

        /// <summary>
        /// Adds a subcategory to this category.
        /// </summary>
        /// <param name="subcategory">The subcategory to add.</param>
        public void AddSubcategory(Category subcategory)
        {
            if (!this.Subcategories.Contains(subcategory))
            {
                this.Subcategories.Add(subcategory);
                subcategory.AddParent(this);
            }
        }

        /// <summary>
        /// Adds a subcategory to this category.
        /// </summary>
        /// <param name="subcategory">The subcategory to add.</param>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
