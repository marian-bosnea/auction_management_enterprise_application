// <copyright file="Product.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using log4net;

    /// <summary>
    /// Represents a product that can be associated with one or more categories.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Logger instance for logging operations within the <see cref="StringUtils"/> class.
        /// </summary>
        /// <remarks>
        /// This static readonly field is used to log information, warnings, errors, and other messages related to string operations.
        /// It utilizes the log4net library for logging, and the logger is configured to log messages based on the class's namespace and type.
        /// </remarks>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// This empty constructor is used for creating a default instance of the <see cref="Product"/> class.
        /// </summary>
        public Product()
        {
            Logger.Info("Initialized a new Product instance with default values.");
            this.Categories = new List<Category>(); // Initialize the Categories list to avoid null reference issues.
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
            Logger.Info($"Creating Product with ID: {id}, Name: {name}");

            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Categories = categories ?? new List<Category>(); // Ensure Categories is initialized.

            Logger.Info("Product created successfully.");
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
            Logger.Info($"Attempting to add category: {category?.Name} to product: {this.Name}");

            if (category == null)
            {
                Logger.Warn("Category is null, cannot add to the product.");
                return;
            }

            if (!this.Categories.Contains(category))
            {
                this.Categories.Add(category);
                Logger.Info($"Category: {category.Name} added to product: {this.Name}");
            }
            else
            {
                Logger.Info($"Category: {category.Name} is already associated with product: {this.Name}");
            }
        }

        /// <summary>
        /// Returns a string representation of the product.
        /// </summary>
        /// <returns>A string that represents the current product.</returns>
        public override string ToString()
        {
            Logger.Info($"Converting Product to string: {this.Name}");
            return this.Name;
        }
    }
}