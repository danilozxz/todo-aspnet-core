namespace Todo.Services;

using Todo.DTOs;
using Todo.Models;
public class TodoService : ITodoService
{

    private static readonly List<Todo> _todos =
    [
        new Todo { Id = 1, Title = "Buy groceries", IsCompleted = false, CreatedAt = DateTime.UtcNow},
        new Todo { Id = 2, Title = "Walk the dog", IsCompleted = true, CreatedAt = DateTime.UtcNow, CompletedAt = DateTime.UtcNow},
        new Todo { Id = 3, Title = "Read a book", IsCompleted = false, CreatedAt = DateTime.UtcNow},
        new Todo { Id = 4, Title = "Write some code", IsCompleted = true, CreatedAt = DateTime.UtcNow, CompletedAt = DateTime.UtcNow},
        new Todo { Id = 5, Title = "Go for a run", IsCompleted = false, CreatedAt = DateTime.UtcNow},
    ];

    private static int _nextId = 11;
    private static readonly Lock _lock = new();

    // Lista todos os itens, mas não faz paginação. (Não utilizado no controller)
    public Task<IEnumerable<Todo>> GetAllAsync() => Task.FromResult<IEnumerable<Todo>>(_todos);

    public Task<(IEnumerable<Todo> Items, int TotalCount)> GetPageAsync(int page, int pageSize, bool? isCompleted = null, string? sortBy = "id", bool descending = false)
    {
        var query = _todos.AsEnumerable();

        if(isCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        }

        query = sortBy?.ToLowerInvariant() switch
        {
            "title" => descending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
            "createdat" => descending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt),
            "completedat" => descending ? query.OrderByDescending(t => t.CompletedAt) : query.OrderBy(t => t.CompletedAt),
            _ => descending ? query.OrderByDescending(t => t.Id) : query.OrderBy(t => t.Id)
        };

        var totalCount = query.Count();
        var items = query.Skip((page - 1) * pageSize).Take(pageSize);

        return Task.FromResult((items, totalCount));
    }

    public Task<Todo?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
    
    public Task<Todo> CreateAsync(CreateTodoRequest request)
    {
        lock (_lock)
        {
            var todo = new Todo
            {
                Id = _nextId++,
                Title = request.Title
            };

            _todos.Add(todo);
            return Task.FromResult(todo);
        }
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }



    public Task<Todo?> UpdateAsync(int id, string? title = null, bool? isCompleted = null)
    {
        throw new NotImplementedException();
    }

}