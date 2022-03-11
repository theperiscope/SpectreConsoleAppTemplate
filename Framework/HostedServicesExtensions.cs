using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SpectreConsoleAppTemplate.Framework
{
    public static class HostedServicesExtensions
    {
        /// <summary>
        /// Adds all the Workers that inherit from IHostedService, using AddHostedService, that will also start the Workers with the service, differently from what AddSingleton would do.
        /// </summary>
        public static IServiceCollection AddHostedServices(this IServiceCollection services, List<Assembly> workersAssemblies)
        {
            MethodInfo methodInfo =
                typeof(ServiceCollectionHostedServiceExtensions)
                    .GetMethods()
                    .FirstOrDefault(p => p.Name == nameof(ServiceCollectionHostedServiceExtensions.AddHostedService));

            if (methodInfo == null)
                throw new Exception($"Impossible to find the extension method '{nameof(ServiceCollectionHostedServiceExtensions.AddHostedService)}' of '{nameof(IServiceCollection)}'.");

            IEnumerable<Type> hostedServices_FromAssemblies = workersAssemblies.SelectMany(a => a.DefinedTypes).Where(x => x.GetInterfaces().Contains(typeof(IHostedService))).Select(p => p.AsType());

            foreach (Type hostedService in hostedServices_FromAssemblies)
            {
                if (typeof(IHostedService).IsAssignableFrom(hostedService))
                {
                    var genericMethod_AddHostedService = methodInfo.MakeGenericMethod(hostedService);
                    _ = genericMethod_AddHostedService.Invoke(obj: null, parameters: new object[] { services }); // this is like calling services.AddHostedService<T>(), but with dynamic T (= backgroundService).
                }
            }

            return services;
        }

    }
}