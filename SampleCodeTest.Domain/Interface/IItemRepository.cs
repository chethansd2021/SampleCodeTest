using SampleCodeTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.Domain.Interface
{
    public interface IItemRepository
    {
        Task<Item> CreateAsync(Item Item);

        Task<IEnumerable<Item>> GetAllAsync(string name = null, string status = null, int? page = null, int? pageSize = null);

        Task<Item> GetByIdAsync(string id);

        Task<Item> UpdateAsync(Item Item);

        Task<bool> DeleteAsync(string id);
    }
}
