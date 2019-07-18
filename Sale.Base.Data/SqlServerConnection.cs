using System;
using System.Configuration;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Globalization;

namespace Sale.Base.Data
{
    public partial  class SqlServerConnection : IDisposable
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ServerConnection"].ConnectionString);

        private List<TResult> ReaderList<TResult>(SqlCommand comando) where TResult : new()
        {
            var resultado = new List<TResult>();
            SqlDataReader dr = null;
            try
            {
                dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    var item = new TResult();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var pi = typeof(TResult).GetProperty(dr.GetName(i));
                        if (pi != null && dr.GetValue(i) != System.DBNull.Value) pi.SetValue(item, dr.GetValue(i));
                    }
                    resultado.Add(item);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null) dr.Close();
                comando.Dispose();
            }
        }
        private TResult ReaderObject<TResult>(SqlCommand comando) where TResult : new()
        {
            var resultado = new TResult();
            SqlDataReader dr = null;
            try
            {
                dr = comando.ExecuteReader();
                if (dr.Read())
                {
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var pi = typeof(TResult).GetProperty(dr.GetName(i));
                        if (pi != null && dr.GetValue(i) != System.DBNull.Value) pi.SetValue(resultado, dr.GetValue(i));
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null) dr.Close();
                comando.Dispose();
            }
        }

        public SqlServerConnection()
        {
            conn.Open();
        }

        public List<TResult> GetArray<TResult>(string stringSql, List<SqlParameter> parameters = null)
            where TResult : new()
        {
            SqlCommand comando = new SqlCommand(stringSql, conn);
            comando.CommandType = CommandType.StoredProcedure;
            if (parameters != null) foreach (var par in parameters) comando.Parameters.Add(par);
            return ReaderList<TResult>(comando);
        }

        public List<TResult> GetPage<TResult>(string stringSql, out int totalCount, List<SqlParameter> parameters = null)
        where TResult : new()
        {
            SqlCommand comando = new SqlCommand(stringSql, conn);
            comando.CommandType = CommandType.StoredProcedure;

            if (parameters != null) foreach (var par in parameters) comando.Parameters.Add(par);
            comando.Parameters.Add(new SqlParameter { ParameterName = "@p_totalCount", Value = 0, Direction = ParameterDirection.InputOutput });

            var resultado = ReaderList<TResult>(comando);
            totalCount = (int)comando.Parameters["@p_totalCount"].Value;
            return resultado;
        }

        public List<TResult> GetPage<TResult>(string stringSql, out int totalCount, out string sumatoria, List<SqlParameter> parameters = null)
        where TResult : new()
        {
            SqlCommand comando = new SqlCommand(stringSql, conn);
            comando.CommandType = CommandType.StoredProcedure;

            foreach (var par in parameters) comando.Parameters.Add(par);
            comando.Parameters.Add(new SqlParameter { ParameterName = "@p_totalCount", Value = 0, Direction = ParameterDirection.InputOutput });
            comando.Parameters.Add(new SqlParameter { ParameterName = "@p_sumatoria", Value = "", SqlDbType = SqlDbType.NVarChar, Size = 20, Direction = ParameterDirection.InputOutput });

            var resultado = ReaderList<TResult>(comando);
            totalCount = (int)comando.Parameters["@p_totalCount"].Value;
            sumatoria = comando.Parameters["@p_sumatoria"].Value != DBNull.Value ? (string)comando.Parameters["@p_sumatoria"].Value : "";
            return resultado;
        }

        public TResult GetObject<TResult>(string stringSql, List<SqlParameter> parameters = null) where TResult : new()
        {
            SqlCommand comando = new SqlCommand(stringSql, conn);
            comando.CommandType = CommandType.StoredProcedure;
            if (parameters != null) foreach (var par in parameters) comando.Parameters.Add(par);
            return ReaderObject<TResult>(comando);
        }

        public object GetValue(string stringSql, List<SqlParameter> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(stringSql, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            object x = null;
            if (parameters != null) foreach (var par in parameters) cmd.Parameters.Add(par);
            try
            {
                x = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
            return x == System.DBNull.Value ? null : x;

        }

        public int Execute(string stringSql, List<SqlParameter> parameters = null, DbTransaction transaction = null)
        {
            var result = new object();
            SqlCommand cmd = new SqlCommand(stringSql, conn);
            if (transaction != null) cmd.Transaction = (SqlTransaction)transaction;
            int x = -1;
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameters != null) foreach (var par in parameters) cmd.Parameters.Add(par);
            try
            {
                x = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                if (transaction != null) cmd.Transaction.Rollback();
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
            return x;
        }

        public int SaveObject<TResult>(TResult obj, int usuarioId = 0, DbTransaction transaction = null) where TResult : new()
        {
            var tableAttribute = (EntityAttribute)Attribute.GetCustomAttribute(obj.GetType(), typeof(EntityAttribute));
            SqlCommand cmd = new SqlCommand(tableAttribute.SpSaveName, conn);
            if (transaction != null) cmd.Transaction = (SqlTransaction)transaction;
            int x = -1;
            cmd.CommandType = CommandType.StoredProcedure;
            var parameters = this.GetParams(obj);
            foreach (var par in parameters) cmd.Parameters.Add(par);
            try
            {
                x = Convert.ToInt32(cmd.ExecuteScalar());
                if (usuarioId != 0) {
                    var entidadId = (int)obj.GetType().GetProperty(tableAttribute.IdUnico).GetValue(obj);
                    var tipo = entidadId == 0 ? "I" : "U";
                    Auditoria.Save(this, obj, usuarioId, entidadId, tipo);
                }
            }
            catch (Exception ex)
            {
                if (transaction != null) cmd.Transaction.Rollback();
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
            return x;
        }


        public List<TResult> GetArray<TResult>(object filter = null)
        where TResult : new()
        {
            var tableAttribute = (EntityAttribute)Attribute.GetCustomAttribute(typeof(TResult), typeof(EntityAttribute));
            var parameters = this.GetParams(filter);
            return GetArray<TResult>(tableAttribute.SpListName, parameters);
        }
        public List<TResult> GetPage<TResult>(out int totalCount, object filter)
        where TResult : new()
        {
            var tableAttribute = (EntityAttribute)Attribute.GetCustomAttribute(typeof(TResult), typeof(EntityAttribute));
            var parameters = this.GetParams(filter);
            return GetPage<TResult>(tableAttribute.SpListName, out totalCount, parameters);
        }
        public List<TResult> GetPage<TResult>(out int totalCount, out string sumatoria, object filter)
        where TResult : new()
        {
            var tableAttribute = (EntityAttribute)Attribute.GetCustomAttribute(typeof(TResult), typeof(EntityAttribute));
            var parameters = this.GetParams(filter);
            return GetPage<TResult>(tableAttribute.SpListName, out totalCount, out sumatoria, parameters);
        }


        public TResult GetObject<TResult>(int id) where TResult : new()
        {
            var tableAttribute = (EntityAttribute)Attribute.GetCustomAttribute(typeof(TResult), typeof(EntityAttribute));
            return GetObject<TResult>(tableAttribute.SpGetName, new List<SqlParameter> { new SqlParameter { ParameterName = tableAttribute.IdGetParamName, Value = id } } );
        }
        public TResult GetObject<TResult>(object filter) where TResult : new()
        {
            var tableAttribute = (EntityAttribute)Attribute.GetCustomAttribute(typeof(TResult), typeof(EntityAttribute));
            var parameters = this.GetParams(filter);
            return GetObject<TResult>(tableAttribute.SpGetName, parameters);
        }


        public DbTransaction BeginTransaction()
        {
            return conn.BeginTransaction();
        }

        public void Commit(DbTransaction transaction)
        {
            transaction.Commit();
        }

        public void Rollback(DbTransaction transaction)
        {
            transaction.Rollback();
        }


        public void Close()
        {
            conn.Close();
        }

        public List<SqlParameter> GetParams(object pParams)
        {
            if (pParams == null) return null;
            var dic = new List<SqlParameter>();
            var pi = pParams.GetType().GetProperties();
            foreach (var p in pi)
            {
                var attr = (ParamAttribute)Attribute.GetCustomAttribute(p, typeof(ParamAttribute));
                if (attr != null)
                {
                    var v = p.GetValue(pParams);
                    if (attr.ConvertTo != null)
                    {
                        if (v != null && !string.IsNullOrEmpty(v.ToString())) v = Convert.ChangeType(v, attr.ConvertTo, new CultureInfo("es-AR"));
                    }
                    dic.Add(new SqlParameter { ParameterName = attr.ParamName, Value = v == null ? System.DBNull.Value : v });
                }


            }
            return dic;
        }

        public void Dispose()
        {
            if (conn.State == ConnectionState.Open) conn.Close();
        }
    }
}