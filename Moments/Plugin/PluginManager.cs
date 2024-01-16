using System.Reflection;
using System.Runtime.Loader;
using System.IO;
using Moments.Api.Model;
using Moments.Api.Plugin;

namespace Moments.Plugin;

public class PluginManager(IWebHostEnvironment webHostEnvironment, IFreeSql db)
{
    private readonly string _folderPath = Path.Combine(webHostEnvironment.ContentRootPath, "Plugin", "Package");

    public readonly List<IPlugin> InstallPlugins = [];
    public readonly List<IPlugin> UnInstallPlugins = [];
    public readonly List<RenderNode> RenderList = [];

    public void LoadPlugins()
    {
        var files = Directory.GetFiles(_folderPath, "*.dll");
        foreach (var file in files)
        {
            LoadPlugin(file);
        }
    }

    public void LoadPlugin(string filePath)
    {
        var packageName = Common.String.GetPluginPackageName(filePath);
        var pluginSwitch = db.Select<PluginSwitch>().Where(x => x.PackageName == packageName).First();
        bool isInstall = !(pluginSwitch is null || pluginSwitch.Install == false);
        try
        {
            Assembly assembly;
            using (var sr = new StreamReader(filePath))
            {
                assembly = AssemblyLoadContext.Default.LoadFromStream(sr.BaseStream);
            }
            Type pluginType = assembly.GetType(packageName + ".Plugin") ?? throw new InvalidOperationException();
            var plugin = (IPlugin)Activator.CreateInstance(pluginType)!;
            if (isInstall)
            {
                InstallPlugins.Add(plugin);
                foreach (var item in plugin.RenderList)
                {
                    item.PackageName = packageName;
                    RenderList.Add(item);
                }
            }
            else
            {
                UnInstallPlugins.Add(plugin);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{Common.String.GetPluginPackageName(filePath)} 加载失败: {ex.Message}");
        }
    }


    public void InstallPlugin(string packageName)
    {
        var item = db.Select<PluginSwitch>().Where(x => x.PackageName == packageName).First();
        if (item is null)
        {
            db.Insert<PluginSwitch>().AppendData(new PluginSwitch
            {
                PackageName = packageName,
                Install = true
            }).ExecuteAffrows();
        }
        else
        {
            db.Update<PluginSwitch>()
                .Where(x => x.PackageName == packageName)
                .Set(x => x.Install == true)
                .ExecuteAffrows();
        }

        var plugin = UnInstallPlugins.First(x => x.PackageName == packageName);
        UnInstallPlugins.Remove(plugin);
        InstallPlugins.Add(plugin);
        plugin.Install();
        foreach (var render in plugin.RenderList)
        {
            render.PackageName = packageName;
            RenderList.Add(render);
        }
    }

    public void UnInstallPlugin(string packageName)
    {
        db.Update<PluginSwitch>()
            .Where(x => x.PackageName == packageName)
            .Set(x => x.Install == false)
            .ExecuteAffrows();
        var plugin = InstallPlugins.First(x => x.PackageName == packageName);
        RenderList.RemoveAll(x => x.PackageName == packageName);
        InstallPlugins.Remove(plugin);
        UnInstallPlugins.Add(plugin);
        plugin.UnInstall();
    }

    public void DeletePlugin(string packageName)
    {
        db.Delete<PluginSwitch>().Where(x => x.PackageName == packageName).ExecuteAffrows();
        var path = _folderPath + $"\\{packageName}.dll";
        UnInstallPlugins.RemoveAll(x => x.PackageName == packageName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
