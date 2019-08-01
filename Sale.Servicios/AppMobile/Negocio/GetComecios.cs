using Sale.Base.Servicios.Classes;
using Sale.Servicios.AppMobile.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Negocio
{
    public class GetComercios : ServiceBase<ComercioRequest>
    {
        public override void ExecuteService(dynamic x)
        {
            var comercios = Connection.GetArray<Comercio>(Request);

            foreach (var item in comercios)
            {
                item.images = Connection.GetArray<Imagen>("sp_imagenes_get", new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@emp_id", Value = item.id }
                }).ToArray();
            }
            foreach (var item in comercios)
            {
                item.reviews = Connection.GetArray<Comentario>("sp_comentarios_get", new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@emp_id", Value = item.id }
                }).ToArray();
            }

            Response.SetResult(new { list = comercios });
        }
    }
}