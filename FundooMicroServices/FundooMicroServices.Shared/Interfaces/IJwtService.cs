using FundooMicroServices.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroServices.Shared.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string email);
        JwtResponseModel ValidateCurrentToken(HttpRequest req);
    }
}
