// <copyright file="AuctionDAO.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper.DAO
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using DataMapper.Interfaces;
    using DomainModel;

    /// <summary>
    /// Provides data access functionality for auctions using EF6.
    /// </summary>
    public class AuctionDAO : IAuctionDAO
    {
        /// <summary>
        /// Represents the EF Core database context used for performing data operations
        /// on the auction management entities. It provides access to the database and
        /// manages tracking of changes to entity objects.
        /// </summary>
        private readonly AuctionManagementEfCoreDbContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionDAO"/> class.
        /// </summary>
        /// <param name="databaseContext">The EF6 database context to use.</param>
        public AuctionDAO(AuctionManagementEfCoreDbContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Adds a new auction to the data store.
        /// </summary>
        /// <param name="auction">The auction to add.</param>
        public void Add(Auction auction)
        {
            this.databaseContext.Auctions.Add(auction);
            this.databaseContext.SaveChanges();
        }

        /// <summary>
        /// Deletes an auction by its identifier.
        /// </summary>
        /// <param name="id">The auction ID.</param>
        public void Delete(int id)
        {
            var auction = this.databaseContext.Auctions.Find(id);
            if (auction != null)
            {
                this.databaseContext.Auctions.Remove(auction);
                this.databaseContext.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves an auction by its identifier.
        /// </summary>
        /// <param name="id">The auction ID.</param>
        /// <returns>The auction object or null if not found.</returns>
        public Auction Get(int id)
        {
            return this.databaseContext.Auctions
                            .Include(a => a.Product)
                            .Include(a => a.Bids)
                            .FirstOrDefault(a => a.Id == id);
        }

        /// <summary>
        /// Gets all auctions.
        /// </summary>
        /// <returns>A list of all auctions.</returns>
        public List<Auction> GetAll()
        {
            return this.databaseContext.Auctions
                            .Include(a => a.Product)
                            .Include(a => a.Bids)
                            .ToList();
        }

        /// <summary>
        /// Retrieves all active auctions for a specific person.
        /// </summary>
        /// <param name="person">The person initiating the auction.</param>
        /// <returns>A list of active auctions for the person.</returns>
        public List<Auction> GetActiveAuctionsForPerson(Person person)
        {
            return this.databaseContext.Auctions
                            .Where(a => a.Id == person.Id && a.EndDate > DateTime.Now)
                            .Include(a => a.Product)
                            .Include(a => a.Bids)
                            .ToList();
        }

        /// <summary>
        /// Retrieves active auctions for a person in a specific category.
        /// </summary>
        /// <param name="person">The person initiating the auction.</param>
        /// <param name="category">The category of the auctions.</param>
        /// <returns>A list of active auctions for the person in the given category.</returns>
        public List<Auction> GetActiveAuctionsForPersonInCategory(Person person, Category category)
        {
            return this.databaseContext.Auctions
                            .Where(a => a.Id == person.Id && a.EndDate > DateTime.Now && a.Product.Categories.Contains(category))
                            .Include(a => a.Product)
                            .Include(a => a.Bids)
                            .ToList();
        }

        /// <summary>
        /// Retrieves all auctions initiated by a specific person.
        /// </summary>
        /// <param name="person">The person who initiated the auctions.</param>
        /// <returns>A list of auctions for the person.</returns>
        public List<Auction> GetAuctionsForPerson(Person person)
        {
            return this.databaseContext.Auctions
                            .Where(a => a.Id == person.Id)
                            .Include(a => a.Product)
                            .Include(a => a.Bids)
                            .ToList();
        }

        /// <summary>
        /// Updates an existing auction.
        /// </summary>
        /// <param name="auction">The auction to update.</param>
        public void Update(Auction auction)
        {
            this.databaseContext.Entry(auction).State = EntityState.Modified;
            this.databaseContext.SaveChanges();
        }
    }
}
