// <copyright file="CategoryDAO.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using DataMapper.Interfaces;
    using DomainModel;
    using log4net;

    /// <summary>
    /// Provides data access functionality for categories using EF6.
    /// </summary>
    public class CategoryDAO : ICategoryDAO
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Represents the EF Core database context used for performing data operations
        /// on the auction management entities. It provides access to the database and
        /// manages tracking of changes to entity objects.
        /// </summary>
        private readonly IAuctionManagementEfCoreDbContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryDAO"/> class.
        /// </summary>
        /// <param name="databaseContext">The EF6 database context to use.</param>
        public CategoryDAO(IAuctionManagementEfCoreDbContext databaseContext)
        {
            this.databaseContext = databaseContext;
            Logger.Info("CategoryDAO initialized.");
        }

        /// <summary>
        /// Adds a new category to the data store.
        /// </summary>
        /// <param name="category">The category to add.</param>
        public void Add(Category category)
        {
            Logger.Info($"Adding category with ID: {category.Id}");
            try
            {
                this.databaseContext.Categories.Add(category);
                this.databaseContext.SaveChanges();
                Logger.Info($"Category with ID: {category.Id} added successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while adding category with ID: {category.Id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes a category by its identifier.
        /// </summary>
        /// <param name="id">The category ID.</param>
        public void Delete(int id)
        {
            Logger.Info($"Deleting category with ID: {id}");
            try
            {
                var category = this.databaseContext.Categories.Find(id);
                if (category != null)
                {
                    this.databaseContext.Categories.Remove(category);
                    this.databaseContext.SaveChanges();
                    Logger.Info($"Category with ID: {id} deleted successfully.");
                }
                else
                {
                    Logger.Warn($"Category with ID: {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while deleting category with ID: {id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a category by its identifier.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>The category object or null if not found.</returns>
        public Category Get(int id)
        {
            Logger.Info($"Retrieving category with ID: {id}");
            try
            {
                var category = this.databaseContext.Categories
                                .Include(c => c.Subcategories)
                                .Include(c => c.Parents)
                                .FirstOrDefault(c => c.Id == id);
                if (category != null)
                {
                    Logger.Info($"Category with ID: {id} retrieved successfully.");
                }
                else
                {
                    Logger.Warn($"Category with ID: {id} not found.");
                }
                return category;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while retrieving category with ID: {id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        public List<Category> GetAll()
        {
            Logger.Info("Retrieving all categories.");
            try
            {
                var categories = this.databaseContext.Categories
                                .Include(c => c.Subcategories)
                                .Include(c => c.Parents)
                                .ToList();
                Logger.Info($"Retrieved {categories.Count} categories.");
                return categories;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while retrieving all categories.", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a category by its name.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <returns>The category object or null if not found.</returns>
        public Category GetByName(string name)
        {
            Logger.Info($"Retrieving category by name: {name}");
            try
            {
                var category = this.databaseContext.Categories
                                .Include(c => c.Subcategories)
                                .Include(c => c.Parents)
                                .FirstOrDefault(c => c.Name == name);
                if (category != null)
                {
                    Logger.Info($"Category '{name}' retrieved successfully.");
                }
                else
                {
                    Logger.Warn($"Category '{name}' not found.");
                }
                return category;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while retrieving category by name: {name}", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing category in the data store.
        /// </summary>
        /// <param name="category">The category to update.</param>
        public void Update(Category category)
        {
            Logger.Info($"Updating category with ID: {category.Id}");
            try
            {
                this.databaseContext.Entry(category).State = EntityState.Modified;
                this.databaseContext.SaveChanges();
                Logger.Info($"Category with ID: {category.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while updating category with ID: {category.Id}", ex);
                throw;
            }
        }
    }
}