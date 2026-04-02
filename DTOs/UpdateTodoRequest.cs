namespace Todo.DTOs;

public record UpdateTodoRequest
(
    string? Title,
    bool IsCompleted
);