using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CareerCloud.BusinessLogicLayer;
using CareerCloud.EntityFrameworkDataAccess;
using CareerCloud.Pocos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCloud.WebAPI.Controllers
{
    [Route("api/careercloud/security/v1")]
    [ApiController]
    public class SecurityLoginController : ControllerBase
    {
        private readonly SecurityLoginLogic _logicref;

        public SecurityLoginController()
        {
            var repo = new EFGenericRepository<SecurityLoginPoco>();
            _logicref = new SecurityLoginLogic(repo);
        }

        [HttpGet]
        [Route("login/{securityloginid}")]
        public ActionResult GetSecurityLogin(Guid securityloginid)
        {
            SecurityLoginPoco poco = _logicref.Get(securityloginid);
            if (poco == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(poco);
            }
        }
        [HttpGet]
        [Route("login")]
        public ActionResult GetAllSecurityLogin()
        {
            var loginlist = _logicref.GetAll();
            if(loginlist == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(loginlist);
            }
        }

        [HttpPost]
        [Route("login")]
        public ActionResult PostSecurityLogin([FromBody] SecurityLoginPoco[] pocos)
        {
            _logicref.Add(pocos);
            return Ok();
        }
        [HttpDelete]
        [Route("login")]
        public ActionResult DeleteSecurityLogin([FromBody] SecurityLoginPoco[] pocos)
        {
            _logicref.Delete(pocos);
            return Ok();
        }
        [HttpPut]
        [Route("login")]
        public ActionResult PutSecurityLogin([FromBody] SecurityLoginPoco[] pocos)
        {
            _logicref.Update(pocos);
            return Ok();
        }
    }
}