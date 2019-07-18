using Sale.Base.Data;
using Sale.Base.Servicios.Interfaces;
using Nancy;
using System.Globalization;

namespace Sale.Base.Servicios.Classes
{
    public abstract class ServiceBase : IService
    {
        public SqlServerConnection Connection { get; set; }
        public dynamic QueryString { get; set; }
        public ServiceResponse Response { get; set; }
        public CultureInfo Culture { get { return new CultureInfo("es-AR"); } }
        public NancyContext Context { get; set; }

        public abstract void ExecuteService(dynamic x);
        public ServiceResponse ExecuteInternalService<TService>(dynamic obj, SqlServerConnection conn)
        where TService : IService, new()
        {
            var service = new TService();
            service.Response = new ServiceResponse();
            service.Connection = conn;
            service.ExecuteService(obj);
            return service.Response;
        }
        public ServiceResponse ExecuteInternalService<TService, TObject>(TObject obj, SqlServerConnection conn)
        where TService : IService<TObject>, new()
        {
            var service = new TService();
            service.Response = new ServiceResponse();
            service.Request = obj;
            service.Connection = conn;
            service.ExecuteService(null);
            return service.Response;
        }

    }
    public abstract class ServiceBase<TObject> : IService<TObject>
    {
        public SqlServerConnection Connection { get; set; }
        public ServiceResponse Response { get; set; }
        public abstract void ExecuteService(dynamic x);
        public TObject Request { get; set; }
        public dynamic QueryString { get; set; }
        public CultureInfo Culture { get { return new CultureInfo("es-AR"); } }

        public NancyContext Context { get; set; }

        public ServiceResponse ExecuteInternalService<TService>(dynamic obj, SqlServerConnection conn)
        where TService : IService, new()
        {
            var service = new TService();
            service.Response = new ServiceResponse();
            service.Connection = conn;
            service.ExecuteService(obj);
            return service.Response;
        }
        public ServiceResponse ExecuteInternalService<TService, TObject>(TObject obj, SqlServerConnection conn)
        where TService : IService<TObject>, new()
        {
            var service = new TService();
            service.Response = new ServiceResponse();
            service.Request = obj;
            service.Connection = conn;
            service.ExecuteService(null);
            return service.Response;
        }
        public ServiceResponse ExecuteInternalService<TService, TObject>(TObject obj, dynamic x, SqlServerConnection conn)
        where TService : IService<TObject>, new()
        {
            var service = new TService();
            service.Response = new ServiceResponse();
            service.Request = obj;
            service.Connection = conn;
            service.ExecuteService(x);
            return service.Response;
        }

    }
}