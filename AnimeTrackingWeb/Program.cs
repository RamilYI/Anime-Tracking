using AnimeTrackingApi;
using AnimeTrackingWeb;
using AnimeTrackingWeb.Controllers;
using AnimeTrackingWeb.Services;
using Hangfire;
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
builder.Services.AddScoped<AnimeTracking>();
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
builder.Services.AddHangfire(configuration => configuration
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings());
Hangfire.GlobalConfiguration.Configuration
    .UseSqlServerStorage("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=AnimeTracking;Integrated Security=True;Multiple Active Result Sets=True");
builder.Services.AddHangfireServer();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AnimeTrackingDemo.AnimeTrackingWeb v1"));
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseHangfireDashboard();
app.MapBotWebhookRoute<BotController>(route: "/api/bot");
app.MapBotWebhookRoute<GetSeasonController>(route: "/api/bot/getSeason");
app.MapControllers();
app.UseQuartz();
app.Run();