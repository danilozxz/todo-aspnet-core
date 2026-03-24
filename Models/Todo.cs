namespace Todo.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public DTOs.TodoResponse ToResponse() => new (
            Id,
            Title,
            IsCompleted,
            CreatedAt,
            CompletedAt
        );
    }
}