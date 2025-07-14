namespace Fermion.EntityFramework.Shared.DTOs.Pagination;

/// <summary>
/// Represents a pageable response containing data and metadata
/// </summary>
/// <typeparam name="T">The type of items contained in the response</typeparam>
public class PageableResponseDto<T>
{
    /// <summary>
    /// Gets or sets the list of data items for the current page
    /// </summary>
    public List<T> Data { get; set; }
    
    /// <summary>
    /// Gets or sets the metadata information about the pagination
    /// </summary>
    public PageableResponseMetaDto Meta { get; set; }

    /// <summary>
    /// Initializes a new instance of the PageableResponseDto class
    /// </summary>
    /// <param name="data">The list of data items for the current page</param>
    /// <param name="meta">The metadata information about the pagination</param>
    public PageableResponseDto(List<T> data, PageableResponseMetaDto meta)
    {
        Meta = meta;
        Data = data;
    }
}