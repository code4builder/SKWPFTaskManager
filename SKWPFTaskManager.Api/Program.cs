using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SKWPFTaskManager.Api.Models;
using SKWPFTaskManager.Api.Models.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // specifies whether the publisher will be validated when validating the token
                    ValidateIssuer = true,
                    // a string representing the publisher
                    ValidIssuer = AuthOptions.ISSUER,

                    // whether the consumer of the token will be validated
                    ValidateAudience = true,
                    // setting of consumer token
                    ValidAudience = AuthOptions.AUDIENCE,
                    // whether lifetime will be validated
                    ValidateLifetime = true,

                    // security key setting
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    // security key validation
                    ValidateIssuerSigningKey = true,
                };
            });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();