using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroServices.Models.ResponseModels
{
    public class JwtResponseModel
    {
        public bool IsValid { get; set; }

        public string UserId { get; set; } = "";

        public string Email { get; set; } = "";
    }
}
