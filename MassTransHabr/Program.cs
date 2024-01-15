using MassTransit;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq();
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserWebApi", Version = "v1" });
});
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "localhost:9001";//вынести в appsettings
    options.InstanceName = "SampleInstance";
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "UserWebApi v1");
    options.RoutePrefix = string.Empty;
});
app.UseRouting();

app.MapControllers();
app.Run();
