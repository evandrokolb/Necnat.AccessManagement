using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Necnat.Client.Extensions;
using Necnat.Shared.HttpClients.NamHttpClients;
using Necnat.Shared.Settings;
using Radzen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Necnat.AccessManagement.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var necnatClientSettingsContainer = builder.Configuration.Get<NecnatClientSettingsContainer>();

            builder.Services.AddHttpClient<NamHttpClient>(client => client.BaseAddress = new Uri(necnatClientSettingsContainer.NecnatClientSettings.ApplicationUrl))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            //Authentication
            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("oidc", options.ProviderOptions);
            });

            //Blazored Local Storage
            builder.Services.AddBlazoredLocalStorage();

            //SweetAlert
            builder.Services.AddSweetAlert2();

            //Radzen
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddScoped<ContextMenuService>();

            //Necnat
            builder.Services.AddNecnatServices();
            builder.Services.AddSingleton<NecnatClientSettings>(x => necnatClientSettingsContainer.NecnatClientSettings);

            //Localization
            builder.Services.AddLocalization();

            var host = builder.Build();
            var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
            var result = await jsInterop.InvokeAsync<string>("blazorCulture.get");
            if (result != null)
            {
                var culture = new CultureInfo(result);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }

            await builder.Build().RunAsync();
        }
    }
}
