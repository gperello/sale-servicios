using Sale.Base.Servicios.Classes;
using Sale.Servicios.AppMobile.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Negocio
{
    public class SaveUsuario : ServiceBase<Usuario>
    {
        public override void ExecuteService(dynamic x)
        {
            var usuario = Connection.GetObject<Usuario>("sp_usuarios_save", new List<SqlParameter> {
                new SqlParameter{ ParameterName = "@email", Value = Request.email },
                new SqlParameter{ ParameterName = "@fullname", Value = Request.fullname },
                new SqlParameter{ ParameterName = "@address", Value = Request.address },
                new SqlParameter{ ParameterName = "@phone", Value = Request.phone },
                new SqlParameter{ ParameterName = "@city", Value = Request.city }
            });
            if (usuario.id > 0) Response.SetResult(new { User = usuario });
            else Response.SetValidationError("Usuario no habilitado.");
        }
    }
}