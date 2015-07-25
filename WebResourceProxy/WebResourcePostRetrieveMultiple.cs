using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Net;
using System.IO;

namespace WebResourceProxy
{
    public class WebResourcePostRetrieveMultiple : Plugin
    {
        public WebResourcePostRetrieveMultiple()
            : base(typeof(WebResourcePostRetrieveMultiple))
        {
            RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(40, "RetrieveMultiple", "webresource", ExecuteWebResourcePostRetrieveMultiple));
        }

        public void ExecuteWebResourcePostRetrieveMultiple(LocalPluginContext context)
        {
            var query = (QueryBase)context.PluginExecutionContext.InputParameters["Query"] as QueryExpression;
            if (query != null)
            {
                var entities = context.PluginExecutionContext.OutputParameters["BusinessEntityCollection"] as EntityCollection;
                foreach(var entity in entities.Entities){
                    var description = entity.GetAttributeValue<string>("description");
                    if (description != null && description.StartsWith("proxy:"))
                    {
                        ReplaceContent(entity, description);
                    }
                }
            }
        }

        private void ReplaceContent(Entity entity, string proxyPath)
        {
            var request = (HttpWebRequest)WebRequest.Create(proxyPath.Substring(6));
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (var stream = response.GetResponseStream())
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    entity["content"] = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }
    }
}
