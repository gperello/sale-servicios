using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Modelos
{
    public class Categoria
    {
        public int id { get; set; }
	    public string picture { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
    }
}