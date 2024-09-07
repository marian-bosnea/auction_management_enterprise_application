// <copyright file="CategoryService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace ServiceLayer.Services
{
    using System.Collections.Generic;
    using DataMapper.Interfaces;
    using DomainModel;
    using ServiceLayer.Interfaces;

    /// <summary>
    /// Provides services for managing categories.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        /// <summary>
        /// The DAO interface for managing category-related data.
        /// </summary>
        private readonly ICategoryDAO categoryDAO;

        /// <summary>
        /// A dictionary for caching categories by their name.
        /// </summary>
        private readonly Dictionary<string, Category> categories;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="categoryDAO">The DAO which manages categories.</param>
        public CategoryService(ICategoryDAO categoryDAO)
        {
            this.categoryDAO = categoryDAO;
            this.categories = new Dictionary<string, Category>();
        }

        /// <summary>
        /// Creates a new category with the specified name if it does not already exist.
        /// </summary>
        /// <param name="name">The name of the category to be created.</param>
        /// <returns>The created or existing <see cref="Category"/> instance.</returns>
        public Category CreateCategory(string name)
        {
            if (!this.categories.TryGetValue(name, out var category))
            {
                category = this.categoryDAO.GetByName(name);

                if (category == null)
                {
                    category = new Category(name);
                    this.categoryDAO.Add(category);
                    this.categories[name] = category;
                }
                else
                {
                    this.categories[name] = category;
                }
            }

            return category;
        }

        public Dictionary<string, Category> Categories { get { return this.categories;  } }
    }
}
