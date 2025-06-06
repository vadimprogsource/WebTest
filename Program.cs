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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IAuthProvider, AuthProvider>();

builder.Services.AddScoped<IDataRepository<User>, UserRepository>();
builder.Services.AddScoped<IDataRepository<UserSession>, UserSessionRepository>();
builder.Services.AddScoped<IDataRepository<ForkLift>, ForkLiftRepository>();
builder.Services.AddScoped<IDataRepository<ForkFault>, ForkFaultRepository>();

builder.Services.AddScoped<IDataFactory<IForkLift>, ForkLiftDataAccessProvider>();
builder.Services.AddScoped<IDataAccessProvider<IForkLift>, ForkLiftDataAccessProvider>();

builder.Services.AddScoped<IDataFactory<IForkFault>, ForkFaultDataAccessProvider>();
builder.Services.AddScoped<IDataAccessProvider<IForkFault>, ForkFaultDataAccessProvider>();

builder.Services.AddSingleton<IDataValidator<IForkLift>, ForkLiftDataValidator>();
builder.Services.AddSingleton<IDataValidator<IForkFault>, ForkFaultDataValidator>();



builder.Services.AddDbContext<ForkDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("pgSql")));
//builder.Services.AddDbContext<ForkDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("msSql")));


//builder.Services.AddDbContext<ForkDbContext>(options =>options.UseMySQL(builder.Configuration.GetConnectionString("mySql")));


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
if (app.Environment.IsDevelopment())
{

   

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

