using Sale.Base.Data;
using Sale.Base.Servicios.Classes;
using Nancy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Base.Servicios.Interfaces
{
    public interface IService<TObject> : IService
    {
        TObject Request { get; set; }
    }

    public interface IService
    {
        CultureInfo Culture { get; }
        NancyContext Context { get; set; }
        ServiceResponse Response { get; set; }
        SqlServerConnection Connection { get; set; }
        dynamic QueryString { get; set; }
        void ExecuteService(dynamic x);
        ServiceResponse ExecuteInternalService<TService>(dynamic obj, SqlServerConnection conn)
        where TService : IService, new();
        ServiceResponse ExecuteInternalService<TService, TObject>(TObject obj, SqlServerConnection conn)
        where TService : IService<TObject>, new();
    }
}
