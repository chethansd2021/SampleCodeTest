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
    public class UpdateItemCommandHandler
    {
        private readonly ItemRepository _repository;

        public UpdateItemCommandHandler(ItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<Item> Handle(UpdateItemCommand command)
        {
            var existingItem = await _repository.GetByIdAsync(command.Id);
            if (existingItem == null) return null;

            existingItem.Name = command.Name ?? existingItem.Name;
            existingItem.Status = command.Status ?? existingItem.Status;

            await _repository.UpdateAsync(existingItem);
            return existingItem;
        }
    }
}
