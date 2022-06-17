using FundooMicroServices.Models.RequestModels;
using FundooMicroServices.Models.ResponseModels;
using FundooMicroServices.Shared.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroServices.Shared.Services
{
    public class UserService : IUserService
    {
        private readonly IJwtService _jwtService;
        ISettingsService _settingsService;

        // Cosmos DocDB API database
        private string _docDbEndpointUri;
        private string _docDbPrimaryKey;
        private string _docDbDatabaseName;

        // Doc DB Collections
        private string _docDbCollectionName;

        private static CosmosClient _cosmosClient;
        private readonly Container _cosmosContainer;



        public UserService(IJwtService jwtService, ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _jwtService = jwtService;
            _docDbEndpointUri = _settingsService.GetDocDbEndpointUri();
            _docDbPrimaryKey = _settingsService.GetDocDbApiKey();

            _docDbDatabaseName = _settingsService.GetDocDbDatabaseName();
            _docDbCollectionName = _settingsService.GetDocDbCollectionName();
            _cosmosClient = new CosmosClient(_settingsService.GetDocDbEndpointUri(), settingsService.GetDocDbApiKey());
            _cosmosContainer = _cosmosClient.GetContainer(_docDbDatabaseName, _docDbCollectionName);
            /*_cosmosContainer = new Lazy<Task<Container>>(async () =>
            {
                var cosmos = new CosmosClient(settingsService.GetDocDbEndpointUri(), settingsService.GetDocDbApiKey());
                var db = cosmos.GetDatabase(settingsService.GetDocDbFundooNotesDatabaseName());
                //TODO: Hardcoded partition key field here
                return await db.CreateContainerIfNotExistsAsync(settingsService.GetDocDbMainCollectionName(),"/email");
            });*/
        }

        #region Get All Users
        public async Task<List<UserResponseModel>> GetUsers()
        {
            try
            {
                if (string.IsNullOrEmpty(_docDbCollectionName))
                    throw new Exception("No Digital Main collection defined!");

                using (var query = _cosmosContainer.GetItemLinqQueryable<UserResponseModel>()
                                .OrderByDescending(e => e.RegisteredAt)
                                .ToFeedIterator())
                {
                    List<UserResponseModel> allDocuments = new List<UserResponseModel>();
                    while (query.HasMoreResults)
                    {
                        var queryResult = await query.ReadNextAsync();

                        allDocuments.AddRange(queryResult.ToList());
                    }

                    return allDocuments;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region User Registration
        public async Task<UserResponseModel> UserRegistration(NewUserModel newUserDetails)
        {
            if (newUserDetails == null)
            {
                throw new ArgumentNullException(nameof(newUserDetails));
            }

            try
            {
                UserResponseModel newUser = new UserResponseModel()
                {
                    RegisteredAt = Convert.ToString(DateTime.Now),
                    UserId = Guid.NewGuid().ToString(),
                    Address = newUserDetails.Address,
                    Email = newUserDetails.Email,
                    FirstName = newUserDetails.FirstName,
                    LastName = newUserDetails.LastName,
                    MobileNumber = newUserDetails.MobileNumber,
                    Password = newUserDetails.Password
                };
                

                using (var response = _cosmosContainer.CreateItemAsync(newUser, new PartitionKey(newUser.Email)))
                {
                    return response.Result.Resource;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
        #endregion

        #region User Login
        public async Task<LoginResponseModel> UserLogin(LoginCredentialsModel userLoginDetails)
        {
            LoginResponseModel loginResponse = new LoginResponseModel();
            try
            {
                if (string.IsNullOrEmpty(userLoginDetails.Email))
                    throw new Exception("Email is null or empty string!!");

                if (string.IsNullOrEmpty(_docDbCollectionName))
                    throw new Exception("No Digital Main collection defined!");

                var user = _cosmosContainer.GetItemLinqQueryable<UserResponseModel>(true)
                     .Where(u => u.Email == userLoginDetails.Email && u.Password == userLoginDetails.Password)
                     .AsEnumerable()
                     .FirstOrDefault();
                if (user == null)
                {
                    return null;
                }
                loginResponse.UserDetails = user;
                loginResponse.token = _jwtService.GenerateToken(loginResponse.UserDetails.UserId, loginResponse.UserDetails.Email);
                return loginResponse;
            }
            catch (Exception ex)
            {
                // Detect a `Resource Not Found` exception...do not treat it as error
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) && ex.InnerException.Message.IndexOf("Resource Not Found") != -1)
                {
                    return null;
                }
                else
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }
        #endregion
    }
}
