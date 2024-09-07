using AnimeTrackingWeb;
using AnimeTrackingWeb.Controllers;
using AnimeTrackingWeb.Services;
using Microsoft.AspNetCore.Cors;
using Quartz;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

var builder = WebApplication.CreateBuilder(args);

// Register named HttpClient to get benefits of IHttpClientFactory
// and consume it with ITelegramBotClient typed client.
// More read:
//  https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests#typed-clients
//  https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        TelegramBotClientOptions options = new("5845820795:AAG5Odw1IRlkrnDZNnbcOuLjlFOm3f7RGv0");
        return new TelegramBotClient(options, httpClient);
    });

// Dummy business-logic service
builder.Services.AddScoped<UpdateHandlersService>();
// Add the required Quartz.NET services
builder.Services.AddQuartz(q =>  
{
    // Use a Scoped container to create jobs. I'll touch on this later
    q.UseMicrosoftDependencyInjectionScopedJobFactory();
});

// Add the Quartz.NET hosted service

builder.Services.AddQuartzHostedService(
    q => q.WaitForJobsToComplete = true);
// There are several strategies for completing asynchronous tasks during startup.
// Some of them could be found in this article https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-part-1/
// We are going to use IHostedService to add and later remove Webhook
builder.Services.AddHostedService<ConfigureWebhook>();


// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.UseHttpsRedirection();
// app.UseAuthorization();
app.MapBotWebhookRoute<BotController>(route: "/api/bot");
app.MapPost("", context =>
{
    return Task.CompletedTask;
});
app.MapControllers();
app.UseQuartz();
app.Run();