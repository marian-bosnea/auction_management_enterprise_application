// <copyright file="ICategoryDAO.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper.Interfaces
{
    using System.Collections.Generic;
    using DomainModel;

    /// <summary>
    /// Defines the data access operations for categories.
    /// </summary>
    public interface ICategoryDAO
    {
        /// <summary>
        /// Adds a new category to the data store.
        /// </summary>
        /// <param name="category">The category to add.</param>
        void Add(Category category);

        /// <summary>
        /// Retrieves all categories from the data store.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        List<Category> GetAll();

        /// <summary>
        /// Retrieves a specific category by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the category to retrieve.</param>
        /// <returns>The category with the specified identifier, or null if not found.</returns>
        Category Get(int id);

        /// <summary>
        /// Retrieves a specific category by its identifier.
        /// </summary>
        /// <param name="name">The name of the category to retrieve.</param>
        /// <returns>The category with the specified identifier, or null if not found.</returns>
        Category GetByName(string name);

        /// <summary>
        /// Updates an existing category in the data store.
        /// </summary>
        /// <param name="category">The category to update.</param>
        void Update(Category category);

        /// <summary>
        /// Deletes a category from the data store by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the category to delete.</param>
        void Delete(int id);
    }
}