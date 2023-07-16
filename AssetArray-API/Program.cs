using AssetArray_API.Logic.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var authPassword = builder.Configuration["AuthSettings:Password"];

if (string.IsNullOrEmpty(authPassword))
{
    throw new InvalidOperationException("AuthSettings:ExpirationTime is not set in the configuration");
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IJWTTokenService, JWTTokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer( options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = false,
        //ValidIssuer = ConfigurationBinder.GetValue<string>("AuthSettings:apiServerURL");
        //ValidAudience = ConfigurationBinder.GetValue<string>("AuthSettings:applicationURL");
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authPassword))
    };
});
  

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
