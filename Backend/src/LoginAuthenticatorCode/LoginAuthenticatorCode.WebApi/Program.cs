using LoginAuthenticatorCode.CrossCutting.DependencyInjection.DbConfig;
using LoginAuthenticatorCode.CrossCutting.DependencyInjection.Repository;
using LoginAuthenticatorCode.CrossCutting.DependencyInjection.Service;
using LoginAuthenticatorCode.CrossCutting.DependencyInjection.AutoMapper.Config;
using LoginAuthenticatorCode.CrossCutting.DependencyInjection.Validation.Base;
using LoginAuthenticatorCode.Shared.Jwt;
using LoginAuthenticatorCode.CrossCutting.DependencyInjection.CacheConfig;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));

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
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
