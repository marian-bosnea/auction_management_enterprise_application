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
    using log4net;

    /// <summary>
    /// Provides data access functionality for auctions using EF6.
    /// </summary>
    public class AuctionDAO : IAuctionDAO
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Represents the EF Core database context used for performing data operations
        /// on the auction management entities. It provides access to the database and
        /// manages tracking of changes to entity objects.
        /// </summary>
        private readonly IAuctionManagementEfCoreDbContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionDAO"/> class.
        /// </summary>
        /// <param name="databaseContext">The EF6 database context to use.</param>
        public AuctionDAO(IAuctionManagementEfCoreDbContext databaseContext)
        {
            this.databaseContext = databaseContext;
            Logger.Info("AuctionDAO initialized.");
        }

        /// <summary>
        /// Adds a new auction to the data store.
        /// </summary>
        /// <param name="auction">The auction to add.</param>
        public void Add(Auction auction)
        {
            Logger.Info($"Adding auction with ID: {auction.Id}");
            try
            {
                this.databaseContext.Auctions.Add(auction);
                this.databaseContext.SaveChanges();
                Logger.Info($"Auction with ID: {auction.Id} added successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while adding auction with ID: {auction.Id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes an auction by its identifier.
        /// </summary>
        /// <param name="id">The auction ID.</param>
        public void Delete(int id)
        {
            Logger.Info($"Deleting auction with ID: {id}");
            try
            {
                var auction = this.databaseContext.Auctions.Find(id);
                if (auction != null)
                {
                    this.databaseContext.Auctions.Remove(auction);
                    this.databaseContext.SaveChanges();
                    Logger.Info($"Auction with ID: {id} deleted successfully.");
                }
                else
                {
                    Logger.Warn($"Auction with ID: {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while deleting auction with ID: {id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves an auction by its identifier.
        /// </summary>
        /// <param name="id">The auction ID.</param>
        /// <returns>The auction object or null if not found.</returns>
        public Auction Get(int id)
        {
            Logger.Info($"Retrieving auction with ID: {id}");
            try
            {
                var auction = this.databaseContext.Auctions
                                .Include(a => a.Product)
                                .Include(a => a.Bids)
                                .FirstOrDefault(a => a.Id == id);
                if (auction != null)
                {
                    Logger.Info($"Auction with ID: {id} retrieved successfully.");
                }
                else
                {
                    Logger.Warn($"Auction with ID: {id} not found.");
                }
                return auction;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while retrieving auction with ID: {id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all auctions.
        /// </summary>
        /// <returns>A list of all auctions.</returns>
        public List<Auction> GetAll()
        {
            Logger.Info("Retrieving all auctions.");
            try
            {
                var auctions = this.databaseContext.Auctions
                                .Include(a => a.Product)
                                .Include(a => a.Bids)
                                .ToList();
                Logger.Info($"Retrieved {auctions.Count} auctions.");
                return auctions;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while retrieving all auctions.", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all active auctions for a specific person.
        /// </summary>
        /// <param name="person">The person initiating the auction.</param>
        /// <returns>A list of active auctions for the person.</returns>
        public List<Auction> GetActiveAuctionsForPerson(Person person)
        {
            Logger.Info($"Retrieving active auctions for person ID: {person.Id}");
            try
            {
                var auctions = this.databaseContext.Auctions
                                .Where(a => a.Id == person.Id && a.EndDate > DateTime.Now)
                                .Include(a => a.Product)
                                .Include(a => a.Bids)
                                .ToList();
                Logger.Info($"Retrieved {auctions.Count} active auctions for person ID: {person.Id}");
                return auctions;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while retrieving active auctions for person ID: {person.Id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves active auctions for a person in a specific category.
        /// </summary>
        /// <param name="person">The person initiating the auction.</param>
        /// <param name="category">The category of the auctions.</param>
        /// <returns>A list of active auctions for the person in the given category.</returns>
        public List<Auction> GetActiveAuctionsForPersonInCategory(Person person, Category category)
        {
            Logger.Info($"Retrieving active auctions for person ID: {person.Id} in category: {category.Name}");
            try
            {
                var auctions = this.databaseContext.Auctions
                                .Where(a => a.Id == person.Id && a.EndDate > DateTime.Now && a.Product.Categories.Contains(category))
                                .Include(a => a.Product)
                                .Include(a => a.Bids)
                                .ToList();
                Logger.Info($"Retrieved {auctions.Count} active auctions for person ID: {person.Id} in category: {category.Name}");
                return auctions;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while retrieving active auctions for person ID: {person.Id} in category: {category.Name}", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all auctions initiated by a specific person.
        /// </summary>
        /// <param name="person">The person who initiated the auctions.</param>
        /// <returns>A list of auctions for the person.</returns>
        public List<Auction> GetAuctionsForPerson(Person person)
        {
            Logger.Info($"Retrieving auctions for person ID: {person.Id}");
            try
            {
                var auctions = this.databaseContext.Auctions
                                .Where(a => a.Seller.Id == person.Id)
                                .Include(a => a.Product)
                                .Include(a => a.Bids)
                                .ToList();
                Logger.Info($"Retrieved {auctions.Count} auctions for person ID: {person.Id}");
                return auctions;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while retrieving auctions for person ID: {person.Id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing auction.
        /// </summary>
        /// <param name="auction">The auction to update.</param>
        public void Update(Auction auction)
        {
            Logger.Info($"Updating auction with ID: {auction.Id}");
            try
            {
                var existingAuction = this.databaseContext.Auctions.Find(auction.Id);
                if (existingAuction != null)
                {
                    this.databaseContext.Entry(existingAuction).CurrentValues.SetValues(auction);
                    this.databaseContext.SaveChanges();
                    Logger.Info($"Auction with ID: {auction.Id} updated successfully.");
                }
                else
                {
                    Logger.Warn($"Auction with ID: {auction.Id} not found for update.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while updating auction with ID: {auction.Id}", ex);
                throw;
            }
        }
    }
}