using System.Reflection.Metadata.Ecma335;
using Todo.Controllers;
using Todo.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ITodoService, TodoService>();

// ProblemDetais é um padrão recomendado para respostas de erro em APIs RESTful, pois fornece uma estrutura consistente e fácil de entender para os clientes. Ele inclui informações como status HTTP, título, detalhes e metadados adicionais, o que facilita a depuração e o tratamento de erros do lado do cliente.
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        // Always include useful metadata
        ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
        ctx.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow;
        ctx.ProblemDetails.Instance = $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";
    };
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.MapOpenApi(); 
app.MapScalarApiReference();

app.MapTodoEndpoints();
app.UseHttpsRedirection();

app.UseExceptionHandler();

app.Run();



