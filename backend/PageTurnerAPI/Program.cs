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
using Microsoft.OpenApi.Models;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner.
builder.Services.AddControllers();



// Configuração do serviço de autenticação JWT
builder.Services.AddAuthentication(cfg => {
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    
    x.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8
            .GetBytes("PDS2024$3cr3tK3y@Jwt#2024PageTurnerAPI")
            // .GetBytes(configuration["ApplicationSettings:JWT_Secret"])

        ),
            LifetimeValidator = (before, expires, token, param) => {
            // Calcular a hora atual mais 1 hora
            var hourFromNow = DateTime.UtcNow.AddHours(8);
            // Verificar se a data atual está dentro do período de validade do token
            return expires.HasValue && expires > hourFromNow;
        },
        ValidateIssuer = false,
        // ValidIssuer = "http://localhost:3000", // apenas valida TOKEN emitidos por este sevidor, no caso o nosso
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero,
    };
});


// Adicionar as configurações do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new() { Title = "PageTurnerAPI", Version = "Beta", 
                                Description = "API para o projeto PageTurner" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Description = "É necessaria autenticação. Fazer login com algum " +
                        "utilizador e colar aqui o token, temos de escrever 'Bearer'. Exemplo: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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
    options.AddPolicy(name: "Dev2",
        policy  =>
        {
            policy.WithOrigins("http://localhost:3000", "http://192.168.1.71:3000", "http://localhost:3000/", "http://172.20.10.4:3000")
                  .WithMethods("GET", "POST", "PUT", "DELETE")
                  .AllowAnyHeader()
                  .AllowCredentials();
        });

        options.AddPolicy("Dev", builder =>
        {
            // Allow multiple methods
            builder.WithMethods("GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS")
            .WithHeaders(
                HeaderNames.Accept,
                HeaderNames.ContentType,
                HeaderNames.Cookie,
                HeaderNames.SetCookie,
                HeaderNames.Authorization)
            .AllowCredentials()
            .SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrWhiteSpace(origin)) return false;
                // Only add this to allow testing with localhost, remove this line in production!
                if (origin.ToLower().StartsWith("http://localhost")) return true;
                // Insert your production domain here.
                if (origin.ToLower().StartsWith("http://192.168.1.71")) return true;
                if (origin.ToLower().StartsWith("http://172.20.10.4")) return true;
                if (origin.ToLower().StartsWith("http://172.16.8.133")) return true;
                return false;
            });
        });


});

// Adicionar serviços API e email
builder.Services.AddScoped<ServicoAPI>(); // O add scoped cria uma instância do serviço uma vez, por solicitação HTTP do cliente
                                           // e será reutilizada em todas as injeções de dependência durante a mesma solicitação
builder.Services.AddTransient<IEmailSender, EmailSender>(); // Cria uma instância sempre que solicitado

// Adicionar políticas de autorização personalizadas
builder.Services.AddAuthorization(options =>
{
    options.AddAdminPolicy(JwtBearerDefaults.AuthenticationScheme);
    options.CheckUserPolicy(JwtBearerDefaults.AuthenticationScheme);
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

app.UseCors("Dev");
app.UseCors("Dev1");

app.UseHttpsRedirection();

app.UseRouting();

// Middleware de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
