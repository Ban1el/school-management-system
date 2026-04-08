using System;
using API.Helpers.Pagination;

namespace API.DTOs.Dropdown;

public class DropdownParamsDto : PaginatedParams
{
    public string? search { get; set; } = string.Empty;
}
