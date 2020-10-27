using System;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Microsoft.AspNetCore.DataProtection.Mongo
{
    /// <summary>
    /// Contains Mongo-specific extension methods for modifying a <see cref="IDataProtectionBuilder"/>.
    /// </summary>
    public static class MongoDataProtectionExtensions
    {
        private const string DefaultDataProtectionCollectionName = "DataProtectionKeys";

        /// <summary>
        /// Configures the data protection system to persist keys to specified key in Mongo database
        /// </summary>
        /// <param name="builder">The builder instance to modify.</param>
        /// <param name="databaseFactory">The delegate used to create <see cref="IMongoDatabase"/> instances.</param>
        /// <param name="collectionName">The name of the data protections collection.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        public static IDataProtectionBuilder PersistKeysToMongo(this IDataProtectionBuilder builder, Func<IMongoDatabase> databaseFactory, 
            string collectionName = DefaultDataProtectionCollectionName)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (databaseFactory == null)
            {
                throw new ArgumentNullException(nameof(databaseFactory));
            }

            builder.Services.Configure<KeyManagementOptions>(options =>
            {
                options.XmlRepository = new MongoXmlRepository(databaseFactory, collectionName);
            });

            return builder;
        }
    }
}
