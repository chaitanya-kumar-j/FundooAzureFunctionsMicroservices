﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroServices.Models.RequestModels
{
    public class NewNoteModel
    {
        [JsonProperty("title"), JsonRequired]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("color")]
        public string Color { get; set; } = string.Empty;

        [JsonProperty("isPinned")]
        public bool IsPinned { get; set; } = false;

        [JsonProperty("isArchived")]
        public bool IsArchived { get; set; } = false;

        [JsonProperty("isTrash")]
        public bool IsTrash { get; set; } = false;

        [JsonProperty("collaborations")]
        public List<string> Collaborations { get; set; } = new List<string>();

        [JsonProperty("labels")]
        public List<string> Labels { get; set; } = new List<string>();

    }
}
