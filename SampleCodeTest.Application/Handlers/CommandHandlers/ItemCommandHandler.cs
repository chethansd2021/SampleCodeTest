using SampleCodeTest.Application.Commands.Item;
using SampleCodeTest.Domain.Entities;
using SampleCodeTest.Domain.Interface;
using SampleCodeTest.Infrastructure.Repositories;

namespace SampleCodeTest.Application.Handlers.CommandHandlers
{

    public class ItemCommandHandler
    {
        private readonly IItemRepository _repository;

        public ItemCommandHandler(IItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<Item> HandleCreateAsync(CreateItemCommand command)
        {
            var item = new Item
            {
                Id = command.Id,
                Name = command.Name,
                Status = command.Status,
                CreatedAt = DateTime.UtcNow
            };

            return await _repository.CreateAsync(item);
        }

        public async Task<Item> HandleUpdateAsync(UpdateItemCommand command)
        {
            var item = new Item
            {
                Id = command.Id,
                Name = command.Name,
                Status = command.Status,
                CreatedAt = command.CreatedAt // Use the existing CreatedAt from the command
            };

            return await _repository.UpdateAsync(item);
        }

        public async Task<bool> HandleDeleteAsync(DeleteItemCommand command)
        {
            var item = await _repository.GetByIdAsync(command.Id);
            if (item == null)
            {
                return false;
            }
            return await _repository.DeleteAsync(command.Id);
        }
    }
}
