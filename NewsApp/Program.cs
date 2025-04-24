using Microsoft.AspNetCore.Diagnostics;
using NewsApp.Database;
using NewsApp.Exceptions;
using NewsApp.Middleware;
using NewsApp.Models.DB;
using NewsApp.Repositories;
using NewsApp.Repositories.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<NewsDbContext>();

builder.Services.AddScoped<IAuthorsRepository, AuthorsRepository>();
builder.Services.AddScoped<IArticlesRepository, ArticlesRepository>();

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<NewsDbContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOutputCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        context.Response.ContentType = "application/json";

        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception is null)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("{\"error\": \"Unknown error occurred.\"}");
            return;
        }

        logger.LogError(exception, "Unhandled exception occurred while processing request.");

        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            BadRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var errorResponse = new
        {
            status = statusCode,
            error = exception.Message,
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});

// TODO: add used endpoints manually to avoid exposing unused ones
app.MapIdentityApi<User>();

app.UseHttpsRedirection();

app.UseOutputCache();

app.UseAuthorization();

app.MapControllers();

app.Run();
