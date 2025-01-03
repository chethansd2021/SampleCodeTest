using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.Domain.Entities
{
    public class Item
    {
        public string? Id { get; set; }

        public string? Name { get; set; }
       
        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; }
       
    }
}
