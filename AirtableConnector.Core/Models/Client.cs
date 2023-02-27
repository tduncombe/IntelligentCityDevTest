using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirtableConnector.Core.Models
{
    public class Client
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string[] ProjectKeys { get; set; }
    }
}