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
    public class SecurityLoginsLogController : ControllerBase
    {
        private readonly SecurityLoginsLogLogic _logicref;

        public SecurityLoginsLogController()
        {
            var repo = new EFGenericRepository<SecurityLoginsLogPoco>();
            _logicref = new SecurityLoginsLogLogic(repo);
        }

        [HttpGet]
        [Route("loginlog/{securityloginlogid}")]
        public ActionResult GetSecurityLoginLog(Guid securityloginlogid)
        {
            SecurityLoginsLogPoco poco = _logicref.Get(securityloginlogid);
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
        [Route("loginlog")]
        public ActionResult GetAllSecurityLoginLog()
        {
            var loginlist = _logicref.GetAll();
            if (loginlist == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(loginlist);
            }
        }

        [HttpPost]
        [Route("loginlog")]
        public ActionResult PostSecurityLoginLog([FromBody] SecurityLoginsLogPoco[] pocos)
        {
            _logicref.Add(pocos);
            return Ok();
        }
        [HttpDelete]
        [Route("loginlog")]
        public ActionResult DeleteSecurityLoginLog([FromBody] SecurityLoginsLogPoco[] pocos)
        {
            _logicref.Delete(pocos);
            return Ok();
        }
        [HttpPut]
        [Route("loginlog")]
        public ActionResult PutSecurityLoginLog([FromBody] SecurityLoginsLogPoco[] pocos)
        {
            _logicref.Update(pocos);
            return Ok();
        }
    }
}