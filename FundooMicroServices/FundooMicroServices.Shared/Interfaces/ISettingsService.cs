using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroServices.Shared.Interfaces
{
    public interface ISettingsService
    {
        string GetDocDbEndpointUri();
        string GetDocDbApiKey();
        string GetDocDbConnectionString();
        string GetDocDbDatabaseName();
        string GetDocDbCollectionName();
        int? GetDocDbThroughput();
    }
}
