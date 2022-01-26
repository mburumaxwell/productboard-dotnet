using productboard.Core;

namespace productboard.Models;

/// <summary>Options for listing features.</summary>
public record FeaturesListOptions : BasicListOptions
{
    /// <summary>
    /// If specified, the resource returns only features in status with given ID.
    /// If both <see cref="StatusId"/> and <see cref="StatusName"/> are specified,
    /// only features that fulfill both criteria are returned.
    /// </summary>
    public string? StatusId { get; set; }

    /// <summary>
    /// If specified, the resource returns only features in status with given name.
    /// If both <see cref="StatusId"/> and <see cref="StatusName"/> are specified,
    /// only features that fulfill both criteria are returned.
    /// </summary>
    public string? StatusName { get; set; }

    /// <summary>
    /// If specified, the resource returns only features with <c>archived</c> flag matching provided value.
    /// </summary>
    public bool? Archived { get; set; }

    internal override void Populate(QueryValues values)
    {
        base.Populate(values);
        values.Add("status.id", StatusId)
              .Add("status.name", StatusName)
              .Add("archived", Archived);

    }
}