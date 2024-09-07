// <copyright file="Person.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a person who can initiate and manage auctions, with a score reflecting their reliability.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        /// <param name="name">The name of the person.</param>
        public Person(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Score = 5.0m;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the person.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters long.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the score of the person, representing their reliability.
        /// </summary>
        [Range(0, 10, ErrorMessage = "Score must be between 0 and 10.")]
        public decimal Score { get; set; }

        /// <summary>
        /// Gets or sets the roles of the person in the auction process.
        /// Multiple roles can be combined using bitwise operations.
        /// </summary>
        [Required(ErrorMessage = "Role is required.")]
        public PersonRole Role { get; set; }

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
