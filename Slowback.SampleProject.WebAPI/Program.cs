using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Slowback.SampleProject.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

JsonConvert.DefaultSettings = () =>
{
    var settings = new JsonSerializerSettings();

    settings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });

    return settings;
};

Startup.PublishAppDbConnection(builder.Configuration);
Startup.SetUpJwtSettingsPublisher(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Startup.EnableLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();