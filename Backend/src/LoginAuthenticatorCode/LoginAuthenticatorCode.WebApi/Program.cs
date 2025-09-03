using LoginAuthenticatorCode.CrossCutting.DependencyInjection.AutoMapper.Config;
using LoginAuthenticatorCode.CrossCutting.DependencyInjection.CacheConfig;
using LoginAuthenticatorCode.CrossCutting.DependencyInjection.DbConfig;
using LoginAuthenticatorCode.CrossCutting.DependencyInjection.Repository;
using LoginAuthenticatorCode.CrossCutting.DependencyInjection.Service;
using LoginAuthenticatorCode.CrossCutting.DependencyInjection.Validation.Base;
using LoginAuthenticatorCode.Shared.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Services

builder.Services.AddSqlServerDependency(builder.Configuration);
builder.Services.AddSqlRepositoryDependency();
builder.Services.AddServiceDependency();
builder.Services.AddMapperConfiguration();
builder.Services.AddValidators();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCacheServerDependency(builder.Configuration);

#endregion Services

#region AuthenticationJwt

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
            ClockSkew = TimeSpan.Zero // Elimina o tempo de tolerância para expiração do token
        };
    });

#endregion AuthenticationJwt

#region Cors

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
                            builder => builder.WithOrigins("http://localhost:5173")// Permitir requisições do Vite
                          .AllowAnyMethod() // Permitir qualquer método (GET, POST, PUT, DELETE, etc.)
                          .AllowAnyHeader() // Permitir qualquer header
                          .AllowCredentials()); // Permitir cookies e autenticação
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();