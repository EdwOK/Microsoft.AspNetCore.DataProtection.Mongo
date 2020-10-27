using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using MongoDB.Driver;

namespace Microsoft.AspNetCore.DataProtection.Mongo
{
    /// <summary>
    /// An XML repository backed by a Mongo list entry.
    /// </summary>
    public class MongoXmlRepository : IXmlRepository
    {
        private const string DataProtectionCollectionName = "DataProtectionKeys";

        private readonly Func<IMongoDatabase> _databaseFactory;

        /// <summary>
        /// Creates a <see cref="MongoXmlRepository"/> with keys stored at the given directory.
        /// </summary>
        /// <param name="databaseFactory">The delegate used to create <see cref="IMongoDatabase"/> instances.</param>
        public MongoXmlRepository(Func<IMongoDatabase> databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<XElement> GetAllElements()
        {
            var database = _databaseFactory();
            var collection = database.GetCollection<string>(DataProtectionCollectionName);

            return GetAllElementsCore().ToList().AsReadOnly();

            IEnumerable<XElement> GetAllElementsCore()
            {
                var elements = collection
                    .Find(FilterDefinition<string>.Empty)
                    .ToCursor();

                foreach (var value in elements.ToEnumerable())
                {
                    yield return XElement.Parse(value);
                }
            }
        }

        /// <inheritdoc />
        public void StoreElement(XElement element, string friendlyName)
        {
            var database = _databaseFactory();
            var collection = database.GetCollection<string>(DataProtectionCollectionName);

            collection.InsertOne(element.ToString(SaveOptions.DisableFormatting));
        }
    }
}
