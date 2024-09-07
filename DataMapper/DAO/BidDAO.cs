// <copyright file="BidDAO.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DataMapper.DAO
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using DataMapper.Interfaces;
    using DomainModel;

    /// <summary>
    /// Provides data access functionality for bids using EF6.
    /// </summary>
    public class BidDAO : IBidDAO
    {
        /// <summary>
        /// Represents the EF Core database context used for performing data operations
        /// on the auction management entities. It provides access to the database and
        /// manages tracking of changes to entity objects.
        /// </summary>
        private readonly AuctionManagementEfCoreDbContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BidDAO"/> class.
        /// </summary>
        /// <param name="databaseContext">The EF6 database context to use.</param>
        public BidDAO(AuctionManagementEfCoreDbContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Adds a new bid to the data store.
        /// </summary>
        /// <param name="bid">The bid to add.</param>
        public void Add(Bid bid)
        {
            this.databaseContext.Bids.Add(bid);
            this.databaseContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a bid by its identifier.
        /// </summary>
        /// <param name="id">The bid ID.</param>
        public void Delete(int id)
        {
            var bid = this.databaseContext.Bids.Find(id);
            if (bid != null)
            {
                this.databaseContext.Bids.Remove(bid);
                this.databaseContext.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves a bid by its identifier.
        /// </summary>
        /// <param name="id">The bid ID.</param>
        /// <returns>The bid object or null if not found.</returns>
        public Bid Get(int id)
        {
            return this.databaseContext.Bids
                            .Include(b => b)
                            .Include(b => b.Bidder)
                            .FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Retrieves all bids.
        /// </summary>
        /// <returns>A list of all bids.</returns>
        public List<Bid> GetAll()
        {
            return this.databaseContext.Bids
                            .Include(b => b.Bidder)
                            .ToList();
        }

        /// <summary>
        /// Updates an existing bid in the data store.
        /// </summary>
        /// <param name="bid">The bid to update.</param>
        public void Update(Bid bid)
        {
            this.databaseContext.Entry(bid).State = EntityState.Modified;
            this.databaseContext.SaveChanges();
        }
    }
}
