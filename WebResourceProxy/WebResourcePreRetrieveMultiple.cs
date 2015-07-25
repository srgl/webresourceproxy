using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebResourceProxy
{
    public class WebResourcePreRetrieveMultiple: Plugin
    {
        public WebResourcePreRetrieveMultiple()
            : base(typeof(WebResourcePreRetrieveMultiple))
        {
            RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(20, "RetrieveMultiple", "webresource", ExecuteWebResourcePreRetrieveMultiple));
        }

        public void ExecuteWebResourcePreRetrieveMultiple(LocalPluginContext context)
        {
            var query = context.PluginExecutionContext.InputParameters["Query"] as QueryExpression;
            if (query != null && !query.ColumnSet.AllColumns && 
                !query.ColumnSet.Columns.Contains("description"))
            {
                query.ColumnSet.Columns.Add("description");
            }
        }
    }
}
