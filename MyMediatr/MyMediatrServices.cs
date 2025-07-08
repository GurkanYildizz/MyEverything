using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyMediatr
{
    public static class MyMediatrServices
    {
        public static IServiceCollection AddMyMediatr(this IServiceCollection services, Assembly assembly)
        {

            var fo = assembly.GetTypes().Where(s => !s.IsAbstract && !s.IsInterface);

            var ft = fo.SelectMany(a => a.GetInterfaces()
                .Where(s => s.IsGenericType &&
                (s.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                s.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                .Select(f => new { Interface = f, Implementation = a }));



            services.AddTransient<ISender, Sender>();
            foreach (var f in ft)
            {
                services.AddScoped(f.Interface, f.Implementation);
            }



            return services;
        }
    }
}
