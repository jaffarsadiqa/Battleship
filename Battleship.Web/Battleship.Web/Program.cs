using Battleship.Common;
using Battleship.Common.Security;
using Battleship.Model;
using Battleship.Model.Validators;
using BattleShip.Service;
using BattleShip.Service.Interface;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AuthSchemeConstants = Battleship.Common.Constants.AuthScheme;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost","https://jaffar.hofs.app/", "https://www.jaffar.hofs.app/")
                              .SetIsOriginAllowed((host) => true)
                              // .SetIsOriginAllowedToAllowWildcardSubdomains()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                      });
});

builder.Services.AddControllers();


// Add JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration[AuthSchemeConstants.JwtIssuer],
        ValidAudience = builder.Configuration[AuthSchemeConstants.JwtAudience],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration[AuthSchemeConstants.JwtKey])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddMemoryCache();
builder.Services.AddAuthorization();

builder.Services.AddMvc(options =>
{
    options.Filters.Add(new ValidationFilter());
}).AddFluentValidation(fv => { });
// can then manually register validators
builder.Services.AddTransient<IValidator<Ship>, ShipValidator>();
builder.Services.AddTransient<IValidator<Board>, BoardValidator>();
builder.Services.AddTransient<IValidator<Coordinate>, CoordinateValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IBoardService, BoardService>();
builder.Services.AddTransient<IShipService, ShipService>();
builder.Services.AddTransient<IPlayerService, PlayerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{*/
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
