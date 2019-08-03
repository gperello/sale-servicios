using Sale.Servicios.AppMobile.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Modelos
{
    public class Orden
    {
        public string nro { get; set; }
        public string comercio { get; set; }
        public string fecha { get; set; }
        public string estado { get; set; }
        public int comid { get; set; }
        public decimal total { get; set; }
        public decimal delivery { get; set; }
        public int formapagoid { get; set; }
        public DetalleDeOrden[] order { get; set; }
    }

    public class DetalleDeOrden {
        public int id { get; set; }
        public string nro { get; set; }
        public string proid { get; set; }
        public string producto { get; set; }
        public int cantidad { get; set; }
        public decimal precio_unitario { get; set; }
        public decimal precio { get; set; }
    }
}