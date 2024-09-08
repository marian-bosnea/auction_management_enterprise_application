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
    public class Product
    {
        public Product()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="id">The id of the product.</param>
        /// <param name="name">The name of the product.</param>
        /// <param name="description">The description of the product.</param>
        /// <param name="categories">The categories of the product.</param>
        public Product(int id, string name, string description, List<Category> categories)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Categories = categories;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public int Id { get; set; }

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
        public List<Category> Categories { get; private set; }

        /// <summary>
        /// Gets the description of the product.
        /// </summary>
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description must not exceed 1000 characters.")]
        public string Description { get; private set; }

        /// <summary>
        /// Adds a category to the list of categories associated with this product.
        /// </summary>
        /// <param name="category">The category to add.</param>
        public void AddCategory(Category category)
        {
            if (category == null)
            {
                return;
            }

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