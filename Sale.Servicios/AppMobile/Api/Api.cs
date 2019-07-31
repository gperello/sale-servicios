using Sale.Base.Servicios.Classes;
using Sale.Servicios.AppMobile.Modelos;
using Sale.Servicios.AppMobile.Negocio;

namespace Sale.Servicios.Api
{
    public class AuthenticateModule : ModuleBase
    {
        public AuthenticateModule()
        {
            Get("usuarioget/{email}", ExecuteService<GetUsuario>());
            Post("usuariosave", ExecuteService<SaveUsuario, Usuario>());
            Post("getcomercios", ExecuteService<GetComercios, ComercioRequest>());
        }

    }
}