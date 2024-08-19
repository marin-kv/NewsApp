using NewsApp.Database;
using NewsApp.Models;
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO: add used endpoints manually to avoid exposing unused ones
app.MapIdentityApi<User>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
