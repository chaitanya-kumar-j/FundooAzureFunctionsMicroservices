using FundooMicroServices.Models.RequestModels;
using FundooMicroServices.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroServices.Shared.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseModel>> GetUsers();

        Task<UserResponseModel> UserRegistration(NewUserModel newUserDetails);

        Task<LoginResponseModel> UserLogin(LoginCredentialsModel userLoginDetails);
    }
}
