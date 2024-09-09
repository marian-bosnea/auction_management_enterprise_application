// <copyright file="Program.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace AuctionManagementApplication
{
    using System.Configuration;
    using System.Data.Common;
    using System.Data.SqlClient;
    using DataMapper;
    using DataMapper.DAO;
    using DataMapper.Interfaces;

    /// <summary>
    /// The entry point class of the application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Logger instance for logging operations within the <see cref="StringUtils"/> class.
        /// </summary>
        /// <remarks>
        /// This static readonly field is used to log information, warnings, errors, and other messages related to string operations.
        /// It utilizes the log4net library for logging, and the logger is configured to log messages based on the class's namespace and type.
        /// </remarks>
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
            IAuctionManagementEfCoreDbContext databaseContext = new AuctionManagementEfCoreDbContext(connection);
            IAuctionDAO auctionDAO = new AuctionDAO(databaseContext);
            IBidDAO bidDAO = new BidDAO(databaseContext);

            bidDAO.Add(new DomainModel.Bid());
        }
    }
}
