using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Sale.Base.Servicios.Classes
{
    public static class FormatterExtensions
    {
        public static Response AsStream(this IResponseFormatter formatter, byte[] image, string contentType)
        {
            return new StreamResponse(image, contentType);
        }
    }

    public class StreamResponse : Response
    {
        public StreamResponse(byte[] image, string contentType)
        {
            Contents = stream =>
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(image);
                }
            };
            ContentType = contentType;
            StatusCode = HttpStatusCode.OK;
        }
    }
}