using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using WebApiNoSql.API.Authentication;
using WebApiNoSql.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("LibraryOpenApiSpecification", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Flight Plan API",
        Version = "v1",
        Description = "Web Api for Flight Plan"
    });
    options.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic authorization header using Bearer shceme"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basicAuth"
                }
            },
            new string[] {}
        }
    });
    options.EnableAnnotations();
});

builder.Services.AddCors();
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
builder.Services.AddScoped<IDataBaseAdapter, MongoDbDatabase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("swagger/LibraryOpenApiSpecification/swagger.json", "Library API");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors(options =>
{
    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

app.UseHsts();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
