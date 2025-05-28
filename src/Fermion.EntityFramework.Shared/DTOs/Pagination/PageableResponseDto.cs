namespace Fermion.EntityFramework.Shared.DTOs.Pagination;

public class PageableResponseDto<T>
{
    public List<T> Data { get; set; }
    public PageableResponseMetaDto Meta { get; set; }

    public PageableResponseDto(List<T> data, PageableResponseMetaDto meta)
    {
        Meta = meta;
        Data = data;
    }
}