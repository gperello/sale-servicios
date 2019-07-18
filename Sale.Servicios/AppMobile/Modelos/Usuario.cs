using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Modelos
{
    public class Usuario
    {
        public int id { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public int city { get; set; }
        public string token { get; set; }

    }
}