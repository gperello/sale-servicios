using Sale.Base.Servicios.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sale.Base.Servicios.Classes
{
    public class ServiceResponse
    {
        public ServiceResponse()
        {
            Status = StatusResult.OK;
            Message = "OK";
        }
        public ServiceResponse(object result)
        {
            SetResult(result);
        }
        public StatusResult Status { get; private set; }
        public string Message { get; private set; }
        public object Result { get; set; }

        public void SetOkMessage(string message)
        {
            Status = StatusResult.OK;
            Message = message;
        }
        public void SetValidationError(string message)
        {
            Status = StatusResult.ValidationError;
            Message = message;
        }
        public void SetDataBaseError(string message)
        {
            Status = StatusResult.DataBaseError;
            Message = message;
        }
        public void SetStatus(StatusResult status, string message)
        {
            Status = status;
            Message = message;
        }
        public void SetResult(object result)
        {
            SetOkMessage("OK");
            Result = result;
        }
    }
}