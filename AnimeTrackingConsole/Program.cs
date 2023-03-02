// See https://aka.ms/new-console-template for more information

using AnimeTrackingApi;
using AnimeTrackingApi.Dto;
using AnimeTrackingConsole;
using Quartz;
using Quartz.Impl;

class Program
{
    static async Task Main(string[] args)
    {
        var animeTracking = new AnimeTracking();
        var result = animeTracking.GetTitleSchedule("Titan").Result;
        var dates = result?.airingSchedule.edges.Select(x => x.node.getAiringAtUtc());
        
        foreach (var date in dates)
        {
            var factory = new StdSchedulerFactory();
            var scheduler = await factory.GetScheduler();
            await scheduler.Start();
            var job = JobBuilder.Create<TestJob>().Build();
            var trigger = TriggerBuilder.Create().StartAt(date).Build();
            await scheduler.ScheduleJob(job, trigger);
        }
    }
}