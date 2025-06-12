using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Domain;
using Test.Entity.Domain;
using Test.Repository.Domain;
using Microsoft.OpenApi.Models;
using TestWebApi.Services;
using Microsoft.Extensions.FileProviders;
using Test.AppService.Domain.Fork;
using Test.AppService.Domain.Security;
using Test.AppService.Domain.Fault;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IAuthProvider, AuthProvider>();
builder.Services.AddScoped<ISessionStorage, DataSessionStorage>();
//builder.Services.AddScoped<ISessionStorage, MemorySessionStorage>();

builder.Services.AddScoped<IDataRepository<User>, UserRepository>();
builder.Services.AddScoped<IDataRepository<UserSession>, UserSessionRepository>();
builder.Services.AddScoped<IDataRepository<ForkLift>, ForkLiftRepository>();
builder.Services.AddScoped<IDataRepository<ForkFault>, ForkFaultRepository>();

builder.Services.AddScoped<IDataFactory<IForkLift>, ForkLiftDataFactory>();
builder.Services.AddScoped<IDataProvider<IForkLift>, ForkLiftDataProvider>();
builder.Services.AddScoped<IDataService<IForkLift>, ForkLiftDataService>();
builder.Services.AddScoped<IDataValidator<IForkLift>, ForkLiftDataValidator>();

builder.Services.AddScoped<IDataFactory<IForkFault>, ForkFaultDataFactory>();
builder.Services.AddScoped<IDataProvider<IForkFault>, ForkFaultDataProvider>();
builder.Services.AddScoped<IDataService<IForkFault>, ForkFaultDataService>();
builder.Services.AddScoped<IDataValidator<IForkFault>, ForkFaultDataValidator>();

builder.Services.AddScoped<IDataRepository<ForkLift>, ForkLiftRepository>();
builder.Services.AddScoped<IDataRepository<ForkFault>, ForkFaultRepository>();
builder.Services.AddScoped<IDataFactory<IForkLift>, ForkLiftDataFactory>();
builder.Services.AddScoped<IDataProvider<IForkLift>, ForkLiftDataProvider>();
builder.Services.AddScoped<IDataService<IForkLift>, ForkLiftDataService>();

switch (builder.Configuration.GetConnectionString("dbServer"))
{
    case "pgSql":  builder.Services.AddDbContext<ForkDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("pgSql")));break;
    case "mySql": //builder.Services.AddDbContext<ForkDbContext>(options =>options.UseMySQL(builder.Configuration.GetConnectionString("mySql")));
                  break;
    default: builder.Services.AddDbContext<ForkDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("msSql")));break;
}





builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod(); // Важно!
    });
});




builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
     {
         
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = false,
             ValidateAudience = false,
             ValidateLifetime = true, 
             ValidateIssuerSigningKey = false, 
             RequireSignedTokens = false 
         };
     });

builder.Services.AddAuthorization();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.  
                        Enter 'Bearer' [space] and then your token in the text input below.  
                        Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");

app.UseStaticFiles();


if (app.Environment.IsDevelopment() || true)  app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, proxy-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});


app.Run();