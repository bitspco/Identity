using Bitspco.Framework.Common;
using Bitspco.Identity.Common.Entities;
using System.Linq;
using System.Web.Http;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    [RoutePrefix("Positions")]
    public class PositionsController : ApiController
    {
        //---------------------------- Position ----------------------------//
        [Route(""), HttpGet]
        public IQueryable<Position> Select() => Controller.GetAllPositions();
        [Route("{id:int}"), HttpGet]
        public Position GetById(int id) => Controller.GetPosition(id);
        [Route(""), HttpPost]
        public OperationResult<Position> Add(Position obj) => Controller.AddPosition(obj);
        [Route(""), HttpPatch]
        public OperationResult<Position> Change(Position obj) => Controller.ChangePosition(obj);
        [Route("{id:int}"), HttpDelete]
        public OperationResult<Position> Remove(int id) => Controller.RemovePosition(id);
        //---------------------------- User ----------------------------//
        [Route("{id:int}/Users"), HttpGet]
        public IQueryable<User> GetAllUser(int id) => Controller.GetAllUsersByPositionId(id);
    }
}
