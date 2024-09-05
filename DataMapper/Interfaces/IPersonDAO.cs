// <copyright file="IPersonDAO.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper.Interfaces
{
    using System.Collections.Generic;
    using DomainModel;

    /// <summary>
    /// Defines the data access operations for users.
    /// </summary>
    public interface IPersonDAO
    {
        /// <summary>
        /// Adds a new user to the data store.
        /// </summary>
        /// <param name="user">The user to add.</param>
        void Add(Person user);

        /// <summary>
        /// Retrieves all users from the data store.
        /// </summary>
        /// <returns>A list of all users.</returns>
        List<Person> GetAll();

        /// <summary>
        /// Retrieves a specific user by their identifier.
        /// </summary>
        /// <param name="id">The identifier of the user to retrieve.</param>
        /// <returns>The user with the specified identifier, or null if not found.</returns>
        Person Get(int id);

        /// <summary>
        /// Updates an existing user in the data store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        void Update(IPerson user);

        /// <summary>
        /// Deletes a user from the data store by their identifier.
        /// </summary>
        /// <param name="id">The identifier of the user to delete.</param>
        void Delete(int id);
    }
}
