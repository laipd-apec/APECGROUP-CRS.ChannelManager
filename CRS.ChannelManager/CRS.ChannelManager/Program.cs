using CRS.ChannelManager.Extensions;
using CRS.ChannelManager.Infrastructure.Shares.Kafka;
using CRS.ChannelManager.Library.Mapper;
using FastEndpoints;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve; });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
builder.Services.AddExtensionHealthChecks();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>().AddHttpContextAccessor();
builder.Services.AddAuthenticationExtensions(builder.Configuration);
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddUnitOfWork();
builder.Services.AddConfigure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddElasticsearch(builder.Configuration);
builder.Services.AddHostedService(serviceProvider => serviceProvider.GetRequiredService<IConsumerService>());
AutoMapperExtention.AutoMapperConfigureServices(builder.Services);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors();

if (true) //app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHealthChecks("/channal-manager-health");
app.AddAutoMigration();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<HandlerMiddleware>();
app.UseFastEndpoints();
app.UseOpenApi();
app.UseSwaggerUi();


app.MapControllers();

app.Run();
