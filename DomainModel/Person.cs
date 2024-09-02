// <copyright file="Person.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

// Person.cs
namespace DomainModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a person who can initiate and manage auctions, with a score reflecting their reliability.
    /// </summary>
    public class Person : IPerson
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        /// <param name="name">The name of the person.</param>
        public Person(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Score = 5.0m; // Initial seriousness score
            this.ActiveAuctions = new List<Auction>();
        }

        /// <summary>
        /// Gets the name of the person.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the score of the person, representing their reliability.
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// Gets or sets the list of active auctions initiated by this person.
        /// </summary>
        public List<Auction> ActiveAuctions { get; set; }

        /// <summary>
        /// Adjusts the person's score based on feedback or auction completion.
        /// </summary>
        /// <param name="amount">The amount to adjust the score by, between -0.1 and 0.1.</param>
        public void AdjustScore(decimal amount)
        {
            this.Score = Math.Max(0, Math.Min(10, this.Score + amount));
        }

        /// <summary>
        /// Returns a string representation of the person.
        /// </summary>
        /// <returns>A string that represents the current person.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
