using System;

namespace API.Helpers.Pagination;

public class PaginatedResult<T>
{
    public PaginationMetadata Metadata { get; set; } = default!;
    public List<T> Items { get; set; } = [];
}


public class PaginationMetadata
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}

public class PaginationHelper
{
    public static  PaginatedResult<T> Create<T>(List<T> items, int count, int pageNumber, int pageSize)
    {
        return new PaginatedResult<T>
        {
            Metadata = new PaginationMetadata
            {
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double) pageSize),
                PageSize = pageSize,
                TotalCount = count
            },
            Items = items
        };
    }
}
