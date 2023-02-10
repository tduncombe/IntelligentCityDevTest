using AirtableApiClient;
using System.Collections.Generic;

namespace AirtableConnector
{
	public class AirtableGetter
	{
		public AirtableBase Base { get; set; }
		public string ApiKey { get; set; }

		public AirtableGetter(string baseId)
		{
			Base = new AirtableBase(ApiKey, baseId);
		}

		public Dictionary<string, List<string>> RetrieveDataFromTables()
		{
			Dictionary<string, List<string>> data = new();
			Base.ListRecords("Projects");
			return data;
		}
	}
}
