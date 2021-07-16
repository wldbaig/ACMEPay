using API.BusinessLogic;
using API.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.WEB
{
    public class AppInitializationService : IHostedService
    {
        // We need to inject the IServiceProvider so we can create the scoped services
        private readonly IServiceProvider _serviceProvider; 
        public AppInitializationService(IServiceProvider serviceProvider)
        { 
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a new scope to retrieve scoped services
           
        }

        // noop
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
