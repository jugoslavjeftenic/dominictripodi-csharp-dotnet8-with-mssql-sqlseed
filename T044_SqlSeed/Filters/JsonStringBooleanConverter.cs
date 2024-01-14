using System.Text.Json;
using System.Text.Json.Serialization;

namespace T044_SqlSeed.Filters
{
	internal class JsonStringBooleanConverter : JsonConverter<bool>
	{
		public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.String)
			{
				if (bool.TryParse(reader.GetString(), out bool result))
				{
					return result;
				}
			}

			// Fallback to default handling.
			return reader.GetBoolean();
		}

		public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
		{
			// Write boolean as a string.
			writer.WriteStringValue(value.ToString());
		}
	}
}
