using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnoPlugins2.Insfrastructure;

namespace TechnoPlugins2
{
    public class OpportunityCustom : PluginImplement
    {
        public override void ExecutePlugin(IServiceProvider serviceProvider)
        {
            Entity opportunity = new Entity();

            if (this.Context.MessageName == "Create" || this.Context.MessageName == "Update")
            {
                opportunity = (Entity)this.Context.InputParameters["Target"];
            }
            else
            {
                opportunity = (Entity)this.Context.PreEntityImages["PreImage"];
            }

            IOrganizationService dynamics2 = ConnectionFactory.GetCrmService();


            if (this.Context.MessageName == "Create")
            {
                CreateOpportunity(dynamics2, opportunity);

            }

            if (this.Context.MessageName == "Update")
            {
                UpdateOpportunity(dynamics2, opportunity);

            }
            if (this.Context.MessageName == "Delete")
            {
                DeleteOpportunity(dynamics2, opportunity);

            }
        }

        private void CreateOpportunity(IOrganizationService dynamics2, Entity oportunidade)
        {
            Entity novaopp = new Entity("opportunity");

            Guid idOpp = (Guid)oportunidade["opportunityid"];
            novaopp["opportunityid"] = idOpp;

            novaopp["name"] = oportunidade["name"].ToString();

            EntityReference contato = oportunidade.Contains("parentcontactid") ? (EntityReference)oportunidade["parentcontactid"] : null;
            novaopp["parentcontactid"] = contato;

            novaopp["parentaccountid"] = VerificandoConta(dynamics2, oportunidade);

            OptionSetValue periodoTempo = oportunidade.Contains("purchasetimeframe") ? (OptionSetValue)oportunidade["purchasetimeframe"] : null;
            novaopp["purchasetimeframe"] = periodoTempo;

            Money valorDescontonaFatura = oportunidade.Contains("budgetamount") ? (Money)oportunidade["budgetamount"] : null;
            novaopp["budgetamount"] = valorDescontonaFatura;

            OptionSetValue processoCompra = oportunidade.Contains("purchaseprocess") ? (OptionSetValue)oportunidade["purchaseprocess"] : null;
            novaopp["purchaseprocess"] = processoCompra;

            string descricao = oportunidade.Contains("description") ? oportunidade["description"].ToString() : string.Empty;
            novaopp["description"] = descricao;

            OptionSetValue categoriaPrevisao = oportunidade.Contains("msdyn_forecastcategory") ? (OptionSetValue)oportunidade["msdyn_forecastcategory"] : null;
            novaopp["msdyn_forecastcategory"] = categoriaPrevisao;

            string situacaoAtual = oportunidade.Contains("currentsituation") ? oportunidade["currentsituation"].ToString() : string.Empty;
            novaopp["currentsituation"] = situacaoAtual;

            string neceddidadeCliente = oportunidade.Contains("customerneed") ? oportunidade["customerneed"].ToString() : string.Empty;
            novaopp["customerneed"] = neceddidadeCliente;

            string solucaoProposta = oportunidade.Contains("proposedsolution") ? oportunidade["proposedsolution"].ToString() : string.Empty;
            novaopp["proposedsolution"] = solucaoProposta;

            dynamics2.Create(novaopp);

        }

        private void UpdateOpportunity(IOrganizationService dynamics2, Entity oportunidade)
        {
            Entity novaopp = new Entity("opportunity");

            Guid idOpp = (Guid)oportunidade["opportunityid"];
            novaopp["opportunityid"] = idOpp;

            novaopp["name"] = oportunidade["name"].ToString();

            EntityReference contato = oportunidade.Contains("parentcontactid") ? (EntityReference)oportunidade["parentcontactid"] : null;
            novaopp["parentcontactid"] = contato;

            novaopp["parentaccountid"] = VerificandoConta(dynamics2, oportunidade);

            OptionSetValue periodoTempo = oportunidade.Contains("purchasetimeframe") ? (OptionSetValue)oportunidade["purchasetimeframe"] : null;
            novaopp["purchasetimeframe"] = periodoTempo;

            Money valorDescontonaFatura = oportunidade.Contains("budgetamount") ? (Money)oportunidade["budgetamount"] : null;
            novaopp["budgetamount"] = valorDescontonaFatura;

            OptionSetValue processoCompra = oportunidade.Contains("purchaseprocess") ? (OptionSetValue)oportunidade["purchaseprocess"] : null;
            novaopp["purchaseprocess"] = processoCompra;

            string descricao = oportunidade.Contains("description") ? oportunidade["description"].ToString() : string.Empty;
            novaopp["description"] = descricao;

            OptionSetValue categoriaPrevisao = oportunidade.Contains("msdyn_forecastcategory") ? (OptionSetValue)oportunidade["msdyn_forecastcategory"] : null;
            novaopp["msdyn_forecastcategory"] = categoriaPrevisao;

            string situacaoAtual = oportunidade.Contains("currentsituation") ? oportunidade["currentsituation"].ToString() : string.Empty;
            novaopp["currentsituation"] = situacaoAtual;

            string neceddidadeCliente = oportunidade.Contains("customerneed") ? oportunidade["customerneed"].ToString() : string.Empty;
            novaopp["customerneed"] = neceddidadeCliente;

            string solucaoProposta = oportunidade.Contains("proposedsolution") ? oportunidade["proposedsolution"].ToString() : string.Empty;
            novaopp["proposedsolution"] = solucaoProposta;

            dynamics2.Update(oportunidade);
        }

        private void DeleteOpportunity(IOrganizationService dynamics2, Entity preDeleteImage)
        {
            string opportunityId = preDeleteImage["opportunityid"].ToString();

            EntityCollection opportunitytId = RetrieveOpportunity(opportunityId, dynamics2);

            foreach (Entity opportunity in opportunitytId.Entities)
            {
                Guid oppId = (Guid)opportunity["opportunityid"];
                dynamics2.Delete("opportunity", oppId);
            }
        }

        private EntityCollection RetrieveOpportunity(string opportunityId, IOrganizationService dynamics2)
        {
            QueryExpression queryRetrieveOpportunity = new QueryExpression("opportunity");
            queryRetrieveOpportunity.ColumnSet.AddColumns("tc4_numero", "opportunityid");
            queryRetrieveOpportunity.Criteria.AddCondition("opportunityid", ConditionOperator.Equal, opportunityId);


            return dynamics2.RetrieveMultiple(queryRetrieveOpportunity);
        }

        public EntityReference VerificandoConta(IOrganizationService dynamics2, Entity opportunity)
        {

            EntityReference contaId = opportunity.Contains("parentaccountid") ? (EntityReference)opportunity["parentaccountid"] : null;

            if (contaId == null)
            {
                return contaId;
            }
            
            Entity conta1 = Service.Retrieve("account", (Guid)contaId.Id, new ColumnSet("tc4_cnpj"));


            string cnpj = conta1.Contains("tc4_cnpj") ? conta1["tc4_cnpj"].ToString() : string.Empty;

            if (cnpj != string.Empty)
            {
                QueryExpression queryConta2 = new QueryExpression("account");
                queryConta2.ColumnSet.AddColumns("accountid", "tc4_cnpj");
                queryConta2.Criteria.AddCondition("tc4_cnpj", ConditionOperator.Equal, cnpj);

                EntityCollection contas = dynamics2.RetrieveMultiple(queryConta2);

                if (contas != null)
                {
                    foreach (Entity account in contas.Entities)
                    {
                        Guid idContaDynamics2 = (Guid)account["accountid"];

                        contaId.Id = idContaDynamics2;

                    }
                }

            }
            else
            {
                contaId = null;
            }


            return contaId;


        }

        public bool VerificandoExistencia(IOrganizationService dynamics2, Guid idDaConta)
        {
            Entity conta1 = dynamics2.Retrieve("account", idDaConta, new ColumnSet("accountid"));

            if(conta1 != null)
            {
                return false;
            }

            return true;
        }
    }
}
