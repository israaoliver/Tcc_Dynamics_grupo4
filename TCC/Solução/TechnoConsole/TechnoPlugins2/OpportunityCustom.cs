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
          
            dynamics2.Create(oportunidade);

        }

        private void UpdateOpportunity(IOrganizationService dynamics2, Entity oportunidade)
        {

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

    }
}
