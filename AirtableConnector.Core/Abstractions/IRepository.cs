using System.Collections.Generic;
using System.Threading.Tasks;
using AirtableConnector.Core.Models;

namespace AirtableConnector.Core.Abstractions
{
    ///<summary>
    /// A basic abstraction to allow us to put a seam between Airtable and our business logic.
    /// It's not a good solution for cases where he have tens, hundreds or thousands of models
    /// in our database. In that case I would write some functions more along the lines of
    /// 
    /// public Task<List<T>> RetrieveAll<T>()
    ///
    /// which would use Airtable's ListRecords<T> and appropriately annotated/named classes
    /// to allow for a single function to retrieve any record type that we like.
    /// I didn't do that for this task largely because dealing with that sort of automated deserializing 
    /// can be fiddly and I didn't think it was worth it to save a minor amount of code repetition.
    ///
    /// Also IRepository is a horribly generic name, sorry.
    ///</summary>
    public interface IRepository
    {
        public Task<List<Client>> RetrieveAllClientsAsync();
        public Task<List<Project>> RetrieveAllProjectsAsync();
        public Task<List<Project>> RetrieveProjectsByCity(string city);
    }
}