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
    public class SecurityRoleController : ControllerBase
    {
        private readonly SecurityRoleLogic _logicref;

        public SecurityRoleController()
        {
            var repo = new EFGenericRepository<SecurityRolePoco>();
            _logicref = new SecurityRoleLogic(repo);

        }
        [HttpGet]
        [Route("role/{securityroleid}")]
        public ActionResult GetSecurityRole(Guid securityroleid)
        {
            SecurityRolePoco poco = _logicref.Get(securityroleid);
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
        [Route("role")]
        public ActionResult GetAllSecurityRole()
        {
            var rolelist = _logicref.GetAll();
            if (rolelist == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(rolelist);
            }
        }

        [HttpPost]
        [Route("role")]
        public ActionResult PostSecurityRole([FromBody] SecurityRolePoco[] pocos)
        {
            _logicref.Add(pocos);
            return Ok();
        }
        [HttpDelete]
        [Route("role")]
        public ActionResult DeleteSecurityRole([FromBody] SecurityRolePoco[] pocos)
        {
            _logicref.Delete(pocos);
            return Ok();
        }
        [HttpPut]
        [Route("role")]
        public ActionResult PutSecurityRole([FromBody] SecurityRolePoco[] pocos)
        {
            _logicref.Update(pocos);
            return Ok();
        }
    }
}