using Sale.Base.Servicios.Classes;
using Sale.Servicios.AppMobile.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Negocio
{
    public class GetUsuario : ServiceBase
    {
        public override void ExecuteService(dynamic x)
        {
            var usuario = Connection.GetObject<Usuario>("sp_usuarios_get", new List<SqlParameter> {
                new SqlParameter{ ParameterName = "@email", Value = (string)x.email }
            });
            if (usuario.id > 0) Response.SetResult(new { User = usuario });
            else Response.SetValidationError("Usuario no habilitado.");
        }
    }
}