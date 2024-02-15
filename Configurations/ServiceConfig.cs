using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using notes.Models;
using notes.Repository;
using notes.Validations;

namespace notes.Configurations
{
  public static class ServiceConfig
  {
    public static WebApplicationBuilder ServiceAppConfig(this WebApplicationBuilder builder)
    {
      builder.GlobalValidator(); // For Global validations
      builder.Services.AddDbContext<NotesContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
      builder.Services.AddAuthorization();
      builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt =>
      {
        var singning = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSecret"]!));
        var credentials = new SigningCredentials(singning, SecurityAlgorithms.HmacSha256Signature);
        opt.RequireHttpsMetadata = false;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateAudience = false,
          ValidateIssuer = false,
          IssuerSigningKey = singning,
        };
      });
      builder.Services.AddOutputCache();
      builder.Services.AddCors();
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();
      builder.Services.AddAutoMapper(typeof(Program));
      builder.Services.AddScoped<IAuthRepository, AuthRepository>();
      builder.Services.AddScoped<IJwtRepository, JwtRepository>();
      builder.Services.AddScoped<INoteRepository, NoteRepository>();
      return builder;
    }
  }
}