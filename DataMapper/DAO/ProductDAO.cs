// <copyright file="ProductDAO.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper.DAO
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using DataMapper.Interfaces;
    using DomainModel;
    using log4net;

    /// <summary>
    /// Provides data access functionality for products using EF6.
    /// </summary>
    public class ProductDAO : IProductDAO
    {
        /// <summary>
        /// The logger for logging actions in the class.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Represents the EF Core database context used for performing data operations
        /// on the auction management entities. It provides access to the database and
        /// manages tracking of changes to entity objects.
        /// </summary>
        private readonly IAuctionManagementEfCoreDbContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDAO"/> class.
        /// </summary>
        /// <param name="databaseContext">The EF6 database context to use.</param>
        public ProductDAO(IAuctionManagementEfCoreDbContext databaseContext)
        {
            this.databaseContext = databaseContext;
            Logger.Info("ProductDAO initialized.");
        }

        /// <summary>
        /// Adds a new product to the data store.
        /// </summary>
        /// <param name="product">The product to add.</param>
        public void Add(Product product)
        {
            Logger.Info($"Adding product: {product.Name}");
            this.databaseContext.Products.Add(product);
            this.databaseContext.SaveChanges();
            Logger.Info($"Product added successfully: {product.Name}");
        }

        /// <summary>
        /// Deletes a product from the data store by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the product to delete.</param>
        public void Delete(int id)
        {
            Logger.Info($"Deleting product with ID: {id}");
            var product = this.databaseContext.Products.Find(id);
            if (product != null)
            {
                this.databaseContext.Products.Remove(product);
                this.databaseContext.SaveChanges();
                Logger.Info($"Product with ID: {id} deleted successfully.");
            }
            else
            {
                Logger.Warn($"Product with ID: {id} not found.");
            }
        }

        /// <summary>
        /// Retrieves a specific product by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the product to retrieve.</param>
        /// <returns>The product with the specified identifier, or null if not found.</returns>
        public Product Get(int id)
        {
            Logger.Info($"Retrieving product with ID: {id}");
            var product = this.databaseContext.Products
                                .Include(p => p.Categories)
                                .FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                Logger.Info($"Product retrieved successfully: {product.Name}");
            }
            else
            {
                Logger.Warn($"Product with ID: {id} not found.");
            }

            return product;
        }

        /// <summary>
        /// Retrieves all products from the data store.
        /// </summary>
        /// <returns>A list of all products.</returns>
        public List<Product> GetAll()
        {
            Logger.Info("Retrieving all products.");
            var products = this.databaseContext.Products
                                .Include(p => p.Categories)
                                .ToList();

            Logger.Info($"{products.Count} products retrieved.");
            return products;
        }

        /// <summary>
        /// Updates an existing product in the data store.
        /// </summary>
        /// <param name="product">The product to update.</param>
        public void Update(Product product)
        {
            Logger.Info($"Updating product: {product.Name}");
            this.databaseContext.Entry(product).State = EntityState.Modified;
            this.databaseContext.SaveChanges();
            Logger.Info($"Product updated successfully: {product.Name}");
        }
    }
}