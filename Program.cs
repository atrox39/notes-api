using Microsoft.EntityFrameworkCore;
using notes;
using notes.Models;
using notes.Respository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NotesContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING")));
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Simple Notes API");
app.MapGroup("/api").MapApi();

app.Run();
