using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroServices.Models.RequestModels
{
    public class UpdateNoteModel
    {
        [JsonProperty("title")]
        public string Title { get; set; } = "";

        [JsonProperty("description")]
        public string Description { get; set; } = "";

        [JsonProperty("color")]
        public string Color { get; set; } = "";

        [JsonProperty("isPinned")]
        public bool IsPinned { get; set; }

        [JsonProperty("isArchived")]
        public bool IsArchived { get; set; }

        [JsonProperty("isTrash")]
        public bool IsTrash { get; set; }

        [JsonProperty("collaborations")]
        public List<string> Collaborations { get; set; } = new List<string>();

        [JsonProperty("labels")]
        public List<string> Labels { get; set; } = new List<string>();
    }
}
