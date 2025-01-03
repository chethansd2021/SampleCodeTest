using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using SampleCodeTest.Domain.Entities;
using SampleCodeTest.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.Infrastructure.Repositories
{

    public class ItemRepository : IItemRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private const string TableName = "ItemsTable"; // DynamoDB Table Name

        public ItemRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        // Create a new Item
        public async Task<Item> CreateAsync(Item Item)
        {
            var putRequest = new PutItemRequest
            {
                TableName = TableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = Item.Id } },
                    { "Name", new AttributeValue { S = Item.Name } },
                    { "Status", new AttributeValue { S = Item.Status } },
                    { "CreatedAt", new AttributeValue { S = Item.CreatedAt.ToString("o") } }
                }
            };

            await _dynamoDb.PutItemAsync(putRequest);
            return Item; // Return the created Item with its ID
        }

        // Get all Items with optional filters and pagination
        public async Task<IEnumerable<Item>> GetAllAsync(string name = null, string status = null, int? page = null, int? pageSize = null)
        {
            var scanRequest = new ScanRequest
            {
                TableName = TableName,
                FilterExpression = BuildFilterExpression(name, status),
                ExpressionAttributeValues = BuildExpressionAttributeValues(name, status)
            };

            if (page.HasValue && pageSize.HasValue)
            {
                scanRequest.ExclusiveStartKey = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = page.ToString() } }
                };
                scanRequest.Limit = pageSize.Value;
            }

            var response = await _dynamoDb.ScanAsync(scanRequest);
            var Items = response.Items.Select(i => new Item
            {
                Id = i["Id"].S,
                Name = i["Name"].S,
                Status = i["Status"].S,
                CreatedAt = DateTime.Parse(i["CreatedAt"].S)
            });

            return Items;
        }

        // Get an Item by ID
        public async Task<Item> GetByIdAsync(string id)
        {
            var getRequest = new GetItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = id } }
                }
            };

            var response = await _dynamoDb.GetItemAsync(getRequest);
            if (response.Item == null || !response.Item.Any())
            {
                return null; // Item not found
            }

            return new Item
            {
                Id = response.Item["Id"].S,
                Name = response.Item["Name"].S,
                Status = response.Item["Status"].S,
                CreatedAt = DateTime.Parse(response.Item["CreatedAt"].S)
            };
        }

        // Update an existing Item
        public async Task<Item> UpdateAsync(Item Item)
        {
            var updateRequest = new UpdateItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = Item.Id } }
                },
                UpdateExpression = "SET #name = :name, #status = :status",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    { "#name", "Name" },
                    { "#status", "Status" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":name", new AttributeValue { S = Item.Name } },
                    { ":status", new AttributeValue { S = Item.Status } }
                },
                ReturnValues = "ALL_NEW" // Return the updated Item
            };

            var response = await _dynamoDb.UpdateItemAsync(updateRequest);
            var updatedItem = new Item
            {
                Id = Item.Id,
                Name = response.Attributes["Name"].S,
                Status = response.Attributes["Status"].S,
                CreatedAt = DateTime.Parse(response.Attributes["CreatedAt"].S)
            };

            return updatedItem;
        }

        // Delete an Item by ID
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteRequest = new DeleteItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = id } }
                }
            };

            var response = await _dynamoDb.DeleteItemAsync(deleteRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        // Helper methods for filtering and pagination
        private string BuildFilterExpression(string name, string status)
        {
            var filters = new List<string>();

            if (!string.IsNullOrEmpty(name))
            {
                filters.Add("contains(#name, :name)");
            }

            if (!string.IsNullOrEmpty(status))
            {
                filters.Add("contains(#status, :status)");
            }

            return string.Join(" and ", filters);
        }

        private Dictionary<string, AttributeValue> BuildExpressionAttributeValues(string name, string status)
        {
            var values = new Dictionary<string, AttributeValue>();

            if (!string.IsNullOrEmpty(name))
            {
                values.Add(":name", new AttributeValue { S = name });
            }

            if (!string.IsNullOrEmpty(status))
            {
                values.Add(":status", new AttributeValue { S = status });
            }

            return values;
        }
    }
}
