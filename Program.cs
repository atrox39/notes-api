using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using notes.Routes;
using notes.Validations;
using notes.Models;
using notes.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.GlobalValidator(); // For Global validations

builder.Services.AddDbContext<NotesContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt => {
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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(x => x
  .WithOrigins("http://localhost:5173")
  .AllowAnyHeader()
  .AllowAnyMethod()
  .SetIsOriginAllowed(origin => true)
  .AllowCredentials()
);

app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapGet("/private", () => Results.Ok("This url is privated only view if you have a token")).RequireAuthorization();
app.MapGroup("/api").MapApi();

app.Run();
