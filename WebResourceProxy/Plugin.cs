using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace WebResourceProxy
{
    public class Plugin : IPlugin
    {
        private Collection<Tuple<int, string, string, Action<LocalPluginContext>>> registeredEvents;

        protected Collection<Tuple<int, string, string, Action<LocalPluginContext>>> RegisteredEvents
        {
            get
            {
                if (registeredEvents == null)
                {
                    registeredEvents = new Collection<Tuple<int, string, string, Action<LocalPluginContext>>>();
                }

                return registeredEvents;
            }
        }

        protected string ChildClassName { get; private set; }

        internal Plugin(Type childClassName)
        {
            ChildClassName = childClassName.ToString();
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            LocalPluginContext localContext = new LocalPluginContext(serviceProvider);

            localContext.Trace(string.Format(CultureInfo.InvariantCulture, "Entered {0}.Execute()", ChildClassName));

            try
            {
                Action<LocalPluginContext> entityAction =
                    (from a in RegisteredEvents
                     where (
                     a.Item1 == localContext.PluginExecutionContext.Stage &&
                     a.Item2 == localContext.PluginExecutionContext.MessageName &&
                     (string.IsNullOrWhiteSpace(a.Item3) || a.Item3 == localContext.PluginExecutionContext.PrimaryEntityName))
                     select a.Item4).FirstOrDefault();

                if (entityAction != null)
                {
                    localContext.Trace(string.Format(
                        CultureInfo.InvariantCulture,
                        "{0} is firing for Entity: {1}, Message: {2}",
                        ChildClassName,
                        localContext.PluginExecutionContext.PrimaryEntityName,
                        localContext.PluginExecutionContext.MessageName));

                    entityAction.Invoke(localContext);
                }
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                localContext.Trace(string.Format(CultureInfo.InvariantCulture, "Exception: {0}", e));

                // Handle the exception.
                throw;
            }
            finally
            {
                localContext.Trace(string.Format(CultureInfo.InvariantCulture, "Exiting {0}.Execute()", ChildClassName));
            }
        }
    }
}