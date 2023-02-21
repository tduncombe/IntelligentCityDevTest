using AirtableConnector.Core;
using System.Collections.Generic;
using Xunit;

namespace AirtableConnector.Tests
{
	public class AirtableGetterTest
	{
		AirtableGetter Getter { get; set; } = new AirtableGetter("apphruxl9mXWH7QJJ", "API_KEY");

		[Fact]
		public void RetrieveDataFromTableTest()
		{
			Dictionary<string, List<string>> data = Getter.RetrieveDataFromTables();
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
