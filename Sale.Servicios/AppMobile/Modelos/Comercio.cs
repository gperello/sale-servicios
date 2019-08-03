using Sale.Base.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Modelos
{
    [Entity(SpListName = "sp_empresas_lst")]
    public class Comercio
    {
        public int id { get; set; }
	    public string address { get; set; }
        public string city { get; set; }
        public string title { get; set; }
        public string hours { get; set; }
        public string phone { get; set; }
        public double lng { get; set; }
        public double lat { get; set; }
        public string picture { get; set; }
        public string thumbnail { get; set; }
        public Imagen[] images { get; set; }
        public string tags { get; set; }
        public string description { get; set; }
        public string label { get; set; }
        public string category { get; set; }
        public string distance { get; set; }
        public double rating { get; set; }
        public Comentario[] reviews { get; set; }
    }

    public class Comentario
    {
        public int id { get; set; }
        public string username { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string date { get; set; }
        public int rating { get; set; }
    }
    public class Imagen
    {
        public string id { get; set; }
        public string url { get; set; }
    }

    public class ComercioRequest {
        [Param(ParamName = "@str_posicion")]
        public string posicion { get; set; }
        [Param(ParamName = "@usuid")]
        public int usuid { get; set; }
        [Param(ParamName = "@pagina")]
        public int pagina { get; set; }
    }
    public class DatosRequest
    {
        [Param(ParamName = "@str_posicion")]
        public string posicion { get; set; }
        [Param(ParamName = "@email")]
        public string email { get; set; }
        [Param(ParamName = "@pagina")]
        public int pagina { get; set; }
    }
}