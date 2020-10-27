using System;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Microsoft.AspNetCore.DataProtection.Mongo
{
    public static class MongoDataProtectionExtensions
    {
        /// <summary>
        /// Configures the data protection system to persist keys to specified key in Mongo database
        /// </summary>
        /// <param name="builder">The builder instance to modify.</param>
        /// <param name="databaseFactory">The delegate used to create <see cref="IMongoDatabase"/> instances.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        public static IDataProtectionBuilder PersistKeysToMongo(this IDataProtectionBuilder builder, Func<IMongoDatabase> databaseFactory)
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
                options.XmlRepository = new MongoXmlRepository(databaseFactory);
            });

            return builder;
        }
    }
}
