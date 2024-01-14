using System.Text.Json.Serialization;
using T044_SqlSeed.Filters;

namespace T044_SqlSeed.Models
{
	internal partial class UserModel
	{
		public int UserId { get; set; }
		public string FirstName { get; set; } = "";
		public string LastName { get; set; } = "";
		public string Email { get; set; } = "";
		public string Gender { get; set; } = "";

		[JsonPropertyName("Active")]
		[JsonConverter(typeof(JsonStringBooleanConverter))]
		public bool Active { get; set; }
	}
}
