using FreeSql.DataAnnotations;

namespace Moments.Api.Model;

public class PluginSwitch
{
    [Column(IsPrimary = true)] public required string PackageName { get; set; }
    public bool Install { get; set; }
}