// <copyright file="Program.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

using DataMapper;
using DataMapper.DAO;
using DataMapper.Interfaces;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace AuctionManagementApplication
{
    /// <summary>
    /// The entry point class of the application.
    /// </summary>
    internal class Program
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The main entry point of the application.
        /// </summary>
        /// <param name="args">Array of command-line arguments passed to the application.</param>
        private static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            Logger.Info("Application Started");

            string connectionString = ConfigurationManager.ConnectionStrings["AuctionManagementEfCoreDbContext"].ConnectionString;


            DbConnection connection = new SqlConnection(connectionString);
            IAuctionManagementEfCoreDbContext dbContext = new AuctionManagementEfCoreDbContext(connection);
            IAuctionDAO auctionDAO = new AuctionDAO(dbContext);
            IBidDAO bidDAO = new BidDAO(dbContext);

            bidDAO.Add(new DomainModel.Bid());
        }
    }
}
