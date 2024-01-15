
namespace Moments.Api.Plugin;

public interface IPlugin
{
    string Name { get; set; }
    string PackageName { get; set; }
    string Version { get; set; }
    string Author { get; set; }
    void Install();
    void UnInstall();

    List<RenderNode> RenderList { get; set; }
}