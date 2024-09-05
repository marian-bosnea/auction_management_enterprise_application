// <copyright file="ICategory.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the contract for a category that can be part of a hierarchy, with parent and subcategory relationships.
    /// </summary>
    public interface ICategory
    {
        /// <summary>
        /// Gets or sets the unique identifier for the category.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the list of parent categories to which this category belongs.
        /// </summary>
        List<ICategory> Parents { get; }

        /// <summary>
        /// Gets the list of subcategories that belong to this category.
        /// </summary>
        List<ICategory> Subcategories { get; }

        /// <summary>
        /// Adds a parent category to this category.
        /// </summary>
        /// <param name="parentCategory">The parent category to add.</param>
        void AddParent(ICategory parentCategory);

        /// <summary>
        /// Adds a subcategory to this category.
        /// </summary>
        /// <param name="subcategory">The subcategory to add.</param>
        void AddSubcategory(ICategory subcategory);

        /// <summary>
        /// Returns a string representation of the category.
        /// </summary>
        /// <returns>A string that represents the current category.</returns>
        string ToString();
    }
}
