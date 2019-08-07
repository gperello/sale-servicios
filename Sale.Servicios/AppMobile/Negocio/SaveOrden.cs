using Sale.Base.Servicios.Classes;
using Sale.Servicios.AppMobile.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Negocio
{
    public class SaveOrden : ServiceBase<Orden>
    {
        public override void ExecuteService(dynamic x)
        {
            Connection.SaveObject(Request);
            foreach (var item in Request.detalle) {
                item.nro = Request.nro;
                Connection.SaveObject(item);
            }

            var list = Connection.GetArray<Orden>("", new List<SqlParameter> {
                new SqlParameter{ ParameterName = "@usuid", Value = Request.usuid }
            });
            foreach (var item in list) {
                item.detalle = Connection.GetArray<DetalleDeOrden>("", new List<SqlParameter> {
                    new SqlParameter{ ParameterName = "@nro", Value = item.nro }
                }).ToArray();
            }
            Response.SetResult(new { ordenes = list });
        }
    }
}