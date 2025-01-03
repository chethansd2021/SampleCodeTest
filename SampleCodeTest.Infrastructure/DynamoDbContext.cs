using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.Infrastructure
{
    public class DynamoDbContext
    {
        public DynamoDBContext Context { get; }

        public DynamoDbContext(IAmazonDynamoDB client)
        {
            Context = new DynamoDBContext(client);
        }
    }
}
