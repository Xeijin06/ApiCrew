using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using CrewMobile.Api.Models;
using CrewMobileApi.Apis.Interfaces;
using CrewMobileApi.Apis;
using CrewMobileApi.Services;
using CrewMobile.Api.Services.Interface;
using CrewMobile.Api.Services;
using CrewMobile.Common.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

#region Advanced Authentication Configuration
/*builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    ConfigurationBinder.Bind(builder.Configuration.GetSection("AzureAd"), options);
    options.Authority = $"{builder.Configuration["AzureAd:Instance"]}{builder.Configuration["AzureAd:TenantId"]}/v2.0";
    options.Audience = options.Audience = builder.Configuration["AzureAd:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = $"https://sts.windows.net/{builder.Configuration["AzureAd:TenantId"]}/", // Actualiza este valor
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});*/
#endregion

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddControllers(
                options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true).AddNewtonsoftJson();

// Register ICopaAPIs and ICopaSoap services
builder.Services.AddScoped<ICopaAPIs, CopaAPIs>();
builder.Services.AddScoped<ICopaSoap, CopaSoap>();

builder.Services.Configure<AzureStorageOptions>(builder.Configuration.GetSection("AzureStorage"));
builder.Services.AddSingleton<IBlobStorageService, BlobStorageService>();
builder.Services.AddSingleton<ILogStorageAccountService, LogStorageAccountService>();

// Register Graph Service
// Validar si se debe registrar como Singleton o Transient
//builder.Services.AddSingleton<GraphService>();
builder.Services.AddHttpClient<GraphService>();

// Load configuration from appsettings.json
// Se elimina para evitar que se cargue nuevamente la configuración, ya que WebApplication.CreateBuilder(args) ya lo hace automáticamente
// builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configure DbContext with connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
