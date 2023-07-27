using System.Security.Claims;
using System.Text;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Data.UnitOfWork;
using InventoryManagementSystem.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace InventoryManagementSystem;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        //Entity setup
        builder.Services.AddDbContext<InventoryDbContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

        //UOW set up
        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

        //AutoMapper set up
        builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);  

        //JWT authentication and Validation middleware setup
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                         .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                         {
                             options.TokenValidationParameters = new TokenValidationParameters
                             {
                                 ValidateIssuer = false,
                                 ValidateAudience = false,
                                 ValidateLifetime = true,
                                 ValidateIssuerSigningKey = true,
                                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
                             };
                         });

        //Add authorizations
        builder.Services.AddAuthorization(option =>
        {
            option.AddPolicy("UserOnly", policy => policy.RequireClaim(ClaimTypes.Role, "User"));

            option.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));

            option.AddPolicy("EditorUserOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Editor User"));

            option.AddPolicy("EditorAdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Editor Admin"));

            option.AddPolicy("MixAdminAndEditor", policy => policy.RequireAssertion(
                context => context.User.HasClaim(
                    claim => claim.Type is ClaimTypes.Role && claim.Value is "Admin" or "Editor Admin")));

            option.AddPolicy("AllRolesOnly", policy => policy.RequireAssertion(
                context => context.User.HasClaim(
                    claim => claim.Type is ClaimTypes.Role && claim.Value is "Admin" or "User" or "Editor User" or "Editor Admin")));

            option.AddPolicy("MixEditorUserOrAdmin", policy => policy.RequireAssertion(
                context => context.User.HasClaim(
                    claim => claim.Type is ClaimTypes.Role && claim.Value is "Editor User" or "Editor Admin")));
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
    }
}

