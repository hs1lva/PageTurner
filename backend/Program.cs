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

#region Serviços adicionados
//Adicionar servico para DbContext
builder.Services.AddDbContext<PageTurnerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// cors
// cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOriginPolicy",
        builder =>
        {
            builder.AllowAnyOrigin() // Permite acesso de qualquer origem
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});



//adiciona servico API
builder.Services.AddScoped<ServicoAPI>();//O add scoped cria uma instancia do serviço uma vez, por solicitação HTTP do cliente
                                            //e será reutilizada em todas as injeçoes de dependência durante a mesma solicitação
//adiciona servico email
builder.Services.AddTransient<IEmailSender, EmailSender>();//é ligeiramente diferente:
                                                            // é criada uma instancia sempre que solicitado. 

                            //no fundo acha que estão bem assim, cada um tem um proposito diferente.

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAnyOriginPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
