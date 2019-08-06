using Bitspco.Framework.Common;
using Bitspco.Identity.Common.Entities;
using System.Linq;
using System.Web.Http;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    [RoutePrefix("Events")]
    public class EventsController : ApiController
    {
        //---------------------------- Event ----------------------------//
        [Route(""), HttpGet]
        public IQueryable<Event> Select() => Controller.GetAllEvents();
        [Route("{id:int}"), HttpGet]
        public Event GetById(int id) => Controller.GetEvent(id);
        [Route(""), HttpPost]
        public OperationResult<Event> Add(Event obj) => Controller.AddEvent(obj);
        [Route(""), HttpPatch]
        public OperationResult<Event> Change(Event obj) => Controller.ChangeEvent(obj);
        [Route("{id:int}"), HttpDelete]
        public OperationResult<Event> Remove(int id) => Controller.RemoveEvent(id);
    }
}
