using backend.Interface;
using backend.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Adicionar servico para DbContext
builder.Services.AddDbContext<PageTurnerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ServidorDB"));
});

//adiciona servico API
builder.Services.AddScoped<ServicoAPI>();
//adiciona servico email
builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
