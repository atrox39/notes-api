using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using notes;
using notes.Models;
using notes.Respository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NotesContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING")));
builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt => {
  var singning = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!));
  var credentials = new SigningCredentials(singning, SecurityAlgorithms.HmacSha256Signature);
  opt.RequireHttpsMetadata = false;
  opt.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateAudience = false,
    ValidateIssuer = false,
    IssuerSigningKey = singning,
  };
});
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IJwtRepository, JwtRepository>();

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Simple Notes API");
app.MapGet("/private", () => Results.Ok("This url is privated only view if you have a token")).RequireAuthorization();
app.MapGroup("/api").MapApi();

app.Run();
