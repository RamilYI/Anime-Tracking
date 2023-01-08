using System.Collections.Specialized;
using Quartz;
using Quartz.Impl;

namespace AnimeTrackingWeb;

public static class Extensions
{
    public static ControllerActionEndpointConventionBuilder MapBotWebhookRoute<T>(
        this IEndpointRouteBuilder endpoints,
        string route)
    {
        var controllerName = typeof(T).Name.Replace("Controller", "");
        var actionName = typeof(T).GetMethods()[0].Name;

        return endpoints.MapControllerRoute(
            name: "bot_webhook",
            pattern: route,
            defaults: new { controller = controllerName, action = actionName });
    }
    
    public static void UseQuartz(this IApplicationBuilder app)
    {
        app.ApplicationServices.GetService<IScheduler>();
    }

    public static async void AddQuartz(this IServiceCollection services)
    {
        var properties = new NameValueCollection
        {
            // json serialization is the one supported under .NET Core (binary isn't)
            ["quartz.serializer.type"] = "json",

            // the following setup of job store is just for example and it didn't change from v2
            ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
            ["quartz.jobStore.useProperties"] = "false",
            ["quartz.jobStore.dataSource"] = "default",
            ["quartz.jobStore.tablePrefix"] = "QRTZ_",
            ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz",
            ["quartz.dataSource.default.provider"] = "SqlServer-41", // SqlServer-41 is the new provider for .NET Core
            ["quartz.dataSource.default.connectionString"] = @"Server=(localdb)\MSSQLLocalDB;Database=sta-scheduler-quartz;Integrated Security=true"
        };

        var schedulerFactory = new StdSchedulerFactory(properties);
        var scheduler = schedulerFactory.GetScheduler().Result;
        scheduler.Start().Wait();

        services.AddSingleton<IScheduler>(scheduler);
    }
}