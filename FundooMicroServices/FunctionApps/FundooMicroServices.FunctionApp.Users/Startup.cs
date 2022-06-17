using FundooMicroServices.FunctionApp.Users;
using FundooMicroServices.Shared.Interfaces;
using FundooMicroServices.Shared.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Startup))]

namespace FundooMicroServices.FunctionApp.Users
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            builder.Services.AddSingleton<IUserService, UserService>();

            builder.Services.AddSingleton<IJwtService, JwtService>();

            builder.Services.AddSingleton<ISettingsService, SettingsService>();
        }
    }
}
