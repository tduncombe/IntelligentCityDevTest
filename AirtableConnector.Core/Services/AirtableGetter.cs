using AirtableApiClient;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Linq;
using AirtableConnector.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using AirtableConnector.Core.Abstractions;

namespace AirtableConnector.Core.Services
{
    public class AirtableGetter : IRepository
    {
        private readonly ILogger<AirtableGetter> _logger;

        public AirtableBase _base { get; set; }

        public AirtableGetter(string baseId,
            string apiKeyFile,
            ILogger<AirtableGetter> logger)
        {
            if (string.IsNullOrWhiteSpace(baseId))
                throw new ArgumentNullException(nameof(baseId));
            if (string.IsNullOrWhiteSpace(apiKeyFile))
                throw new ArgumentNullException(nameof(apiKeyFile));
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            _base = new AirtableBase(ReadApiKey(apiKeyFile), baseId);
            _logger = logger;
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
        
        public async Task<List<Client>> RetrieveAllClientsAsync()
        {
            var clients = new List<Client>();
            var resp = await RetrieveFromAirtable("Clients", filterByFormula: null);
            clients.AddRange(resp.Records.Select(r => ClientFromRecord(r)));
            return clients;
        }

        public async Task<List<Project>> RetrieveAllProjectsAsync()
        {
            var projects = new List<Project>();
            var resp = await RetrieveFromAirtable("Projects", filterByFormula: null);
            projects.AddRange(resp.Records.Select(r => ProjectFromRecord(r)));
            return projects;
        }

        public async Task<List<Project>> RetrieveProjectsByCity(string city)
        {
            var projects = new List<Project>();
            AirtableListRecordsResponse resp = await RetrieveFromAirtable("Projects", $"City = '{city}'");
            projects.AddRange(resp.Records.Select(r => ProjectFromRecord(r)));
            return projects;
        }

        private async Task<AirtableListRecordsResponse> RetrieveFromAirtable(string table, string filterByFormula)
        {
            try
            {
                var resp = await _base.ListRecords(table, filterByFormula: filterByFormula);
                if (!resp.Success) 
                {
                    // bit gross to throw and then immediately catch
                    // but they provide us a sensible exception type we 
                    // can handle below gracefully
                    throw resp.AirtableApiError;
                }
                return resp;
            }
            catch (AirtableApiException e)
            {
                _logger.LogError(e, e.DetailedErrorMessage);
                // it is unlikely this class is able to recover from not being able to retrieve
                // from airtable. Proper error handling logic belongs in an area of the application
                // not dedicated to airtable access.
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving from Airtable");
                throw;
            }
        }

        private Client ClientFromRecord(AirtableApiClient.AirtableRecord record)
        {
            try 
            {
                return new Client
                {
                    Company = record.Fields["Company"].ToString(),
                    Name = record.Fields["Name"].ToString(),
                    Phone = record.Fields["Phone"].ToString(),
                    ProjectKeys = 
                        JsonSerializer.Deserialize<string[]>(record.Fields["Projects"].ToString())
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not create Client from Airtable record");
                // A programming error has likely occurred if we are unable to convert this 
                // record into a Client object. Burying the exception here would make
                // sneaky bugs more likely to survive but there are arguments for not throwing here.
                throw;
            }
        }

        private Project ProjectFromRecord(AirtableApiClient.AirtableRecord record)
        {
            try 
            {
                return new Project
                {
                    ProjectId = int.Parse(record.Fields["Project Id"].ToString()),
                    City = record.Fields["City"].ToString(),
                    SiteArea = int.Parse(record.Fields["Site Area"].ToString()),
                    CostSqFoot = int.Parse(record.Fields["Cost/Sqft"].ToString()),
                    GrossFloorArea = int.Parse(record.Fields["Gross Floor Area"].ToString()),
                    TotalCost = decimal.Parse(record.Fields["Total Cost"].ToString()),
                    ClientKeys = 	
                        JsonSerializer.Deserialize<string[]>(record.Fields["Clients"].ToString())
                
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not create Project from Airtable record");
                throw;
            }
        }

        ///<summary>
        /// I've left this in to show how I initially completed the task before refactoring - 
        /// if I hadn't refactored this would have proper error handling/logging.
        /// AirtableToDict was previously a private method used by both functions, I just 
        /// moved it into a local function instead.
        /// The motivation for refactoring away from this method was to contain all logic regarding
        /// the conversion of data -> objects in code in one layer of our system, and enforce
        /// a testable + injectable seam in the application.
        /// In a real application this could be a first step towards a larger refactoring of 
        /// all airtable access behind a seam without requiring the change being made in one massive commit.
        ///</summary>
        [Obsolete("All access to airtable should be made through dependency injected IRepository")]
        public async Task<Dictionary<string, List<string>>> RetrieveDataFromTablesAsync(string tableName)
        {
            var resp = await _base.ListRecords(tableName);
            return AirtableToDict(resp);

            // the desired data in this dictionary is not specified by the task
            // so we'll just use the primary key as the key and the values for each column in the list
            Dictionary<string, List<string>> AirtableToDict(AirtableListRecordsResponse resp)
            {
                Dictionary<string, List<string>> data = new();
                foreach (var record in resp.Records)
                {
                    var fieldValues = new List<string>();
                    foreach (var field in record.Fields)
                    {
                        fieldValues.Add(field.Value.ToString());
                    }
                    data.Add(record.Id, fieldValues);
                }
                return data;
            }
        }
    }
}
