﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using BookApp.Books.Infrastructure.Seeding;
using BookApp.Books.Persistence;
using BookApp.Main.FrontEnd;
using BookApp.Main.Infrastructure;
using BookApp.Orders.Persistence.EfCoreSql;
using BookApp.Main.FrontEnd.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookApp.Main.FrontEnd.HelperExtensions
{
    public static class DatabaseStartupHelpers
    {
        private const string SecretStart = "Secret|";
        private const string EmulatorString = "Emulator";

        /// <summary>
        /// This makes sure the database is created/updated
        /// </summary>
        /// <param name="webHost"></param>
        /// <returns></returns>
        public static async Task<IHost> SetupDatabaseAsync(this IHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var env = services.GetRequiredService<IWebHostEnvironment>();
                var bookContext = services.GetRequiredService<BookDbContext>();
                var orderContext = services.GetRequiredService<OrderDbContext>();
                try
                {
                    await bookContext.Database.MigrateAsync();
                    await orderContext.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while creating/migrating the SQL database.");

                    throw;
                }

                try
                {
                    if (!bookContext.Books.Any())
                    {
                        await bookContext.SeedDatabaseWithBooksAsync(env.WebRootPath);
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    throw;
                }
            }

            return webHost;
        }


        public static string GetCorrectSqlConnection(this IConfiguration config, BookAppSettings settings)
        {
            var baseConnection = config.GetConnectionString(settings.SqlConnectionString, false);

            if (baseConnection == null)
                throw new NullReferenceException($"The {settings.SqlConnectionString} setting wasn't found or is null.");

            return baseConnection;
        }

        public static CosmosDbSettings GetCosmosDbSettings(this IConfiguration config, BookAppSettings settings)
        {

            if (!settings.CosmosAvailable)
                return null;

            var connectionString = config.GetConnectionString(settings.CosmosConnectionString, true);

            return new CosmosDbSettings(connectionString, settings.CosmosDatabaseName);
        }

        private static string GetConnectionString(this IConfiguration config, string setupValue, bool cosmosConnection)
        {
            if (setupValue.StartsWith(SecretStart))
                return config[setupValue.Substring(SecretStart.Length)];
            if (setupValue == EmulatorString && cosmosConnection)
                return
                    "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

            return setupValue;
        }
    }
}