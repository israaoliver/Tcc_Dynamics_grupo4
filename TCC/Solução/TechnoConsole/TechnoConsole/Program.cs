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
            // Busca as contas no Dynamics 1 na tabela contas
            Contas getaccounts = new Contas(connectDynamics1);
            EntityCollection contasCrm = getaccounts.GetLista();

            //Criando novas contas no Dynamics 2
            Contas createAccounts = new Contas(connectDynamics2);
            createAccounts.CreateDataTable(contasCrm);

            //===================Contatos======================//
            //Buscar os Contatos no Dynamics 1 na tabela contatos
            Contatos getContacts = new Contatos(connectDynamics1);
            EntityCollection contatosCRM = getContacts.GetLista();

            //Criando novos contatos no Dynamics 2
            Contatos createContacs = new Contatos(connectDynamics2);
            createContacs.CreateDataTable(contatosCRM);

            //========================Concorrentes===========================//
            // Busca concorrentes no Dynamics 1 na tabela concorrentes
            Concorrentes getCompetitor = new Concorrentes(connectDynamics1);
            EntityCollection concorrentesCRM = getCompetitor.GetLista();

            //Criando novos concorrentes no Dynamics 2
            Concorrentes createCompetitor = new Concorrentes(connectDynamics2);
            createCompetitor.CreateDataTable(concorrentesCRM);

            //===========================Fatura================================//
            // Busca faturas no Dynamics 1 na tabela faturas
            Fatura getInvoice = new Fatura(connectDynamics1);
            EntityCollection faturasCRM = getInvoice.GetLista();

            //Criando novas faturas no Dynamics 2
            Fatura createInvoice = new Fatura(connectDynamics2);
            createInvoice.CreateDataTable(faturasCRM);


            Console.WriteLine("Carga de Dados Finalizado!");
            Console.ReadLine();
        }

    }
}

