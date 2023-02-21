using AirtableApiClient;
using System.Collections.Generic;

namespace AirtableConnector.Core
{
	public class AirtableGetter
	{
		public AirtableBase Base { get; set; }

		public AirtableGetter(string baseId, string apiKey)
		{
			Base = new AirtableBase(apiKey, baseId);
		}

		public Dictionary<string, List<string>> RetrieveDataFromTables()
		{
			Dictionary<string, List<string>> data = new();
			Base.ListRecords("Projects");
			Base.ListRecords("Clients");
			return data;
		}

		public Dictionary<string, List<string>> QueryProjectsByCity(string cityName)
		{
			Dictionary<string, List<string>> data = new();
			return data;
		}
	}
}
