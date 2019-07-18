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
using Newtonsoft.Json.Serialization;

namespace Sale.Servicios
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register<JsonSerializer, CustomJsonSerializer>();
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

                    if (usuario.id > 0)
                    {
                        return new ClaimsPrincipal(new UserIdentity
                        {
                            IsAuthenticated = true,
                            Name = usuario.email,
                            AuthenticationType = "stateless",
                            Usuario = usuario
                        });
                    }
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

    public class UserIdentity : IIdentity
    {
        public string Name { get; set; }

        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public Usuario Usuario { get; set; }
    }

    public class CustomJsonSerializer : JsonSerializer
    {
        public CustomJsonSerializer()
        {
            this.ContractResolver = new CamelCasePropertyNamesContractResolver();
            this.Formatting = Formatting.Indented;
        }
    }
}