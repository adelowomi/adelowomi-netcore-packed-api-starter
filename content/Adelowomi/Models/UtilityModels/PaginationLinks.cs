using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Adelowomi.Models.UtilityModels;

public class PaginationLinks
{
    [JsonPropertyName("self")]
    public string? Self { get; }

    [JsonPropertyName("first")]
    public string? First { get; }

    [JsonPropertyName("previous")]
    public string? Previous { get; }

    [JsonPropertyName("next")]
    public string? Next { get; }

    [JsonPropertyName("last")]
    public string? Last { get; }

    public PaginationLinks(
        PaginationMetadata metadata, 
        IUrlHelper urlHelper,
        string routeName,
        object? routeValues = null)
    {
        // Create base route values
        var baseRouteValues = routeValues == null 
            ? new RouteValueDictionary() 
            : new RouteValueDictionary(routeValues);

        // Self link
        baseRouteValues["pageNumber"] = metadata.CurrentPage;
        baseRouteValues["pageSize"] = metadata.PageSize;
        Self = urlHelper.Link(routeName, baseRouteValues);

        // First page link
        baseRouteValues["pageNumber"] = 1;
        First = urlHelper.Link(routeName, baseRouteValues);

        // Last page link
        baseRouteValues["pageNumber"] = metadata.TotalPages;
        Last = urlHelper.Link(routeName, baseRouteValues);

        // Previous page link
        if (metadata.HasPrevious)
        {
            baseRouteValues["pageNumber"] = metadata.CurrentPage - 1;
            Previous = urlHelper.Link(routeName, baseRouteValues);
        }

        // Next page link
        if (metadata.HasNext)
        {
            baseRouteValues["pageNumber"] = metadata.CurrentPage + 1;
            Next = urlHelper.Link(routeName, baseRouteValues);
        }
    }
}