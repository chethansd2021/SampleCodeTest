using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.Application.Queries.Item
{
    public class GetItemByIdQuery
    {
        public string Id { get; }

        public GetItemByIdQuery(string id)
        {
            Id = id;
        }
    }
}
