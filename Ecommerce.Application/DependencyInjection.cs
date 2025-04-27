using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ProductService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var thisAssembly = typeof(DependencyInjection).Assembly;
            services.Scan(scan => scan
                .FromAssemblies(thisAssembly)
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
            services.Scan(scan => scan
                .FromAssemblies(thisAssembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
            return services;
        }
    }
}

