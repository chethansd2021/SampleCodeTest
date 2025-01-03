using Microsoft.AspNetCore.Mvc;
using SampleCodeTest.Application.Commands.Item;
using SampleCodeTest.Application.Handlers.CommandHandlers;
using SampleCodeTest.Application.Handlers.QueryHandlers;
using SampleCodeTest.Application.Queries.Item;

namespace SampleCodeTest.API.Controllers
{
    [Route("api/Items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ItemCommandHandler _commandHandler;
        private readonly GetItemByIdQueryHandler _getItemByIdQueryHandler;
        private readonly GetAllItemsQueryHandler _getAllItemsQueryHandler;

        public ItemController(
         ItemCommandHandler commandHandler,
         GetItemByIdQueryHandler getItemByIdQueryHandler,
         GetAllItemsQueryHandler getAllItemsQueryHandler)
        {
            _commandHandler = commandHandler;
            _getItemByIdQueryHandler = getItemByIdQueryHandler;
            _getAllItemsQueryHandler = getAllItemsQueryHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemCommand command)
        {
            var createdItem = await _commandHandler.HandleCreateAsync(command);
            return Ok(new { id = createdItem.Id, item = createdItem });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] UpdateItemCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Item ID mismatch.");
            }

            var updatedItem = await _commandHandler.HandleUpdateAsync(command);
            return Ok(updatedItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            var command = new DeleteItemCommand(id);
            var success = await _commandHandler.HandleDeleteAsync(command);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(string id)
        {
            var query = new GetItemByIdQuery(id);
            var item = await _getItemByIdQueryHandler.Handle(query);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems([FromQuery] string name, [FromQuery] string status, [FromQuery] int? page, [FromQuery] int? pageSize)
        {
            var query = new GetAllItemsQuery(name, status, page, pageSize);
            var items = await _getAllItemsQueryHandler.Handle(query);

            if (items == null || !items.Any())
            {
                return NoContent();
            }

            return Ok(items);
        }
    }
}