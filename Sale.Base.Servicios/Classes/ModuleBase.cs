﻿using Sale.Base.Servicios.Interfaces;
using Nancy.ModelBinding;
using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Globalization;
using Nancy.TinyIoc;
using Sale.Base.Data;

namespace Sale.Base.Servicios.Classes
{
    public class ModuleBase : NancyModule
    {
        private CultureInfo Culture { get { return new CultureInfo("es-AR"); } }

        public Func<dynamic, dynamic> ExecuteService<TService>(string contentType = "")
            where TService : IService, new()
        {
            var service = new TService
            {
                Connection = TinyIoCContainer.Current.Resolve<SqlServerConnection>("sqlConn"),
            };
            service.Response = new ServiceResponse();
            return x => {
                try {
                    service.Context = this.Context;
                    service.QueryString = Request.Query;
                    service.ExecuteService(x);
                }
                catch (Exception ex) {
                    service.Response.SetStatus(Enums.StatusResult.GeneralError, ex.Message);
                }
                return GetResponse(service.Response, contentType);
            };
        }

        public Func<dynamic, dynamic> ExecuteService<TService, TObject>(string contentType = "", CultureInfo culture = null)
            where TService : IService<TObject>, new()
            where TObject : new()
        {
            var service = new TService();
            service.Response = new ServiceResponse();
            service.Connection = TinyIoCContainer.Current.Resolve<SqlServerConnection>("sqlConn");
            return x => {
                try {
                    service.Context = this.Context;
                    service.QueryString = Request.Query;
                    service.Request = Deserialize<TObject>(culture);
                    if (service.Response.Status == Enums.StatusResult.OK) service.ExecuteService(x);
                }
                catch (Exception ex) {
                    service.Response.SetStatus(Enums.StatusResult.GeneralError, ex.Message);
                }
                return GetResponse(service.Response, contentType);
            };
        }

        public Func<dynamic, dynamic> ExecuteService<TService, TObject>(CultureInfo culture)
            where TService : IService<TObject>, new()
            where TObject : new()
        {
            var service = new TService();
            service.Response = new ServiceResponse();
            service.Connection = TinyIoCContainer.Current.Resolve<SqlServerConnection>("sqlConn");
            return x => {
                try
                {
                    service.Context = this.Context;
                    service.QueryString = Request.Query;
                    service.Request = Deserialize<TObject>(culture);
                    if (service.Response.Status == Enums.StatusResult.OK) service.ExecuteService(x);
                }
                catch (Exception ex)
                {
                    service.Response.SetStatus(Enums.StatusResult.GeneralError, ex.Message);
                }
                return GetResponse(service.Response);
            };
        }

        public Tout Deserialize<Tout>(CultureInfo culture = null)
            where Tout : new()
        {
            if (culture == null) culture = Culture;
            var body = Request.Body;
            int length = (int)body.Length; // this is a dynamic variable
            byte[] data = new byte[length];
            body.Read(data, 0, length);
            var x = System.Text.Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<Tout>(x, new JsonSerializerSettings { Culture = culture });
        }

        private Response GetResponse(ServiceResponse result, string contentType = "") {
            if (!string.IsNullOrEmpty(contentType))
            {
                return Response.AsStream((byte[])result.Result, contentType);
            }
            else {
                return Response.AsJson(result);
            }
        }
    }
}