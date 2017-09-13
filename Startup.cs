using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AspNetCoreSample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            ProviderConfig = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("openid-configuration.json")
                .Build();
            ClientConfig = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("client-config.json")
                .Build();
        }

        public IConfigurationRoot ProviderConfig { get; set; }
        public IConfigurationRoot ClientConfig { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddOpenIdConnect(options =>
            {
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.ResponseMode = null;
                options.DisableTelemetry = true;
                options.Authority = ProviderConfig["issuer"];
                options.ClientId = ClientConfig["client_id"];
                options.ClientSecret = ClientConfig["client_secret"];
                options.Scope.Clear();
                options.Scope.Add("openid");
            })
            .AddCookie(); 
            
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
