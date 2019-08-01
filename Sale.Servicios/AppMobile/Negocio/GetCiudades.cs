using Sale.Base.Servicios.Classes;
using Sale.Servicios.AppMobile.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Negocio
{
    public class GetCiudades : ServiceBase
    {
        public override void ExecuteService(dynamic x)
        {
            var list = Connection.GetArray<Ciudad>("sp_ciudades_lst");
            Response.SetResult(new { list = list });
        }
    }
}