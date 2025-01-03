using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.Application.Commands.Item
{
    public class DeleteItemCommand
    {
        public string Id { get; set; }

        public DeleteItemCommand(string id)
        {
            Id = id;
        }
    }
}
