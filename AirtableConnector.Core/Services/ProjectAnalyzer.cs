using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirtableConnector.Core.Abstractions;

namespace AirtableConnector.Core.Services
{
    ///<summary>
    /// Example class to be demonstrate unit testing with 
    /// dependencies abstracted.
    /// It doesn't really do anything, I created it to have an example of 
    /// unit tests in order to contrast against integration testing.
    ///</summary>
    public class ProjectAnalyzer
    {
        private IRepository _repository;

        public ProjectAnalyzer(IRepository repo)
        {
            if (repo is null)
                throw new ArgumentNullException(nameof(repo));

            _repository = repo;
        }

        /// Simple method to represent applying some business logic 
        /// on top of our data layer.
        /// As such I haven't bothered with any error handling.
        public async Task<decimal> TotalVancouverProjectCost() => 
            (await _repository.RetrieveProjectsByCity("Vancouver"))
                .Sum(p => p.TotalCost);
    }
}