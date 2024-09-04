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
    public class Category : ICategory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        public Category(string name)
        {
            this.Name = name;
            this.Parents = new List<ICategory>();
            this.Subcategories = new List<ICategory>();
        }

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
        public List<ICategory> Parents { get;  set; }

        /// <summary>
        /// Gets or sets the list of subcategories that belong to this category.
        /// </summary>
        [Required(ErrorMessage = "Subcategories list is required.")]
        [MinLength(1, ErrorMessage = "Category must have at least one subcategory.")]
        public List<ICategory> Subcategories { get;  set; }

        /// <inheritdoc/>
        public void AddParent(ICategory parentCategory)
        {
            if (!this.Parents.Contains(parentCategory))
            {
                this.Parents.Add(parentCategory);
                parentCategory.AddSubcategory(this);
            }
        }

        /// <inheritdoc/>
        public void AddSubcategory(ICategory subcategory)
        {
            if (!this.Subcategories.Contains(subcategory))
            {
                this.Subcategories.Add(subcategory);
                subcategory.AddParent(this);
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
