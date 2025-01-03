using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.Extensions.DependencyInjection;
using SampleCodeTest.Application.Handlers.CommandHandlers;
using SampleCodeTest.Application.Handlers.QueryHandlers;
using SampleCodeTest.Infrastructure;
using SampleCodeTest.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTest.LambdaFunctions
{
    public class Startup
    {
        public IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // AWS SDK Configuration
            services.AddDefaultAWSOptions(new AWSOptions());

            // Register AWS DynamoDB Client
            services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();

            // Register DynamoDB Context
            services.AddSingleton<DynamoDbContext>();

            // Register Repositories
            services.AddTransient<ItemRepository>();

            // Register Command Handlers
            services.AddTransient<CreateItemCommandHandler>();
            services.AddTransient<UpdateItemCommandHandler>();
            services.AddTransient<DeleteItemCommandHandler>();

            // Register Query Handlers
            services.AddTransient<GetAllItemsQueryHandler>();
            services.AddTransient<GetItemByIdQueryHandler>();

            // Build the Service Provider
            return services.BuildServiceProvider();
        }
    }
}
