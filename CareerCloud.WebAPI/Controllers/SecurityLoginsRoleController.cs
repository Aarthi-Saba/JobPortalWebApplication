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
    public class SecurityLoginsRoleController : ControllerBase
    {
        private readonly SecurityLoginsRoleLogic _logicref;

        public SecurityLoginsRoleController()
        {
            var repo = new EFGenericRepository<SecurityLoginsRolePoco>();
            _logicref = new SecurityLoginsRoleLogic(repo);
        }
        [HttpGet]
        [Route("loginrole/{securityloginroleid}")]
        public ActionResult GetSecurityLoginsRole(Guid securityloginroleid)
        {
            SecurityLoginsRolePoco poco = _logicref.Get(securityloginroleid);
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
        [Route("loginrole")]
        public ActionResult GetAllSecurityLoginRole()
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
        [Route("loginrole")]
        public ActionResult PostSecurityLoginRole([FromBody] SecurityLoginsRolePoco[] pocos)
        {
            _logicref.Add(pocos);
            return Ok();
        }
        [HttpDelete]
        [Route("loginrole")]
        public ActionResult DeleteSecurityLoginRole([FromBody] SecurityLoginsRolePoco[] pocos)
        {
            _logicref.Delete(pocos);
            return Ok();
        }
        [HttpPut]
        [Route("loginrole")]
        public ActionResult PutSecurityLoginRole([FromBody] SecurityLoginsRolePoco[] pocos)
        {
            _logicref.Update(pocos);
            return Ok();
        }
    }

}