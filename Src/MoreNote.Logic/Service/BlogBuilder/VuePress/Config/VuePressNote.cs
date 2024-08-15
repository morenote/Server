using System.Text.Json.Serialization;

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
