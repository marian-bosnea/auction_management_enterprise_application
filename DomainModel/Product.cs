// <copyright file="Product.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a product that can be associated with one or more categories.
    /// </summary>
    public class Product : IProduct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <param name="description">The description of the product.</param>
        public Product(string name, string description)
        {
            this.Name = name;
            this.Description = description;
            this.Categories = new List<ICategory>();
        }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 200 characters long.")]
        public string Name { get; private set; }

        /// <summary>
        /// Gets the list of categories associated with this product.
        /// </summary>
        [Required(ErrorMessage = "At least one category is required.")]
        public List<ICategory> Categories { get; private set; }

        /// <summary>
        /// Gets the description of the product.
        /// </summary>
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description must not exceed 1000 characters.")]
        public string Description { get; private set; }

        /// <inheritdoc/>
        public void AddCategory(ICategory category)
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