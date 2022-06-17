using FundooMicroServices.Models.ResponseModels;
using FundooMicroServices.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroServices.Shared.Services
{
    public class JwtService : IJwtService
    {
		public string GenerateToken(string userId, string email)
		{
			var mySecret = "asdv234234^&%&^%&^hjsdfb2%%%";
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Email, email),
					new Claim("UserId", userId)
				}),
				Expires = DateTime.UtcNow.AddMinutes(15),
				SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public JwtResponseModel ValidateCurrentToken(HttpRequest req)
		{
			JwtResponseModel responseModel = new JwtResponseModel();

			var headers = req.Headers.ToDictionary(q => q.Key, q => (string)q.Value);
			var mySecret = "asdv234234^&%&^%&^hjsdfb2%%%";
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = headers["Authorization"].Split(' ').Last().ToString();
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					IssuerSigningKey = mySecurityKey,
					RequireExpirationTime = true
				}, out SecurityToken validatedToken);
			}
			catch
			{
				responseModel.IsValid = false;
				return responseModel;
			}
			var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
			var userId = securityToken.Claims.First(claim => claim.Type == "UserId").Value;
			var email = securityToken.Claims.First(claim => claim.Type == "email").Value;
			responseModel.IsValid = true;
			responseModel.UserId = userId;
			responseModel.Email = email;
			return responseModel;
		}
	}
}
