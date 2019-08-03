using Sale.Base.Servicios.Classes;
using Sale.Servicios.AppMobile.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Negocio
{
    public class MarcarLeidos : ServiceBase
    {
        public override void ExecuteService(dynamic x)
        {
            Connection.Execute("sp_mensajes_marcarleido", new List<SqlParameter> {
                new SqlParameter{ ParameterName = "@usuid", Value = (int)x.usuid },
                new SqlParameter{ ParameterName = "@menid", Value = (int)x.menid },
            });
            Response.SetResult(true);
        }
    }
}