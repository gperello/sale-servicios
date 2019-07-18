using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sale.Base.Servicios.Enums
{
    public enum StatusResult
    {
        OK,
        ValidationError,
        DataBaseError,
        GeneralError,
        UnknownError
    }
}