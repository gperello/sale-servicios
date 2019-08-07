using Sale.Base.Data;
using Sale.Servicios.AppMobile.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Modelos
{
    [Entity(SpSaveName = "sp_ordenes_insert")]
    public class Orden
    {
        [Param(ParamName = "@nro")]
        public string nro { get; set; }
        public string comercio { get; set; }
        public string fecha { get; set; }
        public string estado { get; set; }
        [Param(ParamName = "@empid")]
        public int comid { get; set; }
        [Param(ParamName = "@usuid")]
        public int usuid { get; set; }
        [Param(ParamName = "@total")]
        public decimal total { get; set; }
        [Param(ParamName = "@delivery")]
        public decimal delivery { get; set; }
        [Param(ParamName = "@formepagoid")]
        public int formapagoid { get; set; }
        public string mpid { get; set; }
        public string token { get; set; }
        public DetalleDeOrden[] detalle { get; set; }
    }

    [Entity(SpSaveName = "sp_ordenes_detalle_insert")]
    public class DetalleDeOrden {
        public int id { get; set; }
        [Param(ParamName = "@nro")]
        public string nro { get; set; }
        [Param(ParamName = "@proid")]
        public string proid { get; set; }
        public string producto { get; set; }
        [Param(ParamName = "@cantidad")]
        public int cantidad { get; set; }
        [Param(ParamName = "@precio_unitario")]
        public decimal precio_unitario { get; set; }
        public decimal precio { get; set; }
    }
}