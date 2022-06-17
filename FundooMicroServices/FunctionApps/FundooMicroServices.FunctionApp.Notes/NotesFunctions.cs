using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FundooMicroServices.Models.RequestModels;
using FundooMicroServices.Models.ResponseModels;
using FundooMicroServices.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace FundooMicroServices.FunctionApp.Notes
{
    public class NotesFunctions
    {
        private readonly ILogger<NotesFunctions> _logger;
        private readonly IJwtService _jwtService;
        private readonly INotesService _notesService;

        public NotesFunctions(ILogger<NotesFunctions> log, IJwtService jwtService, INotesService notesService)
        {
            _logger = log;
            _jwtService = jwtService;
            _notesService = notesService;
        }

        [FunctionName("GetAllNotes")]
        [OpenApiOperation(operationId: "GetAllNotes", tags: new[] { "Notes" })]
        [OpenApiSecurity("JWT Bearer Token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<NoteResponseModel>), Description = "The OK response")]
        public async Task<IActionResult> GetAllNotes(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "notes")] HttpRequest req)
        {
            var authResponse = _jwtService.ValidateCurrentToken(req);
            if (!authResponse.IsValid)
            {
                return new UnauthorizedResult();
            }

            var response = await _notesService.GetAllFundooNotes(authResponse.Email);
            return new OkObjectResult(response);
        }

        [FunctionName("GetNoteById")]
        [OpenApiOperation(operationId: "GetNoteById", tags: new[] { "Notes" })]
        [OpenApiSecurity("JWT Bearer Token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NoteResponseModel), Description = "The OK response")]
        public async Task<IActionResult> GetNoteById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "notes/{id}")] HttpRequest req, string id)
        {
            var authResponse = _jwtService.ValidateCurrentToken(req);
            if (!authResponse.IsValid)
            {
                return new UnauthorizedResult();
            }

            var response = await _notesService.GetFundooNoteById(authResponse.Email, id);
            return new OkObjectResult(response);
        }

        [FunctionName("CreateNote")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Notes" })]
        [OpenApiSecurity("JWT Bearer Token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(NewNoteModel), Required = true, Description = "New note details.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NoteResponseModel), Description = "The OK response")]
        public async Task<IActionResult> CreateNote(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "notes")] HttpRequest req)
        {
            var authResponse = _jwtService.ValidateCurrentToken(req);
            if (!authResponse.IsValid)
            {
                return new UnauthorizedResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<NewNoteModel>(requestBody);

            var response = _notesService.CreateFundooNote(authResponse.Email, data);
            return new OkObjectResult(response);
        }

        [FunctionName("UpdateNote")]
        [OpenApiOperation(operationId: "UpdateNote", tags: new[] { "Notes" })]
        [OpenApiSecurity("JWT Bearer Token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The id parameter")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateNoteModel), Required = true, Description = "Updated note details.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NoteResponseModel), Description = "The OK response")]
        public async Task<IActionResult> UpdateNote(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "notes/{id}")] HttpRequest req, string id)
        {
            var authResponse = _jwtService.ValidateCurrentToken(req);
            if (!authResponse.IsValid)
            {
                return new UnauthorizedResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<UpdateNoteModel>(requestBody);

            var response = _notesService.UpdateFundooNoteById(id, data);
            return new OkObjectResult(response);
        }

        [FunctionName("DeleteNote")]
        [OpenApiOperation(operationId: "DeleteNote", tags: new[] { "Notes" })]
        [OpenApiSecurity("JWT Bearer Token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The id parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NoteResponseModel), Description = "The OK response")]
        public async Task<IActionResult> DeleteNote(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "notes/{id}")] HttpRequest req, string id)
        {
            var authResponse = _jwtService.ValidateCurrentToken(req);
            if (!authResponse.IsValid)
            {
                return new UnauthorizedResult();
            }

            var response = _notesService.DeleteFundooNoteById(id);
            return new OkObjectResult(response);
        }
    }
}

