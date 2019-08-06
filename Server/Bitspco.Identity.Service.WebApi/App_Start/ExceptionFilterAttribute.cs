using Bitspco.Framework.Common;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Bitspco.Identity.Service.WebApi
{
    public class ExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            HttpStatusCode status = HttpStatusCode.OK;
            var ex = context.Exception;
            var op = new OperationResult<string>().SetSuccess(false).SetMessage(ex.Message);
            var ie = ex.InnerException;
            while (ie != null)
            {
                if (string.IsNullOrEmpty(op.Data)) op.Data = "";
                op.Data += ie.Message;
                ie = ie.InnerException;
            }
            if (context.Response != null) op.SetCode((int)context.Response.StatusCode);
            if (ex != null)
            {
                op.Code = (int)HttpStatusCode.InternalServerError;
                if (ex is ClientException)
                {
                    op = ((ClientException)ex).OperationResult;
                }
                if (ex is UnauthorizedAccessException)
                {
                    status = HttpStatusCode.Unauthorized;
                    op.Code = (int)status;
                    op.Message = ex.Message;
                }
                else
                {
                }
                context.Response = context.Request.CreateResponse(status, op);
            }
        }
    }
}