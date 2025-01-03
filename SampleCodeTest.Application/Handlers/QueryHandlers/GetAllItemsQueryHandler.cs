using SampleCodeTest.Application.Queries.Item;
using SampleCodeTest.Domain.Entities;
using SampleCodeTest.Domain.Interface;
using SampleCodeTest.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.Application.Handlers.QueryHandlers
{
    public class GetAllItemsQueryHandler
    {
        private readonly IItemRepository _repository;

        public GetAllItemsQueryHandler(IItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Item>> Handle(GetAllItemsQuery query)
        {
            return await _repository.GetAllAsync();
        }
    }
}
