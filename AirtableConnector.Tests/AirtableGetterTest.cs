using AirtableConnector.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AirtableConnector.Tests
{
    public class AirtableGetterTest
    {
        private ILogger<AirtableGetter> _logger;
        private Lazy<AirtableGetter> _lazyAirtable;
        private AirtableGetter _getter => _lazyAirtable.Value;

        ///<summary>
        /// This file, in my mind, represents integration testing rather than unit testing.
        /// Directly accessing external dependencies (here, Airtable) during testing 
        /// means that tests will run more slowly, and be more liable to break. On the 
        /// plus side it also means they are probably more accurate.
        /// This means that they are better suited to be ran as a way to validate a deployment
        /// is safe, than whether a build is good. Build failures due to test failure should
        /// generally happen only when there is a problem with the code in some way.
        /// Ideally they should also be able to run almost instantly even when there are thousands,
        /// otherwise devs won't use them during development. 
        ///</summary>
        public AirtableGetterTest(ILogger<AirtableGetter> logger)
        {
            _logger = logger;
            // somewhat overkill to use lazy loading but this allows us to avoid 
            // our test harness exploding before a test has been run in the case that
            // secrets.json is not in the correct place, it also means the constructor  
            // is only ever run once across the various tests.
            _lazyAirtable = new Lazy<AirtableGetter>(() =>
                new AirtableGetter(
                    "apphruxl9mXWH7QJJ",
                    Path.Combine(Directory.GetCurrentDirectory(), "secrets.json"),
                    _logger));
        }

        // test is superfluous in that all the others will fail anyway
        // but this hopefully makes it more clear
        [Fact]
        public void SecretsAreInCorrectLocation()
        {
            // if this is failing ensure secrets.json has been placed in the output direction of
            // the application
            var getter = _getter;
            Assert.True(true);
        }

        [Fact]
        public void ConstructorValidatesArgumentsTest()
        {
            Assert.Throws<ArgumentNullException>(() => new AirtableGetter(null, "filepath", _logger));
            Assert.Throws<ArgumentNullException>(() => new AirtableGetter("baseId", null, _logger));
            Assert.Throws<ArgumentNullException>(() => new AirtableGetter("baseId", "filepath", null));
        }

        [Fact]
        public async Task RetrieveDataFromProjectsTableTest()
        {
            Dictionary<string, List<string>> data = await _getter.RetrieveDataFromTablesAsync("Projects");
            Assert.NotEmpty(data);
        }

        [Fact]
        public async Task RetrieveDataFromClientsTableTest()
        {
            Dictionary<string, List<string>> data = await _getter.RetrieveDataFromTablesAsync("Clients");
            Assert.NotEmpty(data);
        }

        [Fact]
        public async Task RetrieveAllClientsTest()
        {
            var clients = await _getter.RetrieveAllClientsAsync();
            Assert.True(clients.Count > 0);
        }

        [Fact]
        public async Task RetrieveAllProjectsTest()
        {
            var projects = await _getter.RetrieveAllProjectsAsync();
            Assert.True(projects.Count > 0);
        }

        [Fact]
        public async Task RetrieveProjectsByCityTest()
        {
            var van = "Vancouver";
            var projects = await _getter.RetrieveProjectsByCity(van);
            // prefer this logic to a basic count, less brittle
            Assert.True(projects.All(p => p.City.Equals(van, System.StringComparison.OrdinalIgnoreCase)));
        }
    }
}
