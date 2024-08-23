// <copyright file="Product.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a product that can be associated with one or more categories.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        public Product(string name)
        {
            this.Name = name;
            this.Categories = new List<Category>();
        }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the list of categories associated with this product.
        /// </summary>
        public List<Category> Categories { get; private set; }

        /// <summary>
        /// Adds a category to the list of categories associated with this product.
        /// </summary>
        /// <param name="category">The category to add.</param>
        public void AddCategory(Category category)
        {
            if (!this.Categories.Contains(category))
            {
                this.Categories.Add(category);
            }
        }

        /// <summary>
        /// Returns a string representation of the product.
        /// </summary>
        /// <returns>A string that represents the current product.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}