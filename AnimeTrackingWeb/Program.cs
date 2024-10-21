using AnimeTrackingApi;
using AnimeTrackingWeb;
using AnimeTrackingWeb.Controllers;
using AnimeTrackingWeb.Interfaces;
using AnimeTrackingWeb.Models;
using AnimeTrackingWeb.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
var botConfigSection = builder.Configuration.GetSection("BotConfiguration");
builder.Services.Configure<BotConfiguration>(botConfigSection);
builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        TelegramBotClientOptions options = new(botConfigSection.Get<BotConfiguration>()!.BotToken);
        return new TelegramBotClient(options, httpClient);
    });

builder.Services.AddScoped<UpdateHandlersService>();
builder.Services.AddSingleton<IAnimeTracking, AnimeTracking>();
builder.Services.AddHostedService<ConfigureWebhook>();

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(configuration => configuration
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(config =>
                    config.UseNpgsqlConnection(botConfigSection.Get<BotConfiguration>()!.HangfireConnection)));
builder.Services.AddHangfireServer();

builder.Services.AddDbContextPool<UserContext>(options => options
    .UseNpgsql(botConfigSection.Get<BotConfiguration>()!.UserTableConnection));
builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<UserContext>();
//    context.Database.EnsureCreated();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AnimeTrackingDemo.AnimeTrackingWeb v1"));
}

app.UseRouting();
app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseHangfireDashboard();
app.MapBotWebhookRoute<BotController>(route: "/api/bot");
app.MapBotWebhookRoute<GetSeasonController>(route: "/api/bot/getSeason");
app.MapControllers();
app.Run();