// <copyright file="Person.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using log4net;

    /// <summary>
    /// Represents a person who can initiate and manage auctions, with a score reflecting their reliability.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// The logger for logging actions in the class.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The score associated with the person, which is clamped between 0 and 10.
        /// </summary>
        private double score;

        /// <summary>
        /// The name of the person. Must be between 1 and 100 characters long.
        /// </summary>
        private string name;

        /// <summary>
        /// The role assigned to the person, indicating their position or function within the system.
        /// </summary>
        private PersonRole role;

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class using the default constructor.
        /// </summary>
        public Person()
        {
            Logger.Info("Person instance created with default constructor.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        /// <param name="name">The name of the person.</param>
        public Person(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Logger.Error("Invalid name: Name cannot be null or whitespace.");
                throw new ArgumentNullException(nameof(name));
            }

            this.name = name;
            this.score = 5.0;

            Logger.Info($"Person instance created with name: {name} and default score: {this.score}.");
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
        public string Name
        {
            get => this.name;
            set
            {
                if (value == null)
                {
                    Logger.Error("Attempted to set Name to null.");
                    throw new ArgumentNullException();
                }

                if (value.Trim().Length == 0 || value.Trim().Length > 100)
                {
                    Logger.Error("Invalid name length: Name must be between 1 and 100 characters long.");
                    throw new ArgumentException("Name must be between 1 and 100 characters long.");
                }

                Logger.Info($"Name changed from '{this.name}' to '{value}'.");
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the score of the person, representing their reliability.
        /// </summary>
        [Range(0, 10, ErrorMessage = "Score must be between 0 and 10.")]
        public double Score
        {
            get => this.score;
            set
            {
                if (value < 0.0)
                {
                    Logger.Warn("Score less than 0. Clamping to 0.");
                    this.score = 0.0;
                }
                else if (value > 10.0)
                {
                    Logger.Warn("Score greater than 10. Clamping to 10.");
                    this.score = 10.0;
                }
                else
                {
                    Logger.Info($"Score updated from {this.score} to {value}.");
                    this.score = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the roles of the person in the auction process.
        /// Multiple roles can be combined using bitwise operations.
        /// </summary>
        [Required(ErrorMessage = "Role is required.")]
        public PersonRole Role
        {
            get => this.role;
            set
            {
                if ((int)value > 2)
                {
                    Logger.Error("Invalid role: Role enum must be valid.");
                    throw new ArgumentException("Role enum must be valid");
                }

                Logger.Info($"Role changed from {this.role} to {value}.");
                this.role = value;
            }
        }

        /// <summary>
        /// Adjusts the person's score based on feedback or auction completion.
        /// </summary>
        /// <param name="amount">The amount to adjust the score by, between -0.1 and 0.1.</param>
        public void AdjustScore(double amount)
        {
            double oldScore = this.score;
            this.score = Math.Max(0, Math.Min(10, this.Score + amount));
            Logger.Info($"Score adjusted from {oldScore} to {this.score} by amount {amount}.");
        }

        /// <summary>
        /// Returns a string representation of the person.
        /// </summary>
        /// <returns>A string that represents the current person.</returns>
        public override string ToString()
        {
            Logger.Debug($"ToString called for Person with name: {this.Name}.");
            return this.Name;
        }
    }
}