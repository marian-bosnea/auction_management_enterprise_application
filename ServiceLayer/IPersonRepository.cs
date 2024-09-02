// <copyright file="IPersonRepository.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace ServiceLayer
{
    using DomainModel;

    /// <summary>
    /// Represents a repository for managing person-related data.
    /// </summary>
    public interface IPersonRepository
    {
        /// <summary>
        /// Updates the specified person in the repository.
        /// </summary>
        /// <param name="person">The person entity to be updated.</param>
        void Update(IPerson person);
    }
}
