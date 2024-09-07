// <copyright file="CategoryDAO.cs" company="Transilvania University of Brasov">
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
    /// Provides data access functionality for categories using EF6.
    /// </summary>
    public class CategoryDAO : ICategoryDAO
    {
        /// <summary>
        /// Represents the EF Core database context used for performing data operations
        /// on the auction management entities. It provides access to the database and
        /// manages tracking of changes to entity objects.
        /// </summary>
        private readonly AuctionManagementEfCoreDbContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryDAO"/> class.
        /// </summary>
        /// <param name="databaseContext">The EF6 database context to use.</param>
        public CategoryDAO(AuctionManagementEfCoreDbContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Adds a new category to the data store.
        /// </summary>
        /// <param name="category">The category to add.</param>
        public void Add(Category category)
        {
            this.databaseContext.Categories.Add(category);
            this.databaseContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a category by its identifier.
        /// </summary>
        /// <param name="id">The category ID.</param>
        public void Delete(int id)
        {
            var category = this.databaseContext.Categories.Find(id);
            if (category != null)
            {
                this.databaseContext.Categories.Remove(category);
                this.databaseContext.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves a category by its identifier.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>The category object or null if not found.</returns>
        public Category Get(int id)
        {
            return this.databaseContext.Categories
                            .Include(c => c.Subcategories)
                            .Include(c => c.Parents)
                            .FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        public List<Category> GetAll()
        {
            return this.databaseContext.Categories
                            .Include(c => c.Subcategories)
                            .Include(c => c.Parents)
                            .ToList();
        }

        /// <summary>
        /// Retrieves a category by its name.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <returns>The category object or null if not found.</returns>
        public Category GetByName(string name)
        {
            return this.databaseContext.Categories
                            .Include(c => c.Subcategories)
                            .Include(c => c.Parents)
                            .FirstOrDefault(c => c.Name == name);
        }

        /// <summary>
        /// Updates an existing category in the data store.
        /// </summary>
        /// <param name="category">The category to update.</param>
        public void Update(Category category)
        {
            this.databaseContext.Entry(category).State = EntityState.Modified;
            this.databaseContext.SaveChanges();
        }
    }
}
