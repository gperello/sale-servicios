using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sale.Base.Data
{
    partial class SqlServerConnection : IDisposable 
    {
        public byte[] GetExcel(string stringSql, List<SqlParameter> parameters = null)
        {
            SqlCommand comando = new SqlCommand(stringSql, conn);
            comando.CommandType = CommandType.StoredProcedure;
            if (parameters != null) foreach (var par in parameters) comando.Parameters.Add(par);
            SqlDataReader dr = null;
            try
            {
                dr = comando.ExecuteReader();
                var p = new ExcelPackage();
                p.Workbook.Worksheets.Add(stringSql);
                var hoja = p.Workbook.Worksheets[1];
                var fila = 1;
                hoja.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                hoja.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
                hoja.Cells[fila, 1, fila, dr.FieldCount].Style.Font.Bold = true;
                hoja.Cells[fila, 1, fila, dr.FieldCount].Style.Font.Size = 15;
                hoja.Cells[fila, 1, fila, dr.FieldCount].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                for (var i = 0; i < dr.FieldCount; i++)
                {
                    hoja.Cells[fila, i + 1].Value = dr.GetName(i);
                }
                fila++;
                while (dr.Read())
                {
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        hoja.Cells[fila, i + 1].Value = dr.GetValue(i);
                    }
                    fila++;
                }
                return p.GetAsByteArray();
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
        public byte[] GetExcel(string stringSql, out int totalCount, List<SqlParameter> parameters = null)
        {
            SqlCommand comando = new SqlCommand(stringSql, conn);
            comando.CommandType = CommandType.StoredProcedure;
            if (parameters != null) foreach (var par in parameters) comando.Parameters.Add(par);
            comando.Parameters.Add(new SqlParameter { ParameterName = "@p_totalCount", Value = 0, Direction = ParameterDirection.InputOutput });
            SqlDataReader dr = null;
            try
            {
                dr = comando.ExecuteReader();
                var p = new ExcelPackage();
                p.Workbook.Worksheets.Add(stringSql);
                var hoja = p.Workbook.Worksheets[1];
                var fila = 1;
                hoja.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                hoja.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
                hoja.Cells[fila, 1, fila, dr.FieldCount].Style.Font.Bold = true;
                hoja.Cells[fila, 1, fila, dr.FieldCount].Style.Font.Size = 15;
                hoja.Cells[fila, 1, fila, dr.FieldCount].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                for (var i = 0; i < dr.FieldCount; i++)
                {
                    hoja.Cells[fila, i + 1].Value = dr.GetName(i);
                }
                fila++;
                while (dr.Read())
                {
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        hoja.Cells[fila, i + 1].Value = dr.GetValue(i);
                    }
                    fila++;
                }
                totalCount = (int)comando.Parameters["@p_totalCount"].Value;
                return p.GetAsByteArray();
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
    }
}
