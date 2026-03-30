using System.Reflection.Metadata.Ecma335;
using Todo.Controllers;
using Todo.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ITodoService, TodoService>();

var app = builder.Build();

app.MapOpenApi(); 
app.MapScalarApiReference();

app.MapTodoEndpoints();
app.UseHttpsRedirection();

app.Run();



