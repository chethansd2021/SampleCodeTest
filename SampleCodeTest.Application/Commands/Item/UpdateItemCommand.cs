using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.Application.Commands.Item
{
    public class UpdateItemCommand
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public UpdateItemCommand(string id, string name = null, string status = null, DateTime createdAt = default)
        {
            Id = id;
            Name = name;
            Status = status;
            CreatedAt = createdAt;
        }
    }
}
