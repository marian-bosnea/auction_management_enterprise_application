// <copyright file="ProductService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using DataMapper.Interfaces;
    using log4net;
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
        public const int DefaultSimilarityThreshold = 5;

        /// <summary>
        /// The DAO interface for managing product-related data.
        /// </summary>
        private readonly IProductDAO productDAO;

        /// <summary>
        /// The service interface for managing category-related data.
        /// </summary>
        private readonly ICategoryService categoryService;

        /// <summary>
        /// The similarity threshold for checking product description similarity.
        /// </summary>
        private int SimilarityThreshold { get; set; }

        /// <summary>
        /// Logger for logging actions in the class.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ProductService));

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productDAO">The DAO interface for managing product-related data.</param>
        /// <param name="categoryService">The service for managing categories.</param>
        public ProductService(IProductDAO productDAO, ICategoryService categoryService)
        {
            this.Categories = new Dictionary<string, Category>();
            this.Products = new List<Product>();

            this.productDAO = productDAO;
            this.categoryService = categoryService;

            this.SimilarityThreshold = this.GetSimilarityThresholdFromConfig();

            Logger.Info($"ProductService initialized with similarity threshold: {this.SimilarityThreshold}");
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
        /// Creates a new product with the specified name, description, and categories.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <param name="description">The description of the product.</param>
        /// <param name="categoryNames">A list of category names to associate with the product.</param>
        /// <returns>The newly created <see cref="Product"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a product with a similar description already exists.</exception>
        public Product CreateProduct(string name, string description, List<string> categoryNames)
        {
            Logger.Info($"Creating product with name: {name}");

            foreach (var existingProduct in this.Products)
            {
                int distance = StringUtils.CalculateLevenshteinDistance(existingProduct.Description, description);
                if (distance <= this.SimilarityThreshold)
                {
                    Logger.Warn($"A similar product with description '{description}' already exists.");
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

            Logger.Info($"Product '{name}' created successfully with ID: {product.Id}");
            return product;
        }

        /// <summary>
        /// Adds a new product to the internal collection and persists it in the data store.
        /// </summary>
        /// <param name="product">The product to be added. This should be a fully initialized <see cref="Product"/> instance.</param>
        public void AddProduct(Product product)
        {
            Logger.Info($"Adding product with ID: {product.Id}");
            this.Products.Add(product);
            this.productDAO.Add(product);
            Logger.Info($"Product with ID: {product.Id} added successfully.");
        }

        /// <summary>
        /// Gets a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The product with the specified ID, or null if not found.</returns>
        public Product GetProductById(int id)
        {
            Logger.Info($"Retrieving product with ID: {id}");
            return this.Products.Find(p => p.Id == id);
        }

        /// <summary>
        /// Gets all products in the system.
        /// </summary>
        /// <returns>A list of all products.</returns>
        public List<Product> GetAllProducts()
        {
            Logger.Info("Retrieving all products.");
            this.Products = this.productDAO.GetAll();
            Logger.Info($"Retrieved {this.Products.Count} products.");
            return this.Products;
        }

        /// <summary>
        /// Updates an existing product in the internal collection and persists the changes in the data store.
        /// </summary>
        /// <param name="product">The product to be updated. This should be a fully initialized <see cref="Product"/> instance with the updated details.</param>
        public void UpdateProduct(Product product)
        {
            Logger.Info($"Updating product with ID: {product.Id}");

            var newProduct = new Product(product.Id, product.Name, product.Description, product.Categories);

            this.Products.Remove(this.Products.Find(p => p.Id == product.Id));
            this.Products.Add(newProduct);

            this.productDAO.Update(product);

            Logger.Info($"Product with ID: {product.Id} updated successfully.");
        }

        /// <summary>
        /// Removes a product from the internal collection of products.
        /// </summary>
        /// <param name="product">The product to be deleted. This should be an instance of <see cref="Product"/> that exists in the internal collection.</param>
        public void DeleteProduct(Product product)
        {
            Logger.Info($"Deleting product with ID: {product.Id}");
            this.Products.Remove(product);
            Logger.Info($"Product with ID: {product.Id} removed from internal collection.");
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
        public int GetSimilarityThresholdFromConfig()
        {
            Logger.Info("Retrieving similarity threshold from configuration.");

            string configValue = ConfigurationManager.AppSettings["SimilarityThreshold"];

            if (int.TryParse(configValue, out int threshold))
            {
                Logger.Info($"Similarity threshold retrieved from configuration: {threshold}");
                return threshold;
            }
            else
            {
                Logger.Warn("Invalid or missing similarity threshold in configuration. Using default value.");
                return DefaultSimilarityThreshold;
            }
        }
    }
}