using Sale.Servicios.AppMobile.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Modelos
{
    public class Orden
    {
        public int id { get; set; }
        public string comercio { get; set; }
        public int comid { get; set; }
        public DetalleDeOrden[] order { get; set; }
        public decimal total { get; set; }
        public string onumber { get; set; }
    }

public class DetalleDeOrden {
    public int id { get; set; }
    public Producto order { get; set; }
    public int qtd { get; set; }
}
}