using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoConsole.Interfaces
{
    interface IPadraoDeMetodos
    {
        EntityCollection GetLista();

        void CreateDataTable(EntityCollection dataTable);
    }
}
