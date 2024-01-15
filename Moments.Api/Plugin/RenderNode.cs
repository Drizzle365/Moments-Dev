using Microsoft.AspNetCore.Components;

namespace Moments.Api.Plugin;

public class RenderNode
{
    public Hook Hook { get; set; }
    public RenderFragment? RenderFragment { get; set; }
    public string? PackageName { get; set; }
}