using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Nancy.ModelBinding;
using System.Globalization;
using Nancy.Security;
using Nancy.Conventions;
using Sale.Base.Data;
using System.Security.Claims;
using System.Security.Principal;
using Sale.Servicios.AppMobile.Modelos;

namespace Sale.Servicios
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            
        }
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            TinyIoCContainer.Current.Register(new SqlServerConnection(), "sqlConn");
        }

        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines, NancyContext context)
        {
            var configuration =
                new StatelessAuthenticationConfiguration(nancyContext =>
                {
                    var c = TinyIoCContainer.Current;
                    var conn = c.Resolve<SqlServerConnection>("sqlConn");

                    var token = nancyContext.Request.Headers.Authorization;
                    var usuario = conn.GetObject<Usuario>("sp_usuarios_valid", new List<SqlParameter> {
                        new SqlParameter{ ParameterName = "@token", Value = token}
                    });

                    if (usuario.id  > 0)
                    {
                        return new ClaimsPrincipal(new UserIdentity {
                            IsAuthenticated = true,
                            Name = usuario.email,
                            AuthenticationType = "stateless",
                            Usuario = usuario
                        });
                    }
                    //conn.Close();
                    return null;
                });

            AllowAccessToConsumingSite(pipelines);

            StatelessAuthentication.Enable(pipelines, configuration);
        }


        static void AllowAccessToConsumingSite(IPipelines pipelines)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline(x =>
            {
                x.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                x.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,DELETE,PUT,OPTIONS");
            });
        }
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("/Excel")
            );
        }

    }
    public class NancyNumericConverter : ITypeConverter
    {
        public bool CanConvertTo(Type destinationType, BindingContext context)
        {
            return IsNumericType(destinationType);
        }

        public object Convert(string input, Type destinationType, BindingContext context)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            return System.Convert.ChangeType(input, destinationType, new CultureInfo("es-AR"));
        }

        private bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }

    public class UserIdentity : IIdentity
    {
        public string Name { get; set; }

        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public Usuario Usuario { get; set; }
    }

    public class PostEntityParams
    {
        public string Entity { get; set; }
    }
    public class PostFilterParams
    {
        public string Filter { get; set; }
    }
    public static class FirstJsonConvert {
        public static Tout DeserializeEntity<Tout>(NancyModule module)
        {
            var x = module.Bind<PostEntityParams>();
            var type = typeof(PostEntityParams);
            var obj = (string)type.GetProperty("Entity").GetValue(x);
            return JsonConvert.DeserializeObject<Tout>(obj, new JsonSerializerSettings { Culture = new System.Globalization.CultureInfo("es-AR") });

        }
        public static Tout DeserializeFilter<Tout>(NancyModule module)
        {
            var x = module.Bind<PostEntityParams>();
            var type = typeof(PostFilterParams);
            var obj = (string)type.GetProperty("Filter").GetValue(x);
            return JsonConvert.DeserializeObject<Tout>(obj, new JsonSerializerSettings { Culture = new System.Globalization.CultureInfo("es-AR") });

        }
        public static Tout Deserialize<Tin, Tout>(string propertyName, NancyModule module)
        {
            var x = module.Bind<PostEntityParams>();
            var type = typeof(Tin);
            var obj = (string)type.GetProperty(propertyName).GetValue(x);
            return JsonConvert.DeserializeObject<Tout>(obj, new JsonSerializerSettings { Culture = new System.Globalization.CultureInfo("es-AR") });

        }
    }
}