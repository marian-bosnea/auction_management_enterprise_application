// <copyright file="Category.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System.Collections.Generic;

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
        /// Gets the name of the category.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the list of parent categories to which this category belongs.
        /// </summary>
        public List<ICategory> Parents { get; private set; }

        /// <summary>
        /// Gets the list of subcategories that belong to this category.
        /// </summary>
        public List<ICategory> Subcategories { get; private set; }

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
