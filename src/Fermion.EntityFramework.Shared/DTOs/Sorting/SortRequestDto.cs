using System.Text.Json.Serialization;

namespace Fermion.EntityFramework.Shared.DTOs.Sorting;

public class SortRequestDto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SortOrderTypes Order { get; set; }
    public string Field { get; set; }

    public SortRequestDto() : this(string.Empty, SortOrderTypes.Desc)
    {
    }

    public SortRequestDto(string field, SortOrderTypes dir)
    {
        Field = field;
        Order = dir;
    }
}