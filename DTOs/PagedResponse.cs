namespace Todo.DTOs;

public record PagedResponse<T> (
    IEnumerable<T> Data,
    PaginationMeta Pagination
);

public record PaginationMeta(
    int Page,
    int PageSize,
    int TotalPages,
    int TotalCount,
    bool HasNext,
    bool HasPrevious
);