using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SampleCodeTest.Application.Commands.Item;
using SampleCodeTest.Application.Handlers.CommandHandlers;
using SampleCodeTest.Application.Handlers.QueryHandlers;
using SampleCodeTest.Application.Queries.Item;
using SampleCodeTest.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.LambdaFunctions
{
    public class Function
    {
        private readonly IServiceProvider _serviceProvider;

        public Function()
        {
            var startup = new Startup();
            _serviceProvider = startup.ConfigureServices();
        }

        public async Task<APIGatewayProxyResponse> CreateItem(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var handler = _serviceProvider.GetRequiredService<CreateItemCommandHandler>();
                var command = JsonConvert.DeserializeObject<CreateItemCommand>(request.Body);

                if (command == null || string.IsNullOrEmpty(command.Name))
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 400,
                        Body = "Invalid input"
                    };
                }

                var id = await handler.Handle(command);

                return new APIGatewayProxyResponse
                {
                    StatusCode = 201,
                    Body = JsonConvert.SerializeObject(new { Id = id })
                };
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<APIGatewayProxyResponse> GetAllItems(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var handler = _serviceProvider.GetRequiredService<GetAllItemsQueryHandler>();

                // Parse query parameters
                var name = request.QueryStringParameters?.ContainsKey("name") == true
                    ? request.QueryStringParameters["name"]
                    : null;
                var status = request.QueryStringParameters?.ContainsKey("status") == true
                    ? request.QueryStringParameters["status"]
                    : null;
                var page = request.QueryStringParameters?.ContainsKey("page") == true
                    ? int.TryParse(request.QueryStringParameters["page"], out var p) ? p : (int?)null
                    : null;
                var pageSize = request.QueryStringParameters?.ContainsKey("pageSize") == true
                    ? int.TryParse(request.QueryStringParameters["pageSize"], out var ps) ? ps : (int?)null
                    : null;

                // Create the query object
                var query = new GetAllItemsQuery(name, status, page, pageSize);

                // Pass the query to the handler
                var items = await handler.Handle(query);

                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonConvert.SerializeObject(items)
                };
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = $"Error: {ex.Message}"
                };
            }
        }


        public async Task<APIGatewayProxyResponse> GetItemById(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var handler = _serviceProvider.GetRequiredService<GetItemByIdQueryHandler>();

                // Extract the 'id' from the request's path parameters
                if (!request.PathParameters.TryGetValue("id", out var id) || string.IsNullOrEmpty(id))
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 400,
                        Body = "Invalid ID"
                    };
                }

                // Create the query object
                var query = new GetItemByIdQuery(id);

                // Call the handler with the query object
                var item = await handler.Handle(query);

                if (item == null)
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 204,
                        Body = string.Empty
                    };
                }

                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonConvert.SerializeObject(item)
                };
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<APIGatewayProxyResponse> UpdateItem(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var handler = _serviceProvider.GetRequiredService<UpdateItemCommandHandler>();
                var id = request.PathParameters["id"];
                var command = JsonConvert.DeserializeObject<UpdateItemCommand>(request.Body);
                command.Id = id;

                var updatedItem = await handler.Handle(command);

                if (updatedItem == null)
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 404,
                        Body = "Item not found"
                    };
                }

                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonConvert.SerializeObject(updatedItem)
                };
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<APIGatewayProxyResponse> DeleteItem(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var handler = _serviceProvider.GetRequiredService<DeleteItemCommandHandler>();
                var id = request.PathParameters["id"];
                var isDeleted = await handler.Handle(id);

                if (!isDeleted)
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 404,
                        Body = "Item not found"
                    };
                }

                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = "Item deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = $"Error: {ex.Message}"
                };
            }
        }
    }
}
