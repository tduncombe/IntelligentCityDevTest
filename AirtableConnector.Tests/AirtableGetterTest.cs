using AirtableConnector.Core;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace AirtableConnector.Tests
{
	public class AirtableGetterTest
	{
		// assume secrets.json as been copied to output directory
		AirtableGetter Getter { get; set; } = new AirtableGetter("apphruxl9mXWH7QJJ",
			Path.Combine(Directory.GetCurrentDirectory(), "secrets.json"));

		[Fact]
		public void RetrieveDataFromProjectsTableTest()
		{
			Dictionary<string, List<string>> data = Getter.RetrieveDataFromTables("Projects");
			Assert.NotEmpty(data);
		}

		[Fact]
		public void RetrieveDataFromClientsTableTest()
		{
			Dictionary<string, List<string>> data = Getter.RetrieveDataFromTables("Client");
			Assert.NotEmpty(data);
		}

		[Fact]
		public void QueryProjectsByCityTest()
		{
			Dictionary<string, List<string>> data = Getter.QueryProjectsByCity("Vancouver");
			Assert.True(data.Count == 6);
		}
	}
}
