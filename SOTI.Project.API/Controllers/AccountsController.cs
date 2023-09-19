using SOTI.Project.DAL.Interfaces;
using SOTI.Project.DAL.Models;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SOTI.Project.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Accounts")]
    public class AccountsController : ApiController
    {
        private readonly IAccount _account = null;

        public AccountsController(IAccount account)
        {
            _account = account;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IHttpActionResult> Login([FromBody] User user)
        {
            var result = await _account.ValidateUserAsync(user.Username, user.Password);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
