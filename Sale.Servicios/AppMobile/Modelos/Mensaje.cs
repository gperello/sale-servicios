﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sale.Servicios.AppMobile.Modelos
{
    public class Mensaje
    {
        public int id { get; set; }
	    public bool read { get; set; }
        public string title { get; set; }
        public string comercio { get; set; }
        public DateTime date { get; set; }
        public string message { get; set; }
        public string producto { get; set; }
        public int proid { get; set; }
        public int comid { get; set; }
    }
}