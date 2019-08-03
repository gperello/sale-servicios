using Sale.Base.Servicios.Classes;
using Sale.Servicios.AppMobile.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Negocio
{
    public class MarcarFavoritos : ServiceBase
    {
        public override void ExecuteService(dynamic x)
        {
            Connection.Execute("sp_favoritos_ups", new List<SqlParameter> {
                new SqlParameter{ ParameterName = "@usuid", Value = (int)x.usuid },
                new SqlParameter{ ParameterName = "@entid", Value = (int)x.entid },
                new SqlParameter{ ParameterName = "@tipid", Value = (int)x.tipid },
                new SqlParameter{ ParameterName = "@ok", Value = (bool)x.ok },
                
            });
            Response.SetResult(true);
        }
    }
}