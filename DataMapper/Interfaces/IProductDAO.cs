// <copyright file="IProductDAO.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper.Interfaces
{
    using System.Collections.Generic;
    using DomainModel;

    /// <summary>
    /// Defines the data access operations for products.
    /// </summary>
    public interface IProductDAO
    {
        /// <summary>
        /// Adds a new product to the data store.
        /// </summary>
        /// <param name="product">The product to add.</param>
        void Add(IProduct product);

        /// <summary>
        /// Retrieves all products from the data store.
        /// </summary>
        /// <returns>A list of all products.</returns>
        List<IProduct> GetAll();

        /// <summary>
        /// Retrieves a specific product by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the product to retrieve.</param>
        /// <returns>The product with the specified identifier, or null if not found.</returns>
        Product Get(int id);

        /// <summary>
        /// Updates an existing product in the data store.
        /// </summary>
        /// <param name="product">The product to update.</param>
        void Update(IProduct product);

        /// <summary>
        /// Deletes a product from the data store by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the product to delete.</param>
        void Delete(int id);
    }
}
