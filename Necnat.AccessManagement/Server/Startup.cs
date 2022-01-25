using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Necnat.AccessManagement.Server.Interfaces;
using Necnat.AccessManagement.Server.Options;
using Necnat.Server.DbContexts;
using Necnat.Server.Helpers.Swagger;
using Necnat.Server.Interfaces;
using Necnat.Server.Services;
using Necnat.Shared;
using Necnat.Shared.Interfaces;
using Necnat.Shared.Settings;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Necnat.AccessManagement.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            DefaultConnection = configuration.GetConnectionString("DefaultConnection");
            AccessManagementConnectionString = configuration["NecnatServerSettings:AccessManagementConnectionString"];
        }

        public IConfiguration Configuration { get; }
        public string DefaultConnection { get; }
        public string AccessManagementConnectionString { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
            services.AddRazorPages();

            services.AddScoped<NecnatServerSettings>(x => Configuration.GetSection(nameof(NecnatServerSettings)).Get<NecnatServerSettings>());
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.UseAllOfType<INecnatAccessManagementService>(new[] { typeof(Startup).Assembly });
            services.UseAllOfType<IValidator>(new[] { typeof(NecnatSharedRef).Assembly });

            services.AddDbContext<NecnatAccessManagementDbContext>(options =>
                options.UseSqlServer(AccessManagementConnectionString));

            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("pt-BR")
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider()
                };
            });

            services.AddAuthorization();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Bearer";
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
                options.DefaultSignInScheme = "Bearer";
                options.DefaultForbidScheme = "Bearer";
            })
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["AuthConfiguration:AuthUrl"];
                    options.ApiName = Configuration["AuthConfiguration:ApiResourceName"];
                    options.RequireHttpsMetadata = false;
                });

            services.AddApiVersioning(options => { options.ReportApiVersions = true; });
            services.AddVersionedApiExplorer(options => { options.GroupNameFormat = "'v'VVV"; });

            //Swagger
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(Configuration["AuthConfiguration:AuthUrl"] + @"/connect/authorize"),
                            TokenUrl = new Uri(Configuration["AuthConfiguration:AuthUrl"] + @"/connect/token"),
                            Scopes = new Dictionary<string, string> {
                                { Configuration["AuthConfiguration:ApiResourceName"], Configuration["AuthConfiguration:ApiResourceName"] }
                            }
                        }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();

                options.IncludeXmlComments(XmlCommentsFilePath.Get(typeof(Startup)));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());

                c.OAuthClientId(Configuration["AuthConfiguration:ClientId"]);
                c.OAuthAppName(Configuration["AuthConfiguration:ApiResourceName"]);
                c.OAuthUsePkce();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
