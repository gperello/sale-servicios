using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Modelos
{
    public class Comercio
    {
        public int id { get; set; }
	    public string address { get; set; }
        public string city { get; set; }
        public string title { get; set; }
        public string hours { get; set; }
        public string phone { get; set; }
        public decimal lng { get; set; }
        public decimal lat { get; set; }
        public string picture { get; set; }
        public string thumbnail { get; set; }
        public string[] images { get; set; }
        public string tags { get; set; }
        public string description { get; set; }
        public string label { get; set; }
        public decimal rating { get; set; }
        public Comentario[] reviews { get; set; }
    }

    public class Comentario {
        public int id { get; set; }
        public string username { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public int rating { get; set; }
    }
}