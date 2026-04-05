using System;

namespace API.DTOs.PsgcApi;

public class PsgcApiResponse<T>
{
    public List<T> Data { get; set; } = new();
}
