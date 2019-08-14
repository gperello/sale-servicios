using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Modelos
{
    public class Producto
    {
        public int id { get; set; }
        public int comid { get; set; }
        public int catid { get; set; }
        public bool favorite { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string picture { get; set; }
        public Imagen[] images { get; set; }
        public decimal price { get; set; }
        public int qtd { get; set; }
        public bool delivery { get; set; }
        public bool promocion { get; set; }
    }
}