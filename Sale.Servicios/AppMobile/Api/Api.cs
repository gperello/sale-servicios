using Sale.Base.Servicios.Classes;
using Sale.Servicios.AppMobile.Modelos;
using Sale.Servicios.AppMobile.Negocio;
using Nancy.Security;

namespace Sale.Servicios.Api
{
    public class AuthenticateModule : ModuleBase
    {
        public AuthenticateModule()
        {
            this.RequiresAuthentication();
            Get("usuarioget/{email}", ExecuteService<GetUsuario>());
            Post("usuariosave", ExecuteService<SaveUsuario, Usuario>());
            Get("getciudades", ExecuteService<GetCiudades>());
            Get("getcategorias", ExecuteService<GetCategorias>());
            Post("getdatos", ExecuteService<GetDatos, ComercioRequest>());
            Get("getmensajes/{usuid}/{ciuid}", ExecuteService<GetMensajes>());
            Get("marcarLeidos/{usuid}/{menid}", ExecuteService<MarcarLeidos>());
            Get("marcarFavoritos/{usuid}/{entid}/{tipid}/{ok}", ExecuteService<MarcarFavoritos>());
            Post("savecomentario", ExecuteService<SaveComentario, Comentario>());


        }

    }
}