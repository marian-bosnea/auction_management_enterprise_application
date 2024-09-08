using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataMapper.DAO;
using DomainModel;

namespace DataMapper.Tests
{
    [TestClass]
    public class PersonDAOTests
    {
        private DbContext _context;
        private PersonDAO _personDAO;

        [TestInitialize]
        public void Setup()
        {
            // Create an in-memory database using Effort
            var connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new AuctionManagementEfCoreDbContext(connection);

            // Initialize DAO with the in-memory context
            _personDAO = new PersonDAO((AuctionManagementEfCoreDbContext)_context);

            // Seed initial data
            SeedDatabase();
        }

        [TestMethod]
        public void AddPerson_ShouldAddPersonToDatabase()
        {
            // Arrange
            var newPerson = new Person { Name = "John Doe" };

            // Act
            _personDAO.Add(newPerson);

            // Assert
            var addedPerson = _context.Set<Person>().Find(newPerson.Id);
            Assert.IsNotNull(addedPerson);
            Assert.AreEqual("John Doe", addedPerson.Name);
        }

        [TestMethod]
        public void UpdatePerson_ShouldUpdatePersonInDatabase()
        {
            // Arrange
            var personToAdd = new Person { Name = "John Doe" };
            _personDAO.Add(personToAdd);

            // Modify the person
            personToAdd.Name = "Jane Doe";

            // Act
            _personDAO.Update(personToAdd);

            // Assert
            var updatedPerson = _context.Set<Person>().Find(personToAdd.Id);
            Assert.IsNotNull(updatedPerson);
            Assert.AreEqual("Jane Doe", updatedPerson.Name);
        }

        [TestMethod]
        public void DeletePerson_ShouldRemovePersonFromDatabase()
        {
            // Arrange
            var personToDelete = new Person { Name = "John Doe" };
            _personDAO.Add(personToDelete);

            // Act
            _personDAO.Delete(personToDelete.Id);

            // Assert
            var deletedPerson = _context.Set<Person>().Find(personToDelete.Id);
            Assert.IsNull(deletedPerson);
        }

        [TestMethod]
        public void GetPerson_ShouldReturnCorrectPerson()
        {
            // Arrange
            var expectedPerson = new Person { Name = "John Doe" };
            _personDAO.Add(expectedPerson);

            // Act
            var person = _personDAO.Get(expectedPerson.Id);

            // Assert
            Assert.IsNotNull(person);
            Assert.AreEqual("John Doe", person.Name);
        }

        private void SeedDatabase()
        {
            var initialPeople = new List<Person>
            {
                new Person { Name = "John Doe" },
                new Person { Name = "Jane Doe" }
            };

            foreach (var person in initialPeople)
            {
                _personDAO.Add(person);
            }
        }
    }
}
