using System.Text.Json.Serialization;

namespace Fermion.EntityFramework.Shared.DTOs.Sorting;

/// <summary>
/// Represents a sort request containing field and order information
/// </summary>
public class SortRequestDto
{
    /// <summary>
    /// Gets or sets the sort order type
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SortOrderTypes Order { get; set; }
    
    /// <summary>
    /// Gets or sets the field name to sort by
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// Initializes a new instance of the SortRequestDto class with default values
    /// </summary>
    public SortRequestDto() : this(string.Empty, SortOrderTypes.Desc)
    {
    }

    /// <summary>
    /// Initializes a new instance of the SortRequestDto class with specified field and order
    /// </summary>
    /// <param name="field">The field name to sort by</param>
    /// <param name="dir">The sort order type</param>
    public SortRequestDto(string field, SortOrderTypes dir)
    {
        Field = field;
        Order = dir;
    }
}