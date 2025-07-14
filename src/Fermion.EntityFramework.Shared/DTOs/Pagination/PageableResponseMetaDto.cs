namespace Fermion.EntityFramework.Shared.DTOs.Pagination;

/// <summary>
/// Contains metadata information about pagination
/// </summary>
public class PageableResponseMetaDto
{
    /// <summary>
    /// Gets or sets the current page number
    /// </summary>
    public int CurrentPage { get; set; }
    
    /// <summary>
    /// Gets or sets the previous page number, null if current page is the first
    /// </summary>
    public int? PreviousPage { get; set; }
    
    /// <summary>
    /// Gets or sets the next page number, null if current page is the last
    /// </summary>
    public int? NextPage { get; set; }
    
    /// <summary>
    /// Gets or sets the number of items per page
    /// </summary>
    public int PerPage { get; set; }
    
    /// <summary>
    /// Gets or sets the total number of pages
    /// </summary>
    public int TotalPages { get; set; }
    
    /// <summary>
    /// Gets or sets the total number of items
    /// </summary>
    public int TotalCount { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether this is the first page
    /// </summary>
    public bool IsFirstPage { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether this is the last page
    /// </summary>
    public bool IsLastPage { get; set; }

    /// <summary>
    /// Initializes a new instance of the PageableResponseMetaDto class
    /// </summary>
    public PageableResponseMetaDto()
    {
    }

    /// <summary>
    /// Initializes a new instance of the PageableResponseMetaDto class with calculation parameters
    /// </summary>
    /// <param name="totalCount">The total number of items</param>
    /// <param name="page">The current page number</param>
    /// <param name="perPage">The number of items per page</param>
    public PageableResponseMetaDto(int totalCount, int page, int perPage)
    {
        var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling((double)totalCount / perPage);
        var previousPage = page - 1 < 1 ? null : page - 1 > totalPages ? (int?)null : page - 1;
        var nextPage = page + 1 > totalPages ? (int?)null : page + 1;
        var isFirstPage = page == 1;
        var isLastPage = page == totalPages || totalPages == 0;

        CurrentPage = page;
        PreviousPage = previousPage;
        NextPage = nextPage;
        PerPage = perPage;
        TotalPages = totalPages;
        TotalCount = totalCount;
        IsFirstPage = isFirstPage;
        IsLastPage = isLastPage;
    }
}