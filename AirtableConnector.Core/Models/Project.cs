using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirtableConnector.Core.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string City { get; set; }
        public int SiteArea { get; set; }
        // CostSqFoot could be a readonly computed property instead of field stored somewhere
        public decimal CostSqFoot { get; set; }
        public int GrossFloorArea { get; set; }
        public decimal TotalCost { get; set; }
        public string[] ClientKeys {get; set; }
    }
}