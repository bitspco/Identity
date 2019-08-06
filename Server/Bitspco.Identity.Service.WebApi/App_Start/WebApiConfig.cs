using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.OData;

namespace Bitspco.Identity.Service.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            // Web API routes
            config.MapHttpAttributeRoutes();
            //Filters
            config.Filters.Add(new ExceptionFilterAttribute());
            config.Filters.Add(new EnableQueryAttribute()
            {
                PageSize = 100,
            });
        }
    }
}
