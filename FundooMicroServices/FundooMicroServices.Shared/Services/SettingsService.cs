using FundooMicroServices.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroServices.Shared.Services
{
    public class SettingsService : ISettingsService
    {
        // Cosmos DB
        private const string DocDbEndpointUri = "DocDbEndpointUri";
        private const string DocDbApiKey = "DocDbApiKey";
        private const string DocDbConnectionString = "CosmosDBConnectionString";
        private const string DocDbDatabaseName = "DocDbDatabaseName";
        private const string DocDbCollectionName = "DocDbCollectionName";
        private const string DocDbThroughput = "DocDbThroughput";

        //*** PRIVATE Methods ***//
        private static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }

        //*** Cosmos ***//
        public string GetDocDbEndpointUri()
        {
            return GetEnvironmentVariable(DocDbEndpointUri);
        }

        public string GetDocDbApiKey()
        {
            return GetEnvironmentVariable(DocDbApiKey);
        }

        public string GetDocDbConnectionString()
        {
            return GetEnvironmentVariable(DocDbConnectionString);
        }

        public string GetDocDbDatabaseName()
        {
            return GetEnvironmentVariable(DocDbDatabaseName);
        }

        public string GetDocDbCollectionName()
        {
            return GetEnvironmentVariable(DocDbCollectionName);
        }

        public int? GetDocDbThroughput()
        {
            if (int.TryParse(GetEnvironmentVariable(DocDbThroughput), out int throughput)) return throughput;
            return null;
        }
    }
}
