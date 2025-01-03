using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.Application.Queries.Item
{
    public class GetAllItemsQuery
    {
        public string Name { get; set; } 
        public string Status { get; set; }
        public int? Page { get; set; } 
        public int? PageSize { get; set; }

        // Constructor to initialize filters and pagination
        public GetAllItemsQuery(string name = null, string status = null, int? page = null, int? pageSize = null)
        {
            Name = name;
            Status = status;
            Page = page;
            PageSize = pageSize;
        }
    }
}
