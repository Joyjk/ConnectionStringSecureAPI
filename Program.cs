using AspNetCoreRateLimit;
using ConnectionStringSecureAPI.ActionFilters;
using ConnectionStringSecureAPI.Extension;
using ConnectionStringSecureAPI.Middleware;
using Microsoft.AspNetCore.Mvc.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCustomRateLimiting(builder.Configuration);

builder.Services.AddControllers(options =>
{
    // options.Filters.AddService<LoggingActionFilter>(); // Add filter globally
});


builder.Services.AddScoped<LoggingActionFilter>();

builder.Services.AddSwaggerExtension();


//builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

//app.UseExceptionHandler("/Home/Error");



// Apply rate limiting middleware
app.UseIpRateLimiting();


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
