using Microsoft.Xrm.Sdk;
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
    public class Fatura : IPadraoDeMetodos
    {
        public string TableName = "invoice";
        public IOrganizationService Service { get; set; }

        public IOrganizationService connectDynamics2 = ConnectionDynamics2.GetCrmService();

        public Fatura(IOrganizationService service)
        {
            this.Service = service;
        }

        public EntityCollection GetLista()
        {
            QueryExpression queryAccount = new QueryExpression(this.TableName);
            queryAccount.ColumnSet.AddColumns
                ("invoicenumber",
                "name",
                "transactioncurrencyid",
                "ispricelocked",
                "pricelevelid",
                "datedelivered",
                "duedate",
                "shippingmethodcode",
                "paymenttermscode",
                "totallineitemamount",
                "discountpercentage",
                "discountamount",
                "freightamount",
                "opportunityid",
                "salesorderid",
                "description",
                "billto_line1",
                "customerid"
                );

            return this.Service.RetrieveMultiple(queryAccount);
        }

        public void CreateDataTable(EntityCollection dataTable)
        {

            Entity invoice = new Entity(this.TableName);

            Contatos getContacts = new Contatos(connectDynamics2);
            EntityCollection contatos = getContacts.GetLista();

            foreach (Entity fatura in dataTable.Entities)
            {
                invoice["invoicenumber"] = fatura["invoicenumber"].ToString();

                invoice["name"] = fatura["name"].ToString();

                EntityReference moeda = fatura.Contains("transactioncurrencyid") ? (EntityReference)fatura["transactioncurrencyid"] : null;
                invoice["transactioncurrencyid"] = moeda;

                invoice["ispricelocked"] = fatura["ispricelocked"];

                DateTime? nullDate = null;
                DateTime? dataEntrega = fatura.Contains("datedelivered") ? ((DateTime)fatura["datedelivered"]) : nullDate;
                invoice["datedelivered"] = dataEntrega;

                DateTime? dataConclusao = fatura.Contains("duedate") ? ((DateTime)fatura["duedate"]) : nullDate;
                invoice["duedate"] = dataConclusao;

                OptionSetValue formaTransporte = fatura.Contains("shippingmethodcode") ? (OptionSetValue)fatura["shippingmethodcode"] : null;
                invoice["shippingmethodcode"] = formaTransporte;

                OptionSetValue condicaoPagamento = fatura.Contains("paymenttermscode") ? (OptionSetValue)fatura["paymenttermscode"] : null;
                invoice["paymenttermscode"] = condicaoPagamento;

                var valorDetalhado = fatura.Contains("totallineitemamount") ? (fatura["totallineitemamount"]) : null;
                invoice["totallineitemamount"] = valorDetalhado;

                Decimal descontoFatura = fatura.Contains("discountpercentage") ? (Decimal)(fatura["discountpercentage"]) : 0;
                invoice["discountpercentage"] = descontoFatura;

                Money valorDescontonaFatura = fatura.Contains("discountamount") ? (Money)fatura["discountamount"] : null;
                invoice["discountamount"] = valorDescontonaFatura;

                Money valorFrete = fatura.Contains("freightamount") ? (Money)fatura["freightamount"] : null;
                invoice["freightamount"] = valorFrete;


                EntityReference opportunity = fatura.Contains("opportunityid") ? (EntityReference)fatura["opportunityid"] : null;
                invoice["opportunityid"] = opportunity;

                EntityReference contrato = fatura.Contains("salesorderid") ? (EntityReference)fatura["salesorderid"] : null;
                invoice["salesorderid"] = contrato;

                EntityReference client = fatura.Contains("customerid") ? (EntityReference)fatura["customerid"] : null;
                invoice["customerid"] = client;

                string enderecoCobranca = fatura.Contains("billto_line1") ? (fatura["billto_line1"]).ToString() : string.Empty;
                invoice["billto_line1"] = enderecoCobranca;

                EntityReference listadePreco = fatura.Contains("pricelevelid") ? (EntityReference)fatura["pricelevelid"] : null;
                invoice["pricelevelid"] = BuscarListaPreco(listadePreco);






                Service.Create(invoice);
            }
        }

        private EntityReference BuscarListaPreco(EntityReference fatura)
        {
            IOrganizationService d2 = ConnectionDynamics2.GetCrmService();

            QueryExpression queryListadePreco = new QueryExpression("pricelevel");
            queryListadePreco.ColumnSet.AddColumn("name");
            queryListadePreco.Criteria.AddCondition("name", ConditionOperator.Equal , fatura.Name);

            EntityCollection faturaBuscada = d2.RetrieveMultiple(queryListadePreco);

            if(faturaBuscada != null)
            {
                foreach (Entity lista in faturaBuscada.Entities)
                {
                    Guid idListadePreco = (Guid)lista["pricelevelid"];

                    fatura.Id = idListadePreco;

                }
            }
            else
            {
                fatura = null;
            }

            return fatura;
        }
    }
}
