using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AirtableConnector.Core.Abstractions;
using AirtableConnector.Core.Models;
using AirtableConnector.Core.Services;
using Xunit;

namespace AirtableConnector.Tests
{
    public class ProjectAnalyzerTests
    {
        [Fact]
        public async Task TestTotalVancouverCost()
        {
            // mock the data layer
            // we could use moq or similar lib instead for more complex scenarios 
            // but this is a simple and effective mock object 
            // AKA arrange
            var fakeRepo = new FakeRepository();
            fakeRepo.ProjectsByCity = new List<Project>()
            {
                new Project 
                {
                    City = "Vancouver",
                    TotalCost = 100,
                },
                new Project 
                {
                    City = "Vancouver",
                    TotalCost = 200,
                }
            };
            
            // act
            var projAnalyzer = new ProjectAnalyzer(fakeRepo);
            var totalCost = await projAnalyzer.TotalVancouverProjectCost();
            
            // assert 
            Assert.True(totalCost == 300);
        }
    }
}

///<summary>
/// I've chosen to use a fake object pattern for mocking dependencies
/// as opposed to a mocking framework. I have found that a lot of the times something like this
/// which allows a test to easily ask for the data it wants to receive from the data layer
/// is a lot more readable and quicker to code than the equivalent functionality using Moq.
/// Make it internal so we don't have any 'real' code using the fake repo.
///</summary>
internal class FakeRepository : IRepository
{
    public List<Project> ProjectsByCity { get; set; }
    public Task<List<Client>> RetrieveAllClientsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Project>> RetrieveAllProjectsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Project>> RetrieveProjectsByCity(string city)
    {
        return Task.FromResult(ProjectsByCity);
    }
}