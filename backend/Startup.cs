using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication(options =>
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

        services.AddControllersWithViews(); // Serviço adicionado para habilitar o uso de controllers
    }
}

/* O Startup.cs é um ficheiro essencial em qualquer aplicação ASP.NET Core. 
É usado para configurar os serviços necessários para a aplicação e para definir como a aplicação irá responder a solicitações HTTP. 
O método ConfigureServices é usado para configurar os serviços que a aplicação irá utilizar. */