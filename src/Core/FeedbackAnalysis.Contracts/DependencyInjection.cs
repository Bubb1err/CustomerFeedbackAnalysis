using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FeedbackAnalysis.Contracts
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddContracts(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
