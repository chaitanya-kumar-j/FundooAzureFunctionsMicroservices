using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroServices.Models.RequestModels
{
    public class NewUserModel
    {
        [JsonProperty("firstName"), JsonRequired]
        public string FirstName { get; set; } = "";


        [JsonProperty("lastName"), JsonRequired]
        public string LastName { get; set; } = "";


        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; } = "";


        [JsonProperty("email"), JsonRequired]
        public string Email { get; set; } = "";


        [JsonProperty("password"), JsonRequired]
        public string Password { get; set; } = "";


        [JsonProperty("address")]
        public string Address { get; set; } = "";
    }
}
