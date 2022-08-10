using APICatalogo;
using APICatalogo.DTOs.Mapping;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions
.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "apiagenda", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Header de autorização JWT usando o esquema Bearer.\r\n\r\nInforme 'Bearer'[espaço] e o seu token.\r\n\r\nExamplo: \'Bearer 12345abcdef\'",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
          new OpenApiSecurityScheme
          {
             Reference = new OpenApiReference
             {
                 Type = ReferenceType.SecurityScheme,
                 Id = "Bearer"
             }
          },
          new string[] {}
       }
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

string mySqlConneciton = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>
                            (options => options.UseMySql(mySqlConneciton, 
                            ServerVersion.AutoDetect(mySqlConneciton)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
                     ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
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
app.UseRouting();
app.UseAuthorization();

app.UseCors(opt => opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
