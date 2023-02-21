using AirtableConnector.Core;
using System.Collections.Generic;
using Xunit;

namespace AirtableConnector.Tests
{
	public class AirtableGetterTest
	{
		AirtableGetter Getter { get; set; } = new AirtableGetter("apphruxl9mXWH7QJJ", "API_KEY_FILEPATH");

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
