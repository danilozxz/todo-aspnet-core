using Microsoft.AspNetCore.Mvc;
using Todo.DTOs;
using Todo.Services;

namespace Todo.Controllers;

public static class TodoController
{
    public static void MapTodoEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api/v1");

        api.MapGet("/todos", async (
            int page = 1,
            int pageSize = 10,
            bool? isCompleted = null,
            string? sortBy = "id",
            ITodoService service = default!) =>
        {
            pageSize = Math.Clamp(pageSize, 1, 100);
            var (todos, totalCount) = await service.GetPageAsync(page, pageSize, isCompleted, sortBy);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return Results.Ok(new PagedResponse<TodoResponse>(
                todos.Select(t => t.ToResponse()),
                new PaginationMeta(page, pageSize, totalPages, totalCount, page < totalPages, page > 1)
            ));
        }
        );

        api.MapGet("/todos/{id:int}", async (int id, ITodoService service) =>
        {
            var todo = await service.GetByIdAsync(id);

            return todo is null
                ? Results.Problem(
                    title: "Todo Not Found",
                    detail: $"Todo with ID {id} was not found.",
                    statusCode: StatusCodes.Status404NotFound
                )
                : Results.Ok(todo.ToResponse());
        });

        api.MapPost("/todos", async (CreateTodoRequest request, ITodoService service) =>
        {
            var errors = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(request.Title)) errors["Title"] = ["Title is required."];


            if (errors.Count > 0)
            {
                return Results.ValidationProblem(errors);
            }

            var todo = await service.CreateAsync(request);

            return Results.Created($"/api/v1/todos/{todo.Id}", todo.ToResponse());
        });

        api.MapPut("/todos/{id:int}", async (int id, UpdateTodoRequest request, ITodoService service) =>
        {
            var todo = await service.UpdateAsync(id, request);

            return todo is null
                ? Results.Problem(
                    title: "Todo Not Found",
                    detail: $"Todo with ID {id} was not found.",
                    statusCode: StatusCodes.Status404NotFound
                )
                : Results.NoContent();
        });

        api.MapDelete("/todos/{id:int}", async (int id, ITodoService service) =>
        {
            var deleted = await service.DeleteAsync(id);

            return deleted
                ? Results.NoContent()
                : Results.Problem(
                    title: "Todo Not Found",
                    detail: $"Todo with ID {id} was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        });
    }
}