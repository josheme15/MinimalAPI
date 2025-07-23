using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalAPI;
using MinimalAPI.Data;
using MinimalAPI.DTO;
using MinimalAPI.EndPoint;
using MinimalAPI.Servicios;
using NLog.Extensions.Logging;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var cadenaConexionSQL = new AccesoDatos(builder.Configuration.GetConnectionString("SQL"));
builder.Services.AddSingleton(cadenaConexionSQL);
builder.Services.AddScoped<IServicioProductos, ServicioProduct>();  
builder.Services.AddScoped<IServicioAuth, ServicioAuth>();
builder.Services.AddScoped<IServicioCliente, ServicioCliente>();
builder.Services.AddScoped<IServicioVenta, ServicioVenta>();
builder.Host.ConfigureLogging((hostingContext, logging) =>
{
    logging.AddNLog();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:ClaveSecreta"]))
        };
    });


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MinimalAPi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autorizacion JWT esquema. \r\n\r\n Escribe 'Bearer' [espacio] y escribe el token proporcionado.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigurarVentaEndpoints();
app.ConfigurarClienteEndpoints();
app.ConfigurarLoginEndpoints();
app.ConfigurarProductoEndpoints();
app.UseHttpsRedirection();
app.Run();