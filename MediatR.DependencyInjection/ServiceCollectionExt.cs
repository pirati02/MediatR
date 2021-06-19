using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MediatR.DependencyInjection
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection AddMediatR(this IServiceCollection services, Assembly mediatRInstancesAssembly,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            if (mediatRInstancesAssembly == null)
            {
                throw new ArgumentNullException($"Provide assembly for MediatR mediatRInstancesAssembly");
            }

            var requests = GetClassesFromAssemblyByType(mediatRInstancesAssembly, typeof(IRequest<>));
            var requestHandlers = GetClassesFromAssemblyByType(mediatRInstancesAssembly, typeof(IRequestHandler<,>));

            var requestAndHandlerContracts = requests.Select(request =>
            {
                var requestHandler = requestHandlers.SingleOrDefault(a =>
                    a.GetInterface("IRequestHandler`2")!.GetGenericArguments()[0] == request);
                return new
                {
                    request, requestHandler
                };
            }).ToDictionary(a => a.request, a => a.requestHandler);


            services.TryAdd(requestHandlers.Select(a => new ServiceDescriptor(a, a, lifetime)));

            services.AddSingleton<IMediatR>(a => new MediatR(a.GetRequiredService, requestAndHandlerContracts));
            return services;
        }

        private static IEnumerable<Type> GetClassesFromAssemblyByType(Assembly mediatRInstancesAssembly,
            Type matchingType)
        {
            return mediatRInstancesAssembly.ExportedTypes
                .Where(a =>
                    !a.IsInterface
                    && !a.IsAbstract
                    && a.GetInterfaces().Any(b =>
                        b.IsGenericType
                        && b.GetGenericTypeDefinition() == matchingType
                    )
                )
                .ToList();
        }
    }
}