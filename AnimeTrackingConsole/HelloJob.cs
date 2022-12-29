using Quartz;

namespace AnimeTrackingConsole;

public class TestJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await Console.Out.WriteLineAsync("test test");
    }
}