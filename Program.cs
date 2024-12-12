using ConnectionStringSecureAPI.ActionFilters;
using ConnectionStringSecureAPI.Extension;
using Microsoft.AspNetCore.Mvc.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers(options =>
{
    options.Filters.AddService<LoggingActionFilter>(); // Add filter globally
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddScoped<LoggingActionFilter>();

builder.Services.AddSwaggerExtension();

//builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Add Swagger endpoints for both versions
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
