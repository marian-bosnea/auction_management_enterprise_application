// <copyright file="ProductService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using DataMapper.Interfaces;
    using ServiceLayer;
    using ServiceLayer.Interfaces;

    /// <summary>
    /// Manages categories and products, allowing creation and association of products with categories.
    /// </summary>
    public class ProductService : IProductService
    {
        /// <summary>
        /// The default similarity threshold used for checking product description similarity.
        /// This value is used if no valid threshold is provided in the configuration file.
        /// </summary>
        private const int DefaultSimilarityThreshold = 5;

        /// <summary>
        /// The DAO interface for managing product-related data.
        /// </summary>
        private readonly IProductDAO productDAO;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// <param name="productDAO">The DAO which manages products.</param>
        /// </summary>
        public ProductService(IProductDAO productDAO)
        {
            this.Categories = new Dictionary<string, ICategory>();
            this.Products = new List<IProduct>();

            this.productDAO = productDAO;

            this.SimilarityThreshold = this.GetSimilarityThresholdFromConfig();
        }

        /// <summary>
        /// Gets the dictionary of categories, keyed by their name.
        /// </summary>
        public Dictionary<string, ICategory> Categories { get; private set; }

        /// <summary>
        /// Gets the list of products managed by this ProductService.
        /// </summary>
        public List<IProduct> Products { get; private set; }

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
        public ICategory CreateCategory(string name)
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
        public IProduct CreateProduct(string name, string description, List<string> categoryNames)
        {
            foreach (var existingProduct in this.Products)
            {
                int distance = StringUtils.CalculateLevenshteinDistance(existingProduct.Description, description);
                if (distance <= this.SimilarityThreshold)
                {
                    throw new InvalidOperationException("A similar product already exists.");
                }
            }

            var product = new Product(0, name, description, new List<ICategory>());
            foreach (var catName in categoryNames)
            {
                if (this.Categories.ContainsKey(catName))
                {
                    product.AddCategory(this.Categories[catName]);
                }
                else
                {
                    var newCategory = this.CreateCategory(catName);
                    this.Categories.Add(catName, newCategory);
                    product.AddCategory(newCategory);
                }
            }

            this.Products.Add(product);
            return product;
        }

        /// <summary>
        /// Adds a product to the system.
        /// <param name="product">The product to be added.</param>
        /// </summary>
        public void AddProduct(IProduct product)
        {
            this.Products.Add(product);
            this.productDAO.Add(product);
        }

        /// <summary>
        /// Gets a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The product with the specified ID, or null if not found.</returns>
        public IProduct GetProductById(int id)
        {
            return this.Products.Find(p => p.Id == id);
        }

        /// <summary>
        /// Gets all products in the system.
        /// </summary>
        /// <returns>A list of all products.</returns>
        public List<IProduct> GetAllProducts()
        {
             this.Products = this.productDAO.GetAll();

             return this.Products;
        }

        /// <summary>
        /// Updates an existing product in the system.
        /// <param name="productID">The ID of the product to be updated.</param>
        /// </summary>
        public void UpdateProduct(int productID)
        {
            var product = this.GetProductById(productID);

            if (product != null)
            {
                var newProduct = new Product(product.Id, product.Name, product.Description, product.Categories);

                this.Products.Remove(product);
                this.Products.Add(newProduct);

                this.productDAO.Update(newProduct);
            }
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        public void DeleteProduct(int id)
        {
            var product = this.GetProductById(id);
            if (product != null)
            {
                this.Products.Remove(product);
            }
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