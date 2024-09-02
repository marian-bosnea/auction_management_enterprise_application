// <copyright file="IPerson.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the contract for a person who can initiate and manage auctions.
    /// </summary>
    public interface IPerson
    {
        /// <summary>
        /// Gets the name of the person.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the score of the person, representing their reliability.
        /// </summary>
        decimal Score { get; }

        /// <summary>
        /// Gets or sets the list of active auctions initiated by this person.
        /// </summary>
        List<Auction> ActiveAuctions { get; set; }
    }
}
