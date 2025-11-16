using System.Text;
using GeoGoAPI._models;
using GeoGoAPI._repositories.implementation;
using GeoGoAPI._repositories.interfaces;
using GeoGoAPI._services.implementations;
using GeoGoAPI._services.interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

//build on localhost - currently allowing access from this port on the local network by <ip-of-pc>:5000/swagger
builder.WebHost.UseUrls("http://0.0.0.0:5000");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "GeoGo API",
            Version = "v1",
            Description = "GeoGo HTTP API (JWT secured)",
        }
    );

    // --- Bearer auth: Authorize button in Swagger ---
    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste your JWT access token here.",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
    };
    opt.AddSecurityDefinition("Bearer", jwtScheme);

    opt.AddSecurityRequirement(
        new OpenApiSecurityRequirement { { jwtScheme, Array.Empty<string>() } }
    );

    opt.EnableAnnotations();
    opt.SupportNonNullableReferenceTypes();
    var xml = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xml);
    if (File.Exists(xmlPath))
        opt.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

builder.Services.AddDbContext<GeoGoDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Dependency injection  TODO: Should be separated into services and repos

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPlaceRepository, PlaceRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserTwinRepository, UserTwinRepository>();
builder.Services.AddScoped<ICategoryWeightsRepository, CategoryWeightsRepository>();
builder.Services.AddScoped<IPlaceLikesRepository, PlaceLikesRepository>();
builder.Services.AddScoped<IInteractionEventRepository, InteractionEventRepository>();
builder.Services.AddScoped<IVirtualPlaceRepository, VirtualPlaceRepository>();
builder.Services.AddScoped<IVirtualObjectRepository, VirtualObjectRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPlaceService, PlaceService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserTwinService, UserTwinService>();
builder.Services.AddScoped<ICategoryWeightsService, CategoryWeightsService>();
builder.Services.AddScoped<IPlaceLikesService, PlaceLikesService>();
builder.Services.AddScoped<IInteractionEventService, InteractionEventService>();
builder.Services.AddScoped<IInteractionProcessorService, InteractionProcessorService>();
builder.Services.AddScoped<IVirtualPlaceService, VirtualPlaceService>();
builder.Services.AddScoped<IVirtualObjectService, VirtualObjectService>();

builder.Services.AddControllers();

// JWT auth
var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("Missing TokenKey");
if (tokenKey.Length < 64)
    throw new Exception("Token key needs to be longer");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

Console.WriteLine($"Running in {builder.Environment.EnvironmentName} mode.");

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
        };
    });

var app = builder.Build();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeoGo API v1");
    c.RoutePrefix = "swagger";
    c.DisplayRequestDuration();
    c.DefaultModelsExpandDepth(-1);
    c.DocExpansion(DocExpansion.None);
    c.EnableTryItOutByDefault();
    c.DisplayOperationId();
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
