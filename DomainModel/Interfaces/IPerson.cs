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
        /// Gets or sets the unique identifier for the person.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets the name of the person.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the roles of the person in the auction process.
        /// Multiple roles can be combined using bitwise operations.
        /// </summary>
        PersonRole Role { get; set; }

        /// <summary>
        /// Gets the score of the person, representing their reliability.
        /// </summary>
        decimal Score { get; }

        /// <summary>
        /// Gets or sets the list of active auctions initiated by this person.
        /// </summary>
        List<IAuction> ActiveAuctions { get; set; }

        /// <summary>
        /// Adjusts the person's score based on feedback or auction completion.
        /// </summary>
        /// <param name="amount">The amount to adjust the score by, between -0.1 and 0.1.</param>
        void AdjustScore(decimal amount);
    }
}
