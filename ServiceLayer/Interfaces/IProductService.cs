// <copyright file="IProductService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace ServiceLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using DomainModel;

    /// <summary>
    /// Interface for managing categories and products, allowing creation, retrieval, update, and deletion of products.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Creates a new category if it does not already exist.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <returns>The created or existing <see cref="ICategory"/>.</returns>
        ICategory CreateCategory(string name);

        /// <summary>
        /// Creates a new product with the specified name, description, and categories.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <param name="description">The description of the product.</param>
        /// <param name="categoryNames">A list of category names to associate with the product.</param>
        /// <returns>The newly created <see cref="IProduct"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a product with a similar description already exists.</exception>
        IProduct CreateProduct(string name, string description, List<string> categoryNames);

        /// <summary>
        /// Adds a product to the system.
        /// </summary>
        /// <param name="product">The product to add.</param>
        void AddProduct(IProduct product);

        /// <summary>
        /// Gets a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The product with the specified ID, or null if not found.</returns>
        IProduct GetProductById(int id);

        /// <summary>
        /// Gets all products in the system.
        /// </summary>
        /// <returns>A list of all products.</returns>
        List<IProduct> GetAllProducts();

        /// <summary>
        /// Updates an existing product in the system.
        /// </summary>
        /// <param name="productID">The ID of the product to update.</param>
        void UpdateProduct(int productID);

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        void DeleteProduct(int id);

        /// <summary>
        /// Returns a string representation of the category manager, listing all categories and products.
        /// </summary>
        /// <returns>A string that represents the current category manager.</returns>
        string ToString();
    }
}
