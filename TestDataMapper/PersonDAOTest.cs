// <copyright file="PersonDAOTest.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper.Tests
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using DataMapper.DAO;
    using DomainModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the <see cref="PersonDAO"/> class.
    /// </summary>
    [TestClass]
    public class PersonDAOTest
    {
        /// <summary>
        /// Represents the database context used for interacting with the database in the tests.
        /// </summary>
        /// <remarks>
        /// This field is typically initialized with an instance of <see cref="DbContext"/> and is used to manage the database connection
        /// and perform CRUD operations in the tests.
        /// </remarks>
        private DbContext context;

        /// <summary>
        /// Represents the data access object (DAO) for managing operations related to <see cref="Person"/> entities in the tests.
        /// </summary>
        /// <remarks>
        /// This field is typically initialized with an instance of <see cref="PersonDAO"/> and is used to perform database operations such as
        /// adding, updating, deleting, and retrieving <see cref="Person"/> entities.
        /// </remarks>
        private PersonDAO personDAO;

        /// <summary>
        /// Initializes the test environment before each test method is run.
        /// This includes setting up an in-memory database and initializing DAOs.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Create an in-memory database using Effort
            var connection = Effort.DbConnectionFactory.CreateTransient();
            this.context = new AuctionManagementEfCoreDbContext(connection);

            // Initialize DAO with the in-memory context
            this.personDAO = new PersonDAO((AuctionManagementEfCoreDbContext)this.context);

            // Seed initial data
            this.SeedDatabase();
        }

        /// <summary>
        /// Tests that the <see cref="PersonDAO.Add"/> method correctly adds a new person to the database.
        /// </summary>
        [TestMethod]
        public void AddPerson_ShouldAddPersonToDatabase()
        {
            // Arrange
            var newPerson = new Person { Name = "John Doe" };

            // Act
            this.personDAO.Add(newPerson);

            // Assert
            var addedPerson = this.context.Set<Person>().Find(newPerson.Id);
            Assert.IsNotNull(addedPerson);
            Assert.AreEqual("John Doe", addedPerson.Name);
        }

        /// <summary>
        /// Tests that the <see cref="PersonDAO.Update"/> method correctly updates an existing person in the database.
        /// </summary>
        [TestMethod]
        public void UpdatePerson_ShouldUpdatePersonInDatabase()
        {
            // Arrange
            var personToAdd = new Person { Name = "John Doe" };
            this.personDAO.Add(personToAdd);

            // Modify the person
            personToAdd.Name = "Jane Doe";

            // Act
            this.personDAO.Update(personToAdd);

            // Assert
            var updatedPerson = this.context.Set<Person>().Find(personToAdd.Id);
            Assert.IsNotNull(updatedPerson);
            Assert.AreEqual("Jane Doe", updatedPerson.Name);
        }

        /// <summary>
        /// Tests that the <see cref="PersonDAO.Delete"/> method correctly removes a person from the database.
        /// </summary>
        [TestMethod]
        public void DeletePerson_ShouldRemovePersonFromDatabase()
        {
            // Arrange
            var personToDelete = new Person { Name = "John Doe" };
            this.personDAO.Add(personToDelete);

            // Act
            this.personDAO.Delete(personToDelete.Id);

            // Assert
            var deletedPerson = this.context.Set<Person>().Find(personToDelete.Id);
            Assert.IsNull(deletedPerson);
        }

        /// <summary>
        /// Tests that the <see cref="PersonDAO.Get"/> method correctly retrieves a person by their ID from the database.
        /// </summary>
        [TestMethod]
        public void GetPerson_ShouldReturnCorrectPerson()
        {
            // Arrange
            var expectedPerson = new Person { Name = "John Doe" };
            this.personDAO.Add(expectedPerson);

            // Act
            var person = this.personDAO.Get(expectedPerson.Id);

            // Assert
            Assert.IsNotNull(person);
            Assert.AreEqual("John Doe", person.Name);
        }

        /// <summary>
        /// Seeds the in-memory database with initial person data required for testing.
        /// </summary>
        private void SeedDatabase()
        {
            var initialPeople = new List<Person>
            {
                new Person { Name = "John Doe" },
                new Person { Name = "Jane Doe" },
            };

            foreach (var person in initialPeople)
            {
                this.personDAO.Add(person);
            }
        }
    }
}
