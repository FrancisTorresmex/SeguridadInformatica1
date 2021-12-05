using Datos.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Negocio.Models.Commons;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string MiCors = "MiCors";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    //añadir el barer token en la documentación de swager (para que podamos usarlo como postman)
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WSMiVenta", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Colocar aqui SOLO el token",
        In = ParameterLocation.Header, //localización de texto
        Type = SecuritySchemeType.Http, //tipo de seguridad
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
           {securityScheme, new string[] { }}
        });

});

//Conexón a BD
builder.Services.AddDbContext<SeguridadInfContext>(item => item.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//JWT
var appSettingsSecction = builder.Configuration.GetSection("AppSettings"); //variable, el GetSection lleva entre parentesis el nombre de la variable declarada en appSettings.json
builder.Services.Configure<AppSettingsCommon>(appSettingsSecction);

var appSettings = appSettingsSecction.Get<AppSettingsCommon>();
var llave = Encoding.ASCII.GetBytes(appSettings.Secreto); //encryptamos mi secreto

//Autoenticación con JWT
builder.Services.AddAuthentication(d =>
{ //damos de alta el token
    d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(d =>
    {
        d.RequireHttpsMetadata = false;
        d.SaveToken = true; //vida del token osea que se pueda guardar
        d.TokenValidationParameters = new TokenValidationParameters // parametros de validación del token
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(llave),  //esta es la que dara el token, asi que le asignamos el secreto, ahora asignada en llave
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


//Agregar cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MiCors, builder =>
    {
        builder.WithOrigins("*");
        builder.WithHeaders("*");
        builder.WithMethods("*");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MiCors);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
