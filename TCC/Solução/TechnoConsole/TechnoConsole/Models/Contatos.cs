using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnoConsole.ConnectionsFactory;
using TechnoConsole.Interfaces;

namespace TechnoConsole.Models
{
    public class Contatos : IPadraoDeMetodos
    {
        public string TableName = "contact";
        public IOrganizationService Service { get; set; }

        public IOrganizationService connectDynamics2 = ConnectionDynamics2.GetCrmService();

        public Contatos(IOrganizationService service)
        {
            this.Service = service;
        }



      
        public EntityCollection GetLista()
        {

            QueryExpression queryContats = new QueryExpression(this.TableName);
            queryContats.ColumnSet.AddColumns
                ("firstname",
                "lastname",
                "fullname",
                "telephone1",
                "jobtitle",
                "parentcustomerid",
                "emailaddress1",
                "telephone1",
                "mobilephone",
                "fax",
                "preferredcontactmethodcode",
                "address1_line1",
                "address1_city",
                "address1_stateorprovince",
                "address1_postalcode",
                "address1_country"
                );

            return this.Service.RetrieveMultiple(queryContats);
        }

        public void CreateDataTable(EntityCollection dataTable)
        {
            Entity newContatc = new Entity(this.TableName);

            Contas getaccounts = new Contas(connectDynamics2);
            EntityCollection accounts = getaccounts.GetLista();

            foreach (Entity contato in dataTable.Entities)
            {
                string contatoName = contato["firstname"].ToString();
                newContatc["firstname"] = contatoName;

                newContatc["lastname"] = contato["lastname"].ToString();

                string telephoneDoContato = contato.Contains("telephone1") ? (contato["telephone1"]).ToString() : string.Empty;
                newContatc["telephone1"] = telephoneDoContato;

                string cargo = contato.Contains("jobtitle") ? (contato["jobtitle"]).ToString() : string.Empty;
                newContatc["jobtitle"] = cargo;

                EntityReference parentid = contato.Contains("parentcustomerid") ? (EntityReference)contato["parentcustomerid"] : null;
                if(parentid != null)
                {
                    foreach (Entity account in accounts.Entities)
                    {
                        string accounttName = account["name"].ToString();
                        if (parentid.Name == accounttName)
                        {
                            string idDaConta = account["accountid"].ToString();
                            Guid id = new Guid(idDaConta);
                            parentid.Id = id;
                            newContatc["parentcustomerid"] = parentid;
                            break;
                        }
                        else
                        { newContatc["parentcustomerid"] = null; }

                    }
                }
                else
                {
                    newContatc["parentcustomerid"] = parentid;
                }

               

                string email = contato.Contains("emailaddress1") ? (contato["emailaddress1"]).ToString() : string.Empty;
                newContatc["emailaddress1"] = email;

                string telephone = contato.Contains("telephone1") ? (contato["telephone1"]).ToString() : string.Empty;
                newContatc["telephone1"] = telephone;

                string celular = contato.Contains("mobilephone") ? (contato["mobilephone"]).ToString() : string.Empty;
                newContatc["mobilephone"] = celular;

                string fax = contato.Contains("fax") ? (contato["fax"]).ToString() : string.Empty;
                newContatc["fax"] = fax;

                OptionSetValue contatoPreferencial = contato.Contains("preferredcontactmethodcode") ? (OptionSetValue)contato["preferredcontactmethodcode"] : null;
                newContatc["preferredcontactmethodcode"] = contatoPreferencial;

                string endereco = contato.Contains("address1_line1") ? (contato["address1_line1"]).ToString() : string.Empty;
                newContatc["address1_line1"] = endereco;

                string cidade = contato.Contains("address1_city") ? (contato["address1_city"]).ToString() : string.Empty;
                newContatc["address1_city"] = cidade;

                string estado = contato.Contains("address1_stateorprovince") ? (contato["address1_stateorprovince"]).ToString() : string.Empty;
                newContatc["address1_stateorprovince"] = estado;

                string cep = contato.Contains("address1_postalcode") ? (contato["address1_postalcode"]).ToString() : string.Empty;
                newContatc["address1_postalcode"] = cep;

                string pais = contato.Contains("address1_country") ? (contato["address1_country"]).ToString() : string.Empty;
                newContatc["address1_country"] = pais;

                Service.Create(newContatc);
            }
        }
    }
}
