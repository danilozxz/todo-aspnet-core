namespace Todo.Services;

using Todo.Models;


public interface ITodoService
{
    Task<IEnumerable<Models.Todo>> GetAllAsync();
    Task<(IEnumerable<Todo> Items, int TotalCount)> GetPageAsync(
        int page, int pageSize, bool? isCompleted = null, string? sortBy = "id", bool descending = false);

    Task<Todo?> GetByIdAsync(int id);
    Task<Todo> CreateAsync(string title);
    Task<Todo?> UpdateAsync(int id, string? title = null, bool? isCompleted = null);
    Task<bool> DeleteAsync(int id);
}
