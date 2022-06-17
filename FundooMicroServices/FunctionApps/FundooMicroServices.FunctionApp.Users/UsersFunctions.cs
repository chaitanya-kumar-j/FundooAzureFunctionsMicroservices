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
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace FundooMicroServices.FunctionApp.Users
{
    public class UsersFunctions
    {
        private readonly ILogger<UsersFunctions> _logger;
        private readonly IUserService _userService;

        public UsersFunctions(ILogger<UsersFunctions> log, IUserService userService)
        {
            _logger = log;
            _userService = userService;
        }

        [FunctionName("UserRegestration")]
        [OpenApiOperation(operationId: "UserRegestration", tags: new[] { "Users" })]
        [OpenApiRequestBody(contentType:"application/json", bodyType:typeof(NewUserModel), Required =true, Description = "New user details.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponseModel), Description = "The OK response")]
        public async Task<IActionResult> UserRegestration(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<NewUserModel>(requestBody);

            var response = _userService.UserRegistration(data);

            return new OkObjectResult(response);
        }

        [FunctionName("UserLogin")]
        [OpenApiOperation(operationId: "UserLogin", tags: new[] { "Login" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(LoginCredentialsModel), Required = true, Description = "New user details.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoginResponseModel), Description = "The OK response")]
        public async Task<IActionResult> UserLogin(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/login")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<LoginCredentialsModel>(requestBody);

            var response = await _userService.UserLogin(data);
            if (response == null)
            {
                return new BadRequestObjectResult("No user with given login credentials. Email and/or password is not correct.");
            }

            return new OkObjectResult(response);
        }
    }
}

