using Sale.Base.Servicios.Classes;
using Sale.Servicios.AppMobile.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Negocio
{
    public class SaveComentario : ServiceBase<Comentario>
    {
        public override void ExecuteService(dynamic x)
        {
            Connection.SaveObject(Request);
            Response.SetResult(true);
        }
    }
}