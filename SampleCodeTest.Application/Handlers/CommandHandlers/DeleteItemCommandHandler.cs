using SampleCodeTest.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.Application.Handlers.CommandHandlers
{
    public class DeleteItemCommandHandler
    {
        private readonly ItemRepository _repository;

        public DeleteItemCommandHandler(ItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(string id)
        {
            var existingItem = await _repository.GetByIdAsync(id);
            if (existingItem == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
