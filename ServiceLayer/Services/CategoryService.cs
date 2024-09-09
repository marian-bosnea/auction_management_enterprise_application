// <copyright file="CategoryService.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace ServiceLayer.Services
{
    using System;
    using System.Collections.Generic;
    using DataMapper.Interfaces;
    using DomainModel;
    using log4net;
    using ServiceLayer.Interfaces;

    /// <summary>
    /// Provides services for managing categories.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        /// <summary>
        /// Logger instance for logging operations within the <see cref="StringUtils"/> class.
        /// </summary>
        /// <remarks>
        /// This static readonly field is used to log information, warnings, errors, and other messages related to string operations.
        /// It utilizes the log4net library for logging, and the logger is configured to log messages based on the class's namespace and type.
        /// </remarks>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            Logger.Info("Initializing CategoryService.");

            this.categoryDAO = categoryDAO;
            this.categories = new Dictionary<string, Category>();

            Logger.Info("CategoryService initialized successfully.");
        }

        /// <summary>
        /// Gets a dictionary for caching categories by their name.
        /// </summary>
        public Dictionary<string, Category> Categories
        {
            get { return this.categories; }
        }

        /// <summary>
        /// Creates a new category with the specified name if it does not already exist.
        /// </summary>
        /// <param name="name">The name of the category to be created.</param>
        /// <returns>The created or existing <see cref="Category"/> instance.</returns>
        public Category CreateCategory(string name)
        {
            Logger.Info($"Attempting to create or retrieve category with name: {name}");

            if (string.IsNullOrWhiteSpace(name))
            {
                Logger.Warn("Category name is null or empty.");
                throw new ArgumentException("Category name must not be null or empty.", nameof(name));
            }

            if (!this.categories.TryGetValue(name, out var category))
            {
                Logger.Info($"Category {name} not found in cache. Querying DAO.");

                category = this.categoryDAO.GetByName(name);

                if (category == null)
                {
                    Logger.Info($"Category {name} does not exist in DAO. Creating new category.");
                    category = new Category(name);
                    this.categoryDAO.Add(category);
                    this.categories[name] = category;
                    Logger.Info($"Category {name} created and added to DAO.");
                }
                else
                {
                    this.categories[name] = category;
                    Logger.Info($"Category {name} retrieved from DAO and added to cache.");
                }
            }
            else
            {
                Logger.Info($"Category {name} retrieved from cache.");
            }

            return category;
        }
    }
}