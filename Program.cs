using ConnectionStringSecureAPI.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();

//builder.Services.AddSwaggerGen(options =>
//{
//    options.DocumentFilter<IncludeSpecificApisFilter>();
//});

builder.Services.AddSwaggerGen(options =>
{
    // Add Swagger docs for v1
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API v1",
        Version = "v1",
        Description = "Documentation for API version 1"
    });

    // Add Swagger docs for v2
    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API v2",
        Version = "v2",
        Description = "Documentation for API version 2"
    });

    // Add a document filter to group APIs by their version
    options.DocInclusionPredicate((version, apiDesc) =>
    {
        if (apiDesc.RelativePath?.StartsWith($"api/{version}/") == true)
        {
            return true;
        }
        return false;
    });
});

var app = builder.Build();

////Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//app.UseSwaggerUI();
//}

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
