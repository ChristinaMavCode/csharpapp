using CSharpApp.Api;
using CSharpApp.Core.Commands;
using CSharpApp.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<PerformanceLoggingMiddleware>();

var versionedEndpointRouteBuilder = app.NewVersionedApi();

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts",
    async ([FromServices] IMediator mediator) =>
    {
        var result = await mediator.Send(new GetAllProductsQuery());
        return result is not null ? Results.Ok(result) : Results.NotFound();
    })
    .WithName("GetProducts").HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproduct/{id:int}",
    async ([FromRoute] int id, [FromServices] IMediator mediator) =>
    {
        var result = await mediator.Send(new GetProductByIdQuery(id));
        return result is not null ? Results.Ok(result) : Results.NotFound();
    })
    .WithName("GetProduct").HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createproduct",
    async ([FromBody] CreateProductCommand command, [FromServices] IMediator mediator) =>
    {
        var product = await mediator.Send(command);
        return Results.Created($"/api/v1/product/{product.Id}", product);
    })
    .WithName("CreateProduct").HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories",
    async ([FromServices] IMediator mediator) =>
    {
        var result = await mediator.Send(new GetAllCategoriesQuery());
        return result is not null ? Results.Ok(result) : Results.NotFound();
    })
    .WithName("GetCategories").HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategory/{id:int}",
    async ([FromRoute] int id, [FromServices] IMediator mediator) =>
    {
        var result = await mediator.Send(new GetCategoryByIdQuery(id));
        return result is not null ? Results.Ok(result) : Results.NotFound();
    })
    .WithName("GetCategory").HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createcategory",
    async ([FromBody] CreateCategoryCommand command, [FromServices] IMediator mediator) =>
    {
        var category = await mediator.Send(command);
        return Results.Created($"/api/v1/categories/{category.Id}", category);
    })
    .WithName("CreateCategory").HasApiVersion(1.0);

app.Run();