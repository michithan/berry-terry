using System.Configuration;
using System.Text.Json;
using berry.configuration;
using berry.interaction.actions;
using berry.interaction.ai;
using berry.interaction.handlers;
using berry.interaction.receivers;
using leash.chat.providers;
using leash.chat.providers.google;
using leash.clients.azuredevops;
using leash.clients.google;
using leash.scm.provider;
using leash.scm.provider.azuredevops;
using leash.ticketing.providers;
using leash.ticketing.providers.azuredevops;

var builder = WebApplication.CreateBuilder(args);

BerryConfiguration berryConfiguration = builder.Configuration.Get<BerryConfiguration>()
    ?? throw new ConfigurationErrorsException("BerryConfiguration is required. Please check the configuration file.");

// Add default services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

// Add custom services
// Configuration should be accessible to all services
builder.Services.AddSingleton(berryConfiguration);
builder.Services.AddSingleton(berryConfiguration.AiConfiguration);

// Add services to interact with with vendor specific systems
if (berryConfiguration.AzureDevOpsClientConfiguration is not null)
{
    builder.Services.AddSingleton(berryConfiguration.AzureDevOpsClientConfiguration);
    builder.Services.AddSingleton<IAzureDevOpsClient, AzureDevOpsClient>();
    builder.Services.AddTransient<IAzureDevOpsScmProvider, AzureDevOpsScmProvider>();
    builder.Services.AddTransient<IAzureDevOpsTicketingProvider, AzureDevOpsTicketingProvider>();

    builder.Services.AddTransient<IScmProvider, AzureDevOpsScmProvider>();
    builder.Services.AddTransient<ITicketingProvider, AzureDevOpsTicketingProvider>();
}
if (berryConfiguration.GoogleClientConfiguration is not null)
{
    builder.Services.AddSingleton(berryConfiguration.GoogleClientConfiguration);
    builder.Services.AddSingleton<IGoogleClient, GoogleClient>();
    builder.Services.AddTransient<IGoogleChatProvider, GoogleChatProvider>();

    builder.Services.AddTransient<IChatProvider, GoogleChatProvider>();
}

// Add services to interact with ai
builder.Services.AddTransient<IAiContext, AiContext>();

// Add services to receive vendor specific notifications
builder.Services.AddTransient<AzureDevOpNotificationReceiver>();
builder.Services.AddTransient<GoogleChatNotificationReceiver>();

// Add services to handle vendor agnostic events
builder.Services.AddTransient<IPullRequestHandler, PullRequestHandler>();
builder.Services.AddTransient<ITicketHandler, TicketHandler>();
builder.Services.AddTransient<IChatHandler, ChatHandler>();

// Add services to enable vendor specific actions with ai output
builder.Services.AddTransient<IPullRequestActor, PullRequestActor>();
builder.Services.AddTransient<ITicketActor, TicketActor>();
builder.Services.AddTransient<IChatActor, ChatActor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Add development services
    app.UseDeveloperExceptionPage();
    Console.WriteLine($"Config: {JsonSerializer.Serialize(berryConfiguration, new JsonSerializerOptions { WriteIndented = true })}");
}

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.MapHealthChecks("/health");

// Start web server
app.Run();

public partial class Program { } // This allows the file to be compiled as a partial class for testing purposes
