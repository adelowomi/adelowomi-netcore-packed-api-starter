using System;
using System.Text.Json.Serialization;

namespace Adelowomi.Models.UtilityModels;

/// <summary>
/// Metadata about the pagination state
/// </summary>
public class PaginationMetadata
{
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; }

    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; }

    [JsonPropertyName("hasNext")]
    public bool HasNext { get; }

    [JsonPropertyName("hasPrevious")]
    public bool HasPrevious { get; }

    public PaginationMetadata(int totalCount, int currentPage, int pageSize)
    {
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        HasNext = CurrentPage < TotalPages;
        HasPrevious = CurrentPage > 1;
    }
}