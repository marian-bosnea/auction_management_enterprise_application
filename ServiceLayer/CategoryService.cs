// <copyright file="CategoryService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using ServiceLayer;

    /// <summary>
    /// Manages categories and products, allowing creation and association of products with categories.
    /// </summary>
    public class CategoryService
    {
        /// <summary>
        /// The default similarity threshold used for checking product description similarity.
        /// This value is used if no valid threshold is provided in the configuration file.
        /// </summary>
        private const int DefaultSimilarityThreshold = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        public CategoryService()
        {
            this.Categories = new Dictionary<string, Category>();
            this.Products = new List<Product>();

            this.SimilarityThreshold = this.GetSimilarityThresholdFromConfig();
        }

        /// <summary>
        /// Gets the dictionary of categories, keyed by their name.
        /// </summary>
        public Dictionary<string, Category> Categories { get; private set; }

        /// <summary>
        /// Gets the list of products managed by this CategoryService.
        /// </summary>
        public List<Product> Products { get; private set; }

        /// <summary>
        /// Gets or sets the similarity threshold for determining if a new product's description is too similar
        /// to the descriptions of existing products. This value is read from the configuration file.
        /// If the value is not specified or is invalid, the default threshold is used.
        /// </summary>
        private int SimilarityThreshold { get; set; }

        /// <summary>
        /// Creates a new category if it does not already exist.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <returns>The created or existing <see cref="Category"/>.</returns>
        public Category CreateCategory(string name)
        {
            if (!this.Categories.ContainsKey(name))
            {
                var category = new Category(name);
                this.Categories[name] = category;
            }

            return this.Categories[name];
        }

        /// <summary>
        /// Creates a new product with the specified name, description, and categories.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <param name="description">The description of the product.</param>
        /// <param name="categoryNames">A list of category names to associate with the product.</param>
        /// <returns>The newly created <see cref="Product"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a product with a similar description already exists.</exception>
        public Product CreateProduct(string name, string description, List<string> categoryNames)
        {
            // Check for similar descriptions in existing products
            foreach (var existingProduct in this.Products)
            {
                int distance = StringUtils.CalculateLevenshteinDistance(existingProduct.Description, description);
                if (distance <= this.SimilarityThreshold)
                {
                    throw new InvalidOperationException("A similar product already exists.");
                }
            }

            // Create and configure the new product
            var product = new Product(name, description);
            foreach (var catName in categoryNames)
            {
                if (this.Categories.ContainsKey(catName))
                {
                    product.AddCategory(this.Categories[catName]);
                }
                else
                {
                    var newCategory = this.CreateCategory(catName);
                    product.AddCategory(newCategory);
                }
            }

            this.Products.Add(product);
            return product;
        }

        /// <summary>
        /// Returns a string representation of the category manager, listing all categories and products.
        /// </summary>
        /// <returns>A string that represents the current category manager.</returns>
        public override string ToString()
        {
            var categoryNames = string.Join(", ", this.Categories.Keys);
            var productNames = string.Join(", ", this.Products);
            return $"Categories: {categoryNames}\nProducts: {productNames}";
        }

        /// <summary>
        /// Retrieves the similarity threshold from the configuration file.
        /// </summary>
        /// <returns>The similarity threshold.</returns>
        private int GetSimilarityThresholdFromConfig()
        {
            int threshold;
            string configValue = ConfigurationManager.AppSettings["SimilarityThreshold"];

            if (int.TryParse(configValue, out threshold))
            {
                return threshold;
            }
            else
            {
                return DefaultSimilarityThreshold;
            }
        }
    }
}