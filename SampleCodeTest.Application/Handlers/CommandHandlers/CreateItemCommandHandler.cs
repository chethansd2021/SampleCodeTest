using SampleCodeTest.Application.Commands.Item;
using SampleCodeTest.Domain.Entities;
using SampleCodeTest.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.Application.Handlers.CommandHandlers
{
    public class CreateItemCommandHandler
    {
        private readonly ItemRepository _repository;

        public CreateItemCommandHandler(ItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(CreateItemCommand command)
        {
            var Item = new Item
            {
                Id = Guid.NewGuid().ToString(),
                Name = command.Name,
                Status = command.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.CreateAsync(Item);
            return Item.Id;
        }
    }
}
