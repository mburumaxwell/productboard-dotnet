namespace productboard.Models;

/// <summary>Representation for a model with <c>links</c> node.</summary>
public interface IHasResourceLinks
{
    /// <summary>Links for the resource.</summary>
    ResourceLinks? Links { get; set; }
}
