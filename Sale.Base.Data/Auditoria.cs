using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Base.Data
{
    public static class Auditoria
    {
        private static string GetDatos(object obj)
        {
            var json = new JObject();
            var tipo = obj.GetType();
            foreach (var pi in tipo.GetProperties())
            {
                var attr = (ParamAttribute)Attribute.GetCustomAttribute(pi, typeof(ParamAttribute));
                if (attr != null)
                    if (pi.GetValue(obj) != null) json.Add(pi.Name, JToken.Parse("'" + pi.GetValue(obj).ToString().Replace(":", "") + "'"));
                    else json.Add(pi.Name, JToken.Parse("''"));
            }
            return JsonConvert.SerializeObject(json);
        }

        public static void Save(SqlServerConnection conn, object obj, int usuarioId, int entidadId, string tipo) {
            var tableAttribute = (EntityAttribute)Attribute.GetCustomAttribute(obj.GetType(), typeof(EntityAttribute));

            var anterior = (int?)conn.GetValue("sp_Auditorias_getanterior", new List<System.Data.SqlClient.SqlParameter> {
                new System.Data.SqlClient.SqlParameter { ParameterName = "@p_Ent_Id", Value = entidadId},
                new System.Data.SqlClient.SqlParameter { ParameterName = "@p_Aud_Entity", Value = tableAttribute.CodigoAuditoria},
            });
            var auditoria = new AuditoriaEntity {
               AnteriorId = anterior,
               Datos = GetDatos(obj),
               EntidadCodigo = tableAttribute.CodigoAuditoria,
               EntidadId = entidadId,
               Fecha = DateTime.Now,
               Tipo = tipo,
               UsuarioId = usuarioId,
            };
            conn.SaveObject<AuditoriaEntity>(auditoria);
        }
        public static IEnumerable<AuditoriaList> GetList(SqlServerConnection conn, AuditoriaFilter filter)
        {
            return conn.GetArray<AuditoriaList>(filter);
        }
        public static AuditoriaCompare GetCompare(SqlServerConnection conn, int auditoriaId)
        {
            return conn.GetObject<AuditoriaCompare>(auditoriaId);
        }

    }

    [EntityAttribute(SpSaveName = "sp_Auditorias_insert")]
    public class AuditoriaEntity
    {
        public int AuditoriaId { get; set; }
        [ParamAttribute(ParamName = "@p_Aud_Fecha")]
        public DateTime Fecha { get; set; }
        [ParamAttribute(ParamName = "@p_Aud_Tipo")]
        public string Tipo { get; set; }
        [ParamAttribute(ParamName = "@p_Usu_Id")]
        public int UsuarioId { get; set; }
        [ParamAttribute(ParamName = "@p_Aud_Entity")]
        public string EntidadCodigo { get; set; }
        [ParamAttribute(ParamName = "@p_Ent_Id")]
        public int EntidadId { get; set; }
        [ParamAttribute(ParamName = "@p_Aud_Datos")]
        public string Datos { get; set; }
        [ParamAttribute(ParamName = "@p_Aud_Anterior")]
        public int? AnteriorId { get; set; }

    }
    public class AuditoriaFilter
    {
        [ParamAttribute(ParamName = "@p_Usu_Id")]
        public int? UsuarioId { get; set; }
        [ParamAttribute(ParamName = "@p_Ent_Id")]
        public int? EntidadId { get; set; }
        [ParamAttribute(ParamName = "@p_Aud_Entity")]
        public string EntidadCodigo { get; set; }

    }
    [EntityAttribute(SpListName = "sp_Auditorias_lst")]
    public class AuditoriaList
    {
        public int AuditoriaId { get; set; }
        public string Fecha { get; set; }
        public string Tipo { get; set; }
        public string Usuario { get; set; }
        public string EntidadCodigo { get; set; }
        public int EntidadId { get; set; }

    }
    [EntityAttribute(SpGetName = "sp_Auditorias_compare", IdGetParamName = "@p_Aud_Id")]
    public class AuditoriaCompare
    {
        public string Anterior { get; set; }
        public string Nueva { get; set; }

    }
}
