using Amazon.DynamoDBv2;
using SampleCodeTest.Application.Handlers.CommandHandlers;
using SampleCodeTest.Application.Handlers.QueryHandlers;
using SampleCodeTest.Domain.Interface;
using SampleCodeTest.Infrastructure.Repositories;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Fetch AWS configuration from appsettings.json
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

// Register controllers
builder.Services.AddControllers();

// Register DynamoDB client
builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    // You can use the AWSOptions from configuration (e.g., appsettings.json or environment variables)
    var awsOptions = builder.Configuration.GetAWSOptions();

    // You can specify a custom endpoint for DynamoDB for local development
    var clientConfig = new AmazonDynamoDBConfig
    {
        RegionEndpoint = Amazon.RegionEndpoint.USEast1 // Change this to your desired region
    };

    // If you need to use a local DynamoDB instance (e.g., for local development):
    var serviceUrl = builder.Configuration["AWS:DynamoDB:ServiceURL"]; // Set this in appsettings.json or environment variable
    if (!string.IsNullOrEmpty(serviceUrl))
    {
        clientConfig.ServiceURL = serviceUrl;  // Custom endpoint (e.g., "http://localhost:8000" for local DynamoDB)
    }

    return new AmazonDynamoDBClient(awsOptions.Credentials, clientConfig);
});

// Register DynamoDB Context (for interacting with DynamoDB using a higher-level abstraction)
builder.Services.AddSingleton<DynamoDBContext>(sp =>
{
    var dynamoDBClient = sp.GetRequiredService<IAmazonDynamoDB>();
    return new DynamoDBContext(dynamoDBClient);
});

// Register application and infrastructure services
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddTransient<ItemCommandHandler>();
builder.Services.AddTransient<GetItemByIdQueryHandler>();
builder.Services.AddTransient<GetAllItemsQueryHandler>();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
