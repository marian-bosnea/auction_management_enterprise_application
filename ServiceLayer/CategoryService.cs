// <copyright file="CategoryService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Manages categories and products, allowing creation and association of products with categories.
    /// </summary>
    public class CategoryService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        public CategoryService()
        {
            this.Categories = new Dictionary<string, Category>();
            this.Products = new List<Product>();
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
        /// Creates a new product and associates it with the specified categories.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <param name="categoryNames">The list of category names to associate with the product.</param>
        /// <returns>The created <see cref="Product"/>.</returns>
        public Product CreateProduct(string name, List<string> categoryNames)
        {
            var product = new Product(name);
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
    }
}