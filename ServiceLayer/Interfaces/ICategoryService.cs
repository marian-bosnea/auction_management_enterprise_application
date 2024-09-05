// <copyright file="ICategoryService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace ServiceLayer.Interfaces
{
    using DomainModel;

    /// <summary>
    /// Defines the contract for a service that manages categories.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Creates a new category with the specified name.
        /// </summary>
        /// <param name="name">The name of the category to be created.</param>
        /// <returns>The created <see cref="ICategory"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when a category with the specified name already exists.</exception>
        ICategory CreateCategory(string name);
    }
}