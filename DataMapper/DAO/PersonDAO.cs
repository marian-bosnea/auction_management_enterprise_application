// <copyright file="PersonDAO.cs" company="Transilvania University of Brasov">
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
    /// Provides data access functionality for persons using EF6.
    /// </summary>
    public class PersonDAO : IPersonDAO
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Represents the EF Core database context used for performing data operations
        /// on the auction management entities. It provides access to the database and
        /// manages tracking of changes to entity objects.
        /// </summary>
        private readonly IAuctionManagementEfCoreDbContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonDAO"/> class.
        /// </summary>
        /// <param name="databaseContext">The EF6 database context to use.</param>
        public PersonDAO(IAuctionManagementEfCoreDbContext databaseContext)
        {
            this.databaseContext = databaseContext;
            Logger.Info("PersonDAO initialized.");
        }

        /// <summary>
        /// Adds a new person to the data store.
        /// </summary>
        /// <param name="user">The person to add.</param>
        public void Add(Person user)
        {
            Logger.Info($"Adding person with ID: {user.Id}");
            try
            {
                this.databaseContext.People.Add(user);
                this.databaseContext.SaveChanges();
                Logger.Info($"Person with ID: {user.Id} added successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while adding person with ID: {user.Id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes a person from the data store by their identifier.
        /// </summary>
        /// <param name="id">The identifier of the person to delete.</param>
        public void Delete(int id)
        {
            Logger.Info($"Deleting person with ID: {id}");
            try
            {
                var person = this.databaseContext.People.Find(id);
                if (person != null)
                {
                    this.databaseContext.People.Remove(person);
                    this.databaseContext.SaveChanges();
                    Logger.Info($"Person with ID: {id} deleted successfully.");
                }
                else
                {
                    Logger.Warn($"Person with ID: {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while deleting person with ID: {id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a specific person by their identifier.
        /// </summary>
        /// <param name="id">The identifier of the person to retrieve.</param>
        /// <returns>The person with the specified identifier, or null if not found.</returns>
        public Person Get(int id)
        {
            Logger.Info($"Retrieving person with ID: {id}");
            try
            {
                var person = this.databaseContext.People.FirstOrDefault(p => p.Id == id);
                if (person != null)
                {
                    Logger.Info($"Person with ID: {id} retrieved successfully.");
                }
                else
                {
                    Logger.Warn($"Person with ID: {id} not found.");
                }
                return person;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while retrieving person with ID: {id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all people from the data store.
        /// </summary>
        /// <returns>A list of all people.</returns>
        public List<Person> GetAll()
        {
            Logger.Info("Retrieving all people.");
            try
            {
                var people = this.databaseContext.People.ToList();
                Logger.Info($"Retrieved {people.Count} people.");
                return people;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while retrieving all people.", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing person in the data store.
        /// </summary>
        /// <param name="user">The person to update.</param>
        public void Update(Person user)
        {
            Logger.Info($"Updating person with ID: {user.Id}");
            try
            {
                this.databaseContext.Entry(user).State = EntityState.Modified;
                this.databaseContext.SaveChanges();
                Logger.Info($"Person with ID: {user.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while updating person with ID: {user.Id}", ex);
                throw;
            }
        }
    }
}