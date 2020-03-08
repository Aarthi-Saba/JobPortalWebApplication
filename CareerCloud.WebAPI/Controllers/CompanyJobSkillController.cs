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
    [Route("api/careercloud/company/v1")]
    [ApiController]
    public class CompanyJobSkillController : ControllerBase
    {
        private readonly CompanyJobSkillLogic _logicref;

        public CompanyJobSkillController()
        {
            var repo = new EFGenericRepository<CompanyJobSkillPoco>();
            _logicref = new CompanyJobSkillLogic(repo);
        }
        [HttpGet]
        [Route("jobskill/{jobskillid}")]
        public ActionResult GetCompanyJobSkill(Guid jobskillid)
        {
            CompanyJobSkillPoco poco = _logicref.Get(jobskillid);
            if (poco != null)
            {
                return Ok(poco);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("jobskill")]
        public ActionResult GetAllCompanyJobSkill()
        {
            var companylist = _logicref.GetAll();
            if (companylist != null)
            {
                return Ok(companylist);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        [Route("jobskill")]
        public ActionResult PostCompanyJobSkill([FromBody] CompanyJobSkillPoco[] pocos)
        {
            _logicref.Add(pocos);
            return Ok();
        }

        [HttpPut]
        [Route("jobskill")]
        public ActionResult PutCompanyJobSkill([FromBody] CompanyJobSkillPoco[] pocos)
        {
            _logicref.Update(pocos);
            return Ok();
        }

        [HttpDelete]
        [Route("jobskill")]
        public ActionResult DeleteCompanyJobSkill([FromBody] CompanyJobSkillPoco[] pocos)
        {
            _logicref.Delete(pocos);
            return Ok();

        }
    }
}