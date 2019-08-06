using Bitspco.Framework.Common;
using Bitspco.Identity.Common.Entities;
using System.Linq;
using System.Web.Http;

namespace Bitspco.Identity.Service.WebApi.Controllers
{
    [RoutePrefix("Tokens")]
    public class TokensController : ApiController
    {
        //---------------------------- Token ----------------------------//
        [Route(""), HttpGet]
        public IQueryable<Token> Select() => Controller.GetAllTokens();
        [Route("{id:int}"), HttpGet]
        public Token GetById(int id) => Controller.GetToken(id);
        [Route(""), HttpPost]
        public OperationResult<Token> Add(Token obj) => Controller.AddToken(obj);
        [Route(""), HttpPatch]
        public OperationResult<Token> Change(Token obj) => Controller.ChangeToken(obj);
        [Route("{id:int}/Expire"), HttpPut]
        public OperationResult<Token> Expire(int id) => Controller.ExpireToken(id);
        [Route("{id:int}"), HttpDelete]
        public OperationResult<Token> Remove(int id) => Controller.RemoveToken(id);
        //---------------------------- Usage ----------------------------//
        [Route("{id:int}/Usages"), HttpGet]
        public IQueryable<TokenUsage> GetAllUsage(int id) => Controller.GetAllTokenUsagesByTokenId(id);
    }
}
