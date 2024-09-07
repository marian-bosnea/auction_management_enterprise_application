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
        /// The service interface for managing category-related data.
        /// </summary>
        private readonly ICategoryService categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productDAO">The DAO interface for managing product-related data. This is used to perform CRUD operations on products.</param>
        /// <param name="categoryService">The service for managing categories. This is used to create and retrieve categories associated with products.</param>
        /// <remarks>
        /// The constructor initializes the internal collections for storing categories and products. It also configures the similarity threshold
        /// for product description comparisons, which is obtained from the configuration settings.
        /// </remarks>
        public ProductService(IProductDAO productDAO, ICategoryService categoryService)
        {
            this.Categories = new Dictionary<string, Category>();
            this.Products = new List<Product>();

            this.productDAO = productDAO;
            this.categoryService = categoryService;

            this.SimilarityThreshold = this.GetSimilarityThresholdFromConfig();
        }

        /// <summary>
        /// Gets the dictionary of categories, keyed by their name.
        /// </summary>
        public Dictionary<string, Category> Categories { get; private set; }

        /// <summary>
        /// Gets the list of products managed by this ProductService.
        /// </summary>
        public List<Product> Products { get; private set; }

        /// <summary>
        /// Gets or sets the similarity threshold for determining if a new product's description is too similar
        /// to the descriptions of existing products. This value is read from the configuration file.
        /// If the value is not specified or is invalid, the default threshold is used.
        /// </summary>
        private int SimilarityThreshold { get; set; }

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
            foreach (var existingProduct in this.Products)
            {
                int distance = StringUtils.CalculateLevenshteinDistance(existingProduct.Description, description);
                if (distance <= this.SimilarityThreshold)
                {
                    throw new InvalidOperationException("A similar product already exists.");
                }
            }

            var product = new Product(0, name, description, new List<Category>());

            foreach (var catName in categoryNames)
            {
                var category = this.categoryService.CreateCategory(catName);
                product.AddCategory(category);
            }

            this.productDAO.Add(product);
            this.Products.Add(product);

            return product;
        }

        /// <summary>
        /// Adds a new product to the internal collection and persists it in the data store.
        /// </summary>
        /// <param name="product">The product to be added. This should be a fully initialized <see cref="Product"/> instance.</param>
        /// <remarks>
        /// The method performs two key actions:
        /// 1. Adds the product to the internal list of products managed by this service.
        /// 2. Uses the <see cref="ProductDAO"/> to add the product to the data store, ensuring it is persisted across sessions.
        /// </remarks>
        public void AddProduct(Product product)
        {
            this.Products.Add(product);
            this.productDAO.Add(product);
        }

        /// <summary>
        /// Gets a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The product with the specified ID, or null if not found.</returns>
        public Product GetProductById(int id)
        {
            return this.Products.Find(p => p.Id == id);
        }

        /// <summary>
        /// Gets all products in the system.
        /// </summary>
        /// <returns>A list of all products.</returns>
        public List<Product> GetAllProducts()
        {
             this.Products = this.productDAO.GetAll();

             return this.Products;
        }

        /// <summary>
        /// Updates an existing product in the internal collection and persists the changes in the data store.
        /// </summary>
        /// <param name="product">The product to be updated. This should be a fully initialized <see cref="Product"/> instance with the updated details.</param>
        /// <remarks>
        /// The method performs two key actions:
        /// 1. Replaces the existing product in the internal list of products with the updated product.
        /// 2. Uses the <see cref="ProductDAO"/> to update the product in the data store, ensuring that the changes are persisted across sessions.
        /// </remarks>
        public void UpdateProduct(Product product)
        {
                var newProduct = new Product(product.Id, product.Name, product.Description, product.Categories);

                this.Products.Remove(product);
                this.Products.Add(newProduct);

                this.productDAO.Update(newProduct);
        }

        /// <summary>
        /// Removes a product from the internal collection of products.
        /// </summary>
        /// <param name="product">The product to be deleted. This should be an instance of <see cref="Product"/> that exists in the internal collection.</param>
        /// <remarks>
        /// This method removes the specified product from the internal list of products. It does not interact with the data store or perform any other operations.
        /// To ensure consistency, any additional persistence or cleanup operations should be handled separately.
        /// </remarks>
        public void DeleteProduct(Product product)
        {
                this.Products.Remove(product);
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