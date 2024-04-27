using backend.Interface;
using backend.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PageTurnerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner.
builder.Services.AddControllers();

// Configuração do serviço de autenticação por cookies

builder.Services.AddAuthentication(AuthorizationPolicy.AuthScheme)
.AddCookie(AuthorizationPolicy.AuthScheme);


// Configuração do serviço de autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Definir como true se quisermos validar o emissor
            ValidateAudience = false, // Definir como true se quisermos validar o público-alvo
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("$3cr3tK3y@Jwt#2024")) // Utilizar a mesma secret key aqui que esta no modelo JwtTokenGenerator
        };
    });

// Adicionar as configurações do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração da autenticação do Google
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = "606115652111-6mup70jr8qejlblfgg4spfati22h8ecv.apps.googleusercontent.com";
    options.ClientSecret = "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";
});

builder.Services.AddControllersWithViews(); // Serviço adicionado para habilitar o uso de controllers

#region Serviços adicionados

// Adicionar serviço para DbContext
builder.Services.AddDbContext<PageTurnerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configurar CORS
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

// Adicionar serviços API e email
builder.Services.AddScoped<ServicoAPI>(); // O add scoped cria uma instância do serviço uma vez, por solicitação HTTP do cliente
                                           // e será reutilizada em todas as injeções de dependência durante a mesma solicitação
builder.Services.AddTransient<IEmailSender, EmailSender>(); // Cria uma instância sempre que solicitado

// Adicionar políticas de autorização personalizadas
builder.Services.AddAuthorization(options =>
{
    options.AddAdminPolicy();
    options.CheckUserPolicy();
    
    // podemos utilizar mais autenticações, ver depois com Hugo Silva
    // options.AddAdminPolicy(JwtBearerDefaults.AuthenticationScheme);
    // options.CheckUserPolicy(JwtBearerDefaults.AuthenticationScheme);

});


#endregion

var app = builder.Build();

// Configure o pipeline de solicitação HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAnyOriginPolicy");

app.UseHttpsRedirection();

app.UseRouting();

// Middleware de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
