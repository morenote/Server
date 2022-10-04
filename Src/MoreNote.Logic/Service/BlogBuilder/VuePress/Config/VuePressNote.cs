using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.BlogBuilder.VuePress.Config
{
    public class VuePressNote
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("link")]
        public string Link { get; set; }
        [JsonPropertyName("children")]
        public string Children { get; set; }
    }
}
