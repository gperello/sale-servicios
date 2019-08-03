using Sale.Base.Servicios.Classes;
using Sale.Servicios.AppMobile.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Negocio
{
    public class GetMensajes : ServiceBase
    {
        public override void ExecuteService(dynamic x)
        {
            var list = Connection.GetArray<Categoria>("sp_mensajes_get", new List<SqlParameter> {
                new SqlParameter{ ParameterName = "@usuid", Value = (int)x.usuid },
                new SqlParameter{ ParameterName = "@ciuid", Value = (int)x.ciuid },
            });
            Response.SetResult(new { mensajes = list });
        }
    }
}