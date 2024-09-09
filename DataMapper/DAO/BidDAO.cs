// <copyright file="BidDAO.cs" company="Transilvania University of Brasov">
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
    /// Provides data access functionality for bids using EF6.
    /// </summary>
    public class BidDAO : IBidDAO
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Represents the EF Core database context used for performing data operations
        /// on the auction management entities. It provides access to the database and
        /// manages tracking of changes to entity objects.
        /// </summary>
        private readonly IAuctionManagementEfCoreDbContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BidDAO"/> class.
        /// </summary>
        /// <param name="databaseContext">The EF6 database context to use.</param>
        public BidDAO(IAuctionManagementEfCoreDbContext databaseContext)
        {
            this.databaseContext = databaseContext;
            Logger.Info("BidDAO initialized.");
        }

        /// <summary>
        /// Adds a new bid to the data store.
        /// </summary>
        /// <param name="bid">The bid to add.</param>
        public void Add(Bid bid)
        {
            Logger.Info($"Adding bid with ID: {bid.Id}");
            try
            {
                this.databaseContext.Bids.Add(bid);
                this.databaseContext.SaveChanges();
                Logger.Info($"Bid with ID: {bid.Id} added successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while adding bid with ID: {bid.Id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes a bid by its identifier.
        /// </summary>
        /// <param name="id">The bid ID.</param>
        public void Delete(int id)
        {
            Logger.Info($"Deleting bid with ID: {id}");
            try
            {
                var bid = this.databaseContext.Bids.Find(id);
                if (bid != null)
                {
                    this.databaseContext.Bids.Remove(bid);
                    this.databaseContext.SaveChanges();
                    Logger.Info($"Bid with ID: {id} deleted successfully.");
                }
                else
                {
                    Logger.Warn($"Bid with ID: {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while deleting bid with ID: {id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a bid by its identifier.
        /// </summary>
        /// <param name="id">The bid ID.</param>
        /// <returns>The bid object or null if not found.</returns>
        public Bid Get(int id)
        {
            Logger.Info($"Retrieving bid with ID: {id}");
            try
            {
                var bid = this.databaseContext.Bids
                             .Include(b => b.Bidder)
                             .FirstOrDefault(b => b.Id == id);
                if (bid != null)
                {
                    Logger.Info($"Bid with ID: {id} retrieved successfully.");
                }
                else
                {
                    Logger.Warn($"Bid with ID: {id} not found.");
                }
                return bid;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while retrieving bid with ID: {id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all bids.
        /// </summary>
        /// <returns>A list of all bids.</returns>
        public List<Bid> GetAll()
        {
            Logger.Info("Retrieving all bids.");
            try
            {
                var bids = this.databaseContext.Bids
                                .Include(b => b.Bidder)
                                .ToList();
                Logger.Info($"Retrieved {bids.Count} bids.");
                return bids;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while retrieving all bids.", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing bid in the data store.
        /// </summary>
        /// <param name="bid">The bid to update.</param>
        public void Update(Bid bid)
        {
            Logger.Info($"Updating bid with ID: {bid.Id}");
            try
            {
                this.databaseContext.Entry(bid).State = EntityState.Modified;
                this.databaseContext.SaveChanges();
                Logger.Info($"Bid with ID: {bid.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while updating bid with ID: {bid.Id}", ex);
                throw;
            }
        }
    }
}