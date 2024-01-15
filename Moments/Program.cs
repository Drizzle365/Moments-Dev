using Masa.Blazor;
using Masa.Blazor.Presets;
using Moments.Api.Model;
using Moments.Api.Service;
using Moments.Components;
using Moments.Plugin;
using Moments.Service;
var dbPath =(Environment.CurrentDirectory +"/moments.db");
if (!File.Exists(dbPath))
{
    File.Create(dbPath).Close();
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMasaBlazor(options =>
{
    options.Defaults = new Dictionary<string, IDictionary<string, object?>?>()
    { 
        {
            PopupComponents.SNACKBAR, new Dictionary<string, object?>()
            {
                { nameof(PEnqueuedSnackbars.Position), SnackPosition.BottomCenter }
            }
        }
    };
});

builder.Services.AddSingleton(SqlFactory);
builder.Services.AddSingleton<ISubscriptionService, SubscriptionService>();
builder.Services.AddSingleton<IReadService, ReadService>();
builder.Services.AddSingleton<PluginManager>();

var app = builder.Build();
using (IServiceScope serviceScope = app.Services.CreateScope())
{
    var sql = serviceScope.ServiceProvider.GetRequiredService<IFreeSql>();
    var plugin = serviceScope.ServiceProvider.GetRequiredService<PluginManager>();
    plugin.LoadPlugins();
    sql.CodeFirst.SyncStructure(typeof(Subscription));
    sql.CodeFirst.SyncStructure(typeof(Read));
    sql.CodeFirst.SyncStructure(typeof(PluginSwitch));
    sql.CodeFirst.SyncStructure(typeof(Category));
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();
app.Run();

IFreeSql SqlFactory(IServiceProvider _)
{
    IFreeSql sql = new FreeSql.FreeSqlBuilder().UseConnectionString(FreeSql.DataType.Sqlite, @"Data Source=moments.db")
        .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}")) //监听SQL语句
        .UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
        .Build();
    return sql;
}