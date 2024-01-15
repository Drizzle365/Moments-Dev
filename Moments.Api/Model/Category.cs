using FreeSql.DataAnnotations;

namespace Moments.Api.Model;

public class Category
{
    [Column(IsPrimary = true)] public required string Name { get; set; }
    public string? Parent { get; set; }
}