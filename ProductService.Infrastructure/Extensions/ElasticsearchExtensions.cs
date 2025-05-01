using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Extensions
{
    public static class ElasticsearchExtensions
    {
        public static IServiceCollection AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var uri = new Uri(configuration["Elasticsearch:Uri"]!);
            var username = configuration["Elasticsearch:Username"];
            var password = configuration["Elasticsearch:Password"];
            var defaultIndex = configuration["Elasticsearch:DefaultIndex"];

            var settings = new ElasticsearchClientSettings(uri)
                .Authentication(new BasicAuthentication(username!, password!))
                .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
                .DefaultIndex(defaultIndex!)
                .EnableDebugMode();

            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client); // Register as a singleton

            return services;
        }
    }
}

