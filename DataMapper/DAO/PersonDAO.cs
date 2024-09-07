// <copyright file="PersonDAO.cs" company="Transilvania University of Brasov">
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
    /// Provides data access functionality for persons using EF6.
    /// </summary>
    public class PersonDAO : IPersonDAO
    {
        /// <summary>
        /// Represents the EF Core database context used for performing data operations
        /// on the auction management entities. It provides access to the database and
        /// manages tracking of changes to entity objects.
        /// </summary>
        private readonly AuctionManagementEfCoreDbContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonDAO"/> class.
        /// </summary>
        /// <param name="databaseContext">The EF6 database context to use.</param>
        public PersonDAO(AuctionManagementEfCoreDbContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Adds a new person to the data store.
        /// </summary>
        /// <param name="user">The person to add.</param>
        public void Add(Person user)
        {
            this.databaseContext.People.Add(user);
            this.databaseContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a person from the data store by their identifier.
        /// </summary>
        /// <param name="id">The identifier of the person to delete.</param>
        public void Delete(int id)
        {
            var person = this.databaseContext.People.Find(id);
            if (person != null)
            {
                this.databaseContext.People.Remove(person);
                this.databaseContext.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves a specific person by their identifier.
        /// </summary>
        /// <param name="id">The identifier of the person to retrieve.</param>
        /// <returns>The person with the specified identifier, or null if not found.</returns>
        public Person Get(int id)
        {
            return this.databaseContext.People.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Retrieves all people from the data store.
        /// </summary>
        /// <returns>A list of all people.</returns>
        public List<Person> GetAll()
        {
            return this.databaseContext.People.ToList();
        }

        /// <summary>
        /// Updates an existing person in the data store.
        /// </summary>
        /// <param name="user">The person to update.</param>
        public void Update(Person user)
        {
            this.databaseContext.Entry(user).State = EntityState.Modified;
            this.databaseContext.SaveChanges();
        }
    }
}
