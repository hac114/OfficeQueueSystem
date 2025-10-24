using Microsoft.Extensions.DependencyInjection;
using Repository.Interface;
using Repository.Service;

namespace OfficeQueueSystem
{
    public static class StartUpConfigurator
    {
        public static void AddServiceDb(this IServiceCollection services)
        {
            // Repository
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            // Aggiungi qui altri repository futuri
            // services.AddScoped<IDeskRepository, DeskRepository>();
        }
    }
}