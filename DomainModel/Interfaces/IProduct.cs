// <copyright file="IProduct.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the contract for a product that can be auctioned.
    /// </summary>
    public interface IProduct
    {
        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the list of categories the product belongs to.
        /// </summary>
        List<ICategory> Categories { get; }

        /// <summary>
        /// Adds a category to the list of categories associated with this product.
        /// </summary>
        /// <param name="category">The category to add.</param>
        void AddCategory(ICategory category);
    }
}
