using Sale.Base.Servicios.Classes;
using Sale.Servicios.AppMobile.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Negocio
{
    public class GetDatos : ServiceBase<ComercioRequest>
    {
        public override void ExecuteService(dynamic x)
        {
            Request.posicion = "-31.434629 -64.446939";
            var comercios = Connection.GetArray<Comercio>(Request);

            foreach (var item in comercios)
            {
                item.images = Connection.GetArray<Imagen>("sp_imagenes_get", new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@ent_id", Value = item.id },
                    new SqlParameter{ ParameterName = "@tip_id", Value = 1 }
                }).ToArray();
            }
            foreach (var item in comercios)
            {
                item.reviews = Connection.GetArray<Comentario>("sp_comentarios_get", new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@emp_id", Value = item.id }
                }).ToArray();
            }

            var ciudades = Connection.GetArray<Ciudad>("sp_ciudades_lst");
            var categorias = Connection.GetArray<Categoria>("sp_categorias_lst");
            var productos = Connection.GetArray<Producto>("sp_productos_lst", new List<SqlParameter> {
                    new SqlParameter{ ParameterName = "@empids", Value = string.Join(",", comercios.Select(y=> y.id).ToArray()) },
                    new SqlParameter{ ParameterName = "@usuid", Value = Request.usuid }
            });
            foreach (var item in productos)
            {
                item.images = Connection.GetArray<Imagen>("sp_imagenes_get", new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@ent_id", Value = item.id },
                    new SqlParameter{ ParameterName = "@tip_id", Value = 2 }
                }).ToArray();
            }
            // consulta para cargar comercios favoritos

            var comfavoritos = Connection.GetArray<Comercio>("sp_empresas_favoritos", new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@usuid", Value = Request.usuid },
                    new SqlParameter{ ParameterName = "@str_posicion", Value = Request.posicion }
                });

            foreach (var item in comfavoritos)
            {
                item.images = Connection.GetArray<Imagen>("sp_imagenes_get", new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@ent_id", Value = item.id },
                    new SqlParameter{ ParameterName = "@tip_id", Value = 1 }
                }).ToArray();
            }
            foreach (var item in comfavoritos)
            {
                item.reviews = Connection.GetArray<Comentario>("sp_comentarios_get", new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@emp_id", Value = item.id }
                }).ToArray();
            }
            // constultar productos favoritos

            var prodfavoritos = Connection.GetArray<Producto>("sp_productos_favoritos", new List<SqlParameter> {
                    new SqlParameter{ ParameterName = "@usuid", Value = Request.usuid }
            });
            foreach (var item in prodfavoritos)
            {
                item.images = Connection.GetArray<Imagen>("sp_imagenes_get", new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@ent_id", Value = item.id },
                    new SqlParameter{ ParameterName = "@tip_id", Value = 2 }
                }).ToArray();
            }
            Response.SetResult(new
            {
                comercios = comercios,
                productos = productos,
                categorias = categorias,
                ciudades = ciudades,
                comfavoritos = comfavoritos,
                prodfavoritos = prodfavoritos,
            });

        }
    }
}
