using AirtableApiClient;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AirtableConnector.Core
{
	public class AirtableGetter
	{
		public AirtableBase Base { get; set; }

		public AirtableGetter(string baseId, string apiKeyFile)
		{
			Base = new AirtableBase(ReadApiKey(apiKeyFile), baseId);
		}

		private static string ReadApiKey(string apiKeyFile, string key = "Airtable")
		{
			// Read the entire file
			string json = File.ReadAllText(apiKeyFile);

			// Parse the JSON data into a C# object
			JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			JsonObject data = JsonSerializer.Deserialize<JsonObject>(json, options);
			return data[key].ToString();
		}

		public Dictionary<string, List<string>> RetrieveDataFromTables(string tableName)
		{
			Dictionary<string, List<string>> data = new();
			Base.ListRecords(tableName);
			return data;
		}

		public Dictionary<string, List<string>> QueryProjectsByCity(string cityName)
		{
			Dictionary<string, List<string>> data = new();
			return data;
		}
	}
}
