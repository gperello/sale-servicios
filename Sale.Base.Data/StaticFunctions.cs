﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sale.Base.Data
{
    public static class StaticFunctions
    {

        public static IEnumerable<TResult> GetList<TResult>(int cant) where TResult : new()
        {
            Random random = new Random();
            var list = new List<TResult>();
            return list;
        }
        private static string GetString(Random random, int length)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            
            for (int i = 0; i < length; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();      
        }

        private static string GetByValues(Random random, string[] values)
        {
            int randomInteger = random.Next(0, values.Length);

            return values[randomInteger];
        }

        private static int GetInteger(Random random, long min, long max)
        {
            int randomInteger = random.Next((int)min, (int)max);

            return randomInteger;
        }

        public static bool GetBoolean(Random random)
        {
            return random.Next(0, 2) == 0;
        }

        private static DateTime GetDate(Random random, string min, string max)
        {
            var auxMin = min.Split('/');
            var auxMax = max.Split('/');
            DateTime minDate = new DateTime(int.Parse(auxMin[0]), int.Parse(auxMin[2]), int.Parse(auxMin[2]));
            DateTime maxDate = new DateTime(int.Parse(auxMax[0]), int.Parse(auxMax[2]), int.Parse(auxMax[2]));

            int range = (maxDate - minDate).Days;
            return minDate.AddDays(random.Next(range));
        }

        private static Decimal GetDecimal(Random random,  long min, long max)
        {
            var promedio = (min + (max - min) / 2);
            return Convert.ToDecimal(random.NextDouble() * promedio);
        }

        private static long GetLong(Random random,  long min, long max)
        {
            var promedio = (min + (max - min) / 2);
            return Convert.ToInt64(random.NextDouble() * promedio);
        }

        public static ListItem[] GetListItem(int cantidad, string detalle)
        {
            var list = new List<ListItem>();
            ListItem item = new ListItem();

            for (int i = 0; i < cantidad; i++)
            {
                item.id = (i + 1).ToString();
                item.text = detalle + item.id.ToString();

                list.Add(item);
            }

            return list.ToArray();
        }

        public static string GetRandomPassword(Random random, int length)
        {
            StringBuilder builder = new StringBuilder();
            char ch;

            for (int i = 0; i < length / 2; i++)
            {
                int randomInteger = random.Next(0, 5);
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
                builder.Append(randomInteger);
            }

            return builder.ToString();
        }

        public static void enviarCorreo(string asunto, bool isHtml, string body, string emailDestinatario)
        {
            SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["Smtp"]);
            var mail = new MailMessage();

            mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "Cooperación Seguros");
            mail.To.Add(emailDestinatario);
            mail.Subject = asunto;
            mail.IsBodyHtml = isHtml;
            mail.Body = body;

            SmtpServer.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            if (ConfigurationManager.AppSettings["Domain"] != "")
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["UserName"],
                                                                        ConfigurationManager.AppSettings["Password"],
                                                                        ConfigurationManager.AppSettings["Domain"]);
            else
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["UserName"],
                                                                        ConfigurationManager.AppSettings["Password"]);
            SmtpServer.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]);
            

            try
            {
                SmtpServer.Send(mail);
                mail.Dispose();
            }
            catch (Exception ex)
            {
                //excepcion
            }
        }

        public static void Enviarcorreo2(string asunto, bool isHtml, string body, string emailDestinatario)
        { 
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(emailDestinatario));
            msg.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "Cooperación Seguros");
            msg.Subject = asunto;
            msg.Body = body;
            msg.IsBodyHtml = isHtml;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["UserName"], ConfigurationManager.AppSettings["Password"]);
            client.Port = int.Parse(ConfigurationManager.AppSettings["Port"]); 
            client.Host = ConfigurationManager.AppSettings["Smtp"];
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("WEB CLIENTES ENVIO DE MAIL", ex.ToString(), EventLogEntryType.Error);
            }
           
        }

        public static byte[] ExportToExcel(this DataTable Tbl, string ExcelFilePath = null)
        {
            try
            {
                if (Tbl == null || Tbl.Columns.Count == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");

                // load excel, and create a new workbook
                Excel.Application excelApp = new Excel.Application();
                excelApp.Workbooks.Add();

                // single worksheet
                Excel._Worksheet workSheet = excelApp.ActiveSheet;

                workSheet.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                workSheet.Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                // column headings
                for (int i = 0; i < Tbl.Columns.Count; i++)
                {
                    workSheet.Cells[1, (i + 1)] = Tbl.Columns[i].ColumnName;

                    switch (Tbl.Columns[i].ColumnName)
                    {
                        case "MvsDescripcion":
                            workSheet.Cells[1, (i + 1)].EntireColumn.ColumnWidth = Tbl.Columns[i].ColumnName.Length + 21;
                            break;
                        case "ClienteFinal":
                            workSheet.Cells[1, (i + 1)].EntireColumn.ColumnWidth = Tbl.Columns[i].ColumnName.Length + 21;
                            break;
                        case "Opcionales":
                            workSheet.Cells[1, (i + 1)].EntireColumn.ColumnWidth = Tbl.Columns[i].ColumnName.Length + 55;
                            break;
                        default:
                            workSheet.Cells[1, (i + 1)].EntireColumn.ColumnWidth = Tbl.Columns[i].ColumnName.Length + 5;
                            break;
                    }

                }

                // rows
                for (int i = 0; i < Tbl.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (int j = 0; j < Tbl.Columns.Count; j++)
                    {
                        workSheet.Cells[(i + 2), (j + 1)] = Tbl.Rows[i][j];
                    }
                }

                //Used Range Rows
                Excel.Range rows = workSheet.UsedRange.Rows;

                //set borders 
                rows.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                //set gray and bold columns headings
                foreach (Excel.Range row in rows)
                {
                    if (!string.IsNullOrEmpty(row.Cells[1].Value as String))
                    {
                        row.Interior.ColorIndex = 15;
                        row.EntireRow.Font.Bold = true;
                    }

                    break;

                }


                // check fielpath
                if (ExcelFilePath != null && ExcelFilePath != "")
                {
                    try
                    {
                        using (var ms = new MemoryStream())
                        {
                            excelApp.Workbooks[1].SaveCopyAs(ms);
                            return ms.ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                            + ex.Message);
                    }
                }
                else    // no filepath is given
                {
                    excelApp.Visible = true;
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }
    }
}
