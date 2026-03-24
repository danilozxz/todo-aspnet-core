namespace Todo.DTOs

{
    public record TodoResponse(
            int Id,
            string Title,
            bool IsCompleted,
            DateTime CreatedAt,
            DateTime? CompletedAt
    );
}