using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnoConsole.Interfaces;

namespace TechnoConsole.Models
{
    public class Contas : IPadraoDeMetodos
    {
        public string TableName = "account";

        public IOrganizationService Service { get; set; }

        public Contas(IOrganizationService service)
        {
            this.Service = service;
        }

        public EntityCollection GetLista()
        {
            QueryExpression queryAccount = new QueryExpression(this.TableName);
            queryAccount.ColumnSet.AddColumns
                ("name",
                "accountid",
                "telephone1",
                "tc4_niveldecliente",
                "tc4_porte",
                "fax",
                "websiteurl",
                "parentaccountid",
                "tickersymbol",
                "customertypecode",
                "defaultpricelevelid",
                "address1_line1"
                );

            return this.Service.RetrieveMultiple(queryAccount);
        }

        public void CreateDataTable(EntityCollection dataTable)
        {
            Entity account = new Entity(this.TableName);

            foreach (Entity conta in dataTable.Entities)
            {

                account["name"] = conta["name"].ToString();

                account["accountid"] = Guid.NewGuid();

                string telephoneDaConta = conta.Contains("telephone1") ? (conta["telephone1"]).ToString() : string.Empty;
                account["telephone1"] = telephoneDaConta;

                OptionSetValue nivel = conta.Contains("tc4_niveldecliente") ? (OptionSetValue)conta["tc4_niveldecliente"] : null;
                account["tc4_niveldecliente"] = nivel;

                OptionSetValue porte = conta.Contains("tc4_porte") ? (OptionSetValue)conta["tc4_porte"] : null;
                account["tc4_porte"] = porte;

                string fax = conta.Contains("fax") ? (conta["fax"]).ToString() : string.Empty;
                account["fax"] = fax;

                string site = conta.Contains("websiteurl") ? (conta["websiteurl"]).ToString() : string.Empty;
                account["websiteurl"] = site;

                EntityReference parentid = conta.Contains("parentaccountid") ? (EntityReference)conta["parentaccountid"] : null;
                account["parentaccountid"] = parentid;

                string simbolo = conta.Contains("tickersymbol") ? (conta["tickersymbol"]).ToString() : string.Empty;
                account["tickersymbol"] = simbolo;


                OptionSetValue tipoDeRelacao = conta.Contains("customertypecode") ? (OptionSetValue)conta["customertypecode"] : null;
                account["customertypecode"] = tipoDeRelacao;

                EntityReference listaPrecos = conta.Contains("defaultpricelevelid") ? (EntityReference)conta["defaultpricelevelid"] : null;
                account["defaultpricelevelid"] = listaPrecos;

                string endereco = conta.Contains("address1_line1") ? (conta["address1_line1"]).ToString() : string.Empty;
                account["address1_line1"] = endereco;

                Service.Create(account);
            }
        }
    }
}
