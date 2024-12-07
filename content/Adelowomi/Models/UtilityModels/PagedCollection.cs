using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Adelowomi.Models.UtilityModels;

/// <summary>
/// Represents a paginated collection following OpenAPI specification
/// </summary>
public class PagedCollection<T>
{
    [JsonPropertyName("items")]
    public IReadOnlyList<T> Items { get; }

    [JsonPropertyName("metadata")]
    public PaginationMetadata Metadata { get; }

    [JsonPropertyName("links")]
    public PaginationLinks Links { get; }

    public PagedCollection(
        IReadOnlyList<T> items,
        int totalCount,
        int currentPage,
        int pageSize,
        IUrlHelper urlHelper,
        string routeName,
        object? routeValues = null)
    {
        Items = items;
        Metadata = new PaginationMetadata(totalCount, currentPage, pageSize);
        Links = new PaginationLinks(Metadata, urlHelper, routeName, routeValues);
    }
}
