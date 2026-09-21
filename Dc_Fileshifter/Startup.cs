using System;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;
using Dc_Fileshifter.Configurations;
using Dc_Fileshifter.service;
using Dc_Fileshifter.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly:FunctionsStartup(typeof(Dc_Fileshifter.Startup))]
namespace Dc_Fileshifter
{
    public class Startup : FunctionsStartup
    {
        public Startup()
        {
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<ISharePointClient, SharePointClient>();
            builder.Services.AddSingleton<ISharePointProvider, SharePointProvider>();
            builder.Services.AddSingleton<ICdbConnectivity, CdbConnectivity>(); 
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver() // Ensure this is set
            };

            builder.Services.AddSingleton(options);
        }
    }
}

