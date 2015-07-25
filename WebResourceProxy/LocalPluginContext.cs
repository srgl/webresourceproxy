using System;
using Microsoft.Xrm.Sdk;

namespace WebResourceProxy
{
    public class LocalPluginContext
    {
        internal IServiceProvider ServiceProvider { get; private set; }

        internal IOrganizationService OrganizationService { get; private set; }

        internal IPluginExecutionContext PluginExecutionContext { get; private set; }

        internal ITracingService TracingService { get; private set; }

        internal LocalPluginContext(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            ServiceProvider = serviceProvider;

            PluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            OrganizationService = factory.CreateOrganizationService(PluginExecutionContext.UserId);
        }

        internal void Trace(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || TracingService == null)
            {
                return;
            }

            if (PluginExecutionContext == null)
            {
                TracingService.Trace(message);
            }
            else
            {
                TracingService.Trace("{0}, Correlation Id: {1}, Initiating User: {2}", message,
                    PluginExecutionContext.CorrelationId, PluginExecutionContext.InitiatingUserId);
            }
        }
    }
}
