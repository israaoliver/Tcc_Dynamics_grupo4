using Microsoft.Xrm.Sdk;
using TechnoConsole.ConnectionsFactory;
using TechnoConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Conexão do para o primeiro dynamics
            IOrganizationService connectDynamics1 = ConnectionDynamics1.GetCrmService();
            // Conexão para o segundo dynamics
            IOrganizationService connectDynamics2 = ConnectionDynamics2.GetCrmService();

            //===================Contas======================//
            Contas getaccounts = new Contas(connectDynamics1);
            EntityCollection contasCrm = getaccounts.GetLista();

            Contas createAccounts = new Contas(connectDynamics2);
            createAccounts.CreateDataTable(contasCrm);

            //===================Contatos======================//
            Contatos getContacts = new Contatos(connectDynamics1);
            EntityCollection contatosCRM = getContacts.GetLista();

            Contatos createContacs = new Contatos(connectDynamics2);
            createContacs.CreateDataTable(contatosCRM);

            //========================Concorrentes===========================//
            Concorrentes getCompetitor = new Concorrentes(connectDynamics1);
            EntityCollection concorrentesCRM = getCompetitor.GetLista();

            Concorrentes createCompetitor = new Concorrentes(connectDynamics2);
            createCompetitor.CreateDataTable(concorrentesCRM);

            //===========================Fatura================================//
            Fatura getInvoice = new Fatura(connectDynamics1);
            EntityCollection faturasCRM = getInvoice.GetLista();

            Fatura createInvoice = new Fatura(connectDynamics2);
            createInvoice.CreateDataTable(faturasCRM);


            Console.WriteLine("Carga de Dados Finalizado!");
            Console.ReadLine();
        }

    }
}

