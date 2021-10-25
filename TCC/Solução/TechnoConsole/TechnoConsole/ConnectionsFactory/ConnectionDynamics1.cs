using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoConsole.ConnectionsFactory
{
    class ConnectionDynamics1
    {
        public static IOrganizationService GetCrmService()
        {
            string connectionString =
                "AuthType=OAuth;" +
                "Username=dyn@techno1marketplace.onmicrosoft.com;" +
                "Password=Fyi@2021;" +
                "Url=https://org0045920f.crm2.dynamics.com/;" +
                "AppId=644535b3-cf67-4d65-aae4-39c82a105fef;" +
                "RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;";

            CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);
            return crmServiceClient.OrganizationWebProxyClient;
        }
    }
}
