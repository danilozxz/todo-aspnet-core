namespace Todo.Services;

using Todo.Models;
using Todo.DTOs;

public interface ITodoService
{
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<(IEnumerable<Todo> Items, int TotalCount)> GetPageAsync(
        int page, int pageSize, bool? isCompleted = null, string? sortBy = "id", bool descending = false);

    Task<Todo?> GetByIdAsync(int id);
    Task<Todo> CreateAsync(CreateTodoRequest request);
    Task<Todo?> UpdateAsync(int id, string? title = null, bool? isCompleted = null);
    Task<bool> DeleteAsync(int id);
}
