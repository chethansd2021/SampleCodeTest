using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.Application.Commands.Item
{
    public class CreateItemCommand
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public CreateItemCommand(string id, string name, string status)
        {
            Id = id;
            Name = name;
            Status = status;
        }
    }
}
