using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroServices.Models.ResponseModels
{
    public class LoginResponseModel
    {
        public UserResponseModel UserDetails { get; set; }

        public string token { get; set; }
    }
}
