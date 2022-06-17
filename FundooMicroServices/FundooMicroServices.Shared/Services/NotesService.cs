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
    public class NotesService : INotesService
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



        public NotesService(IJwtService jwtService, ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _jwtService = jwtService;
            _docDbEndpointUri = _settingsService.GetDocDbEndpointUri();
            _docDbPrimaryKey = _settingsService.GetDocDbApiKey();

            _docDbDatabaseName = _settingsService.GetDocDbDatabaseName();
            _docDbCollectionName = _settingsService.GetDocDbCollectionName();
            _cosmosClient = new CosmosClient(_settingsService.GetDocDbEndpointUri(), 
                settingsService.GetDocDbApiKey());
            _cosmosContainer = _cosmosClient.GetContainer(_docDbDatabaseName,
                _docDbCollectionName);

        }

        public async Task<NoteResponseModel> CreateFundooNote(string email, 
            NewNoteModel newFundooNote)
        {
            try
            {
                NoteResponseModel noteResponseModel = new NoteResponseModel()
                {
                    CreatedAt = Convert.ToString(DateTime.Now),
                    NoteId = Guid.NewGuid().ToString(),
                    Collaborations = new List<string>() { email},
                    Color = newFundooNote.Color,
                    Description = newFundooNote.Description,
                    IsArchived = newFundooNote.IsArchived,
                    IsPinned = newFundooNote.IsPinned,
                    IsTrash = newFundooNote.IsTrash,
                    Labels = newFundooNote.Labels,
                    Title = newFundooNote.Title
                };

                using (var response = _cosmosContainer
                    .CreateItemAsync(noteResponseModel, 
                    new PartitionKey(noteResponseModel.NoteId)))
                {
                    return response.Result.Resource;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<NoteResponseModel> DeleteFundooNoteById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(_docDbCollectionName))
                    throw new Exception("No Digital Main collection defined!");

                NoteResponseModel document = await _cosmosContainer.DeleteItemAsync<NoteResponseModel>(id, new PartitionKey(id));


                return document;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<NoteResponseModel>> GetAllFundooNotes(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(_docDbCollectionName))
                    throw new Exception("No Digital Main collection defined!");

                using (var query = _cosmosContainer.GetItemLinqQueryable<NoteResponseModel>()
                                .Where(n => n.Collaborations.Contains(email))
                                .ToFeedIterator())
                {
                    List<NoteResponseModel> allDocuments = new List<NoteResponseModel>();
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

        public async Task<NoteResponseModel> GetFundooNoteById(string email, string id)
        {
            try
            {
                if (string.IsNullOrEmpty(_docDbCollectionName))
                    throw new Exception("No Digital Main collection defined!");

                NoteResponseModel document = await _cosmosContainer.ReadItemAsync<NoteResponseModel>(id, new PartitionKey(id));

                return document;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<NoteResponseModel> UpdateFundooNoteById(string id, UpdateNoteModel updatedFundooNote)
        {
            try
            {
                if (string.IsNullOrEmpty(_docDbCollectionName))
                    throw new Exception("No Digital Main collection defined!");

                NoteResponseModel document = await _cosmosContainer.ReadItemAsync<NoteResponseModel>(id, new PartitionKey(id));
                if (document == null)
                {
                    return null;
                }
                NoteResponseModel noteResponseModel = new NoteResponseModel()
                {
                    CreatedAt = document.CreatedAt,
                    NoteId = document.NoteId,
                    Collaborations = updatedFundooNote.Collaborations,
                    Color = updatedFundooNote.Color,
                    Description = updatedFundooNote.Description,
                    IsArchived = updatedFundooNote.IsArchived,
                    IsPinned = updatedFundooNote.IsPinned,
                    IsTrash = updatedFundooNote.IsTrash,
                    Labels = updatedFundooNote.Labels,
                    Title = updatedFundooNote.Title
                };
                var response = await _cosmosContainer.ReplaceItemAsync<NoteResponseModel>(noteResponseModel, id, new PartitionKey(id));
                return response.Resource;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
