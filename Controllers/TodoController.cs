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
    }
}