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

    /// <summary>
    /// Provides data access functionality for products using EF6.
    /// </summary>
    public class ProductDAO : IProductDAO
    {
        /// <summary>
        /// Represents the EF Core database context used for performing data operations
        /// on the auction management entities. It provides access to the database and
        /// manages tracking of changes to entity objects.
        /// </summary>
        private readonly AuctionManagementEfCoreDbContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDAO"/> class.
        /// </summary>
        /// <param name="databaseContext">The EF6 database context to use.</param>
        public ProductDAO(AuctionManagementEfCoreDbContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Adds a new product to the data store.
        /// </summary>
        /// <param name="product">The product to add.</param>
        public void Add(Product product)
        {
            this.databaseContext.Products.Add(product);
            this.databaseContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a product from the data store by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the product to delete.</param>
        public void Delete(int id)
        {
            var product = this.databaseContext.Products.Find(id);
            if (product != null)
            {
                this.databaseContext.Products.Remove(product);
                this.databaseContext.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves a specific product by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the product to retrieve.</param>
        /// <returns>The product with the specified identifier, or null if not found.</returns>
        public Product Get(int id)
        {
            return this.databaseContext.Products
                            .Include(p => p.Categories)
                            .FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Retrieves all products from the data store.
        /// </summary>
        /// <returns>A list of all products.</returns>
        public List<Product> GetAll()
        {
            return this.databaseContext.Products
                            .Include(p => p.Categories)
                            .ToList();
        }

        /// <summary>
        /// Updates an existing product in the data store.
        /// </summary>
        /// <param name="product">The product to update.</param>
        public void Update(Product product)
        {
            this.databaseContext.Entry(product).State = EntityState.Modified;
            this.databaseContext.SaveChanges();
        }
    }
}
