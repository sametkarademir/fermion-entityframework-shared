namespace Fermion.EntityFramework.Shared.DTOs.Pagination;

public class PageableResourceDto<T>
{
    public List<T> Content { get; set; }
    public int TotalCount { get; set; }
    public PageableResponseMetaDto Meta { get; set; }

    public PageableResourceDto(List<T> data, int totalCount, int page, int pageSize)
    {
        Content = data;
        TotalCount = totalCount;
        Meta = new PageableResponseMetaDto(totalCount, page, pageSize);
    }
}