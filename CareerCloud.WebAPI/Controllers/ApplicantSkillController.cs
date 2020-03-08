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
    [Route("api/careercloud/applicant/v1")]
    [ApiController]
    public class ApplicantSkillController : ControllerBase
    {
        private readonly ApplicantSkillLogic _logicref;

        public ApplicantSkillController()
        {
            var repo = new EFGenericRepository<ApplicantSkillPoco>();
            _logicref = new ApplicantSkillLogic(repo);
        }
        [HttpGet]
        [Route("skill/{applicantskillid}")]
        public ActionResult GetApplicantSkill(Guid applicantskillid)
        {
            ApplicantSkillPoco poco = _logicref.Get(applicantskillid);
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
        [Route("skill")]
        public ActionResult GetAllApplicantSkill()
        {
            var applicants = _logicref.GetAll();
            if (applicants != null)
            {
                return Ok(applicants);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        [Route("skill")]
        public ActionResult PostApplicantSkill([FromBody] ApplicantSkillPoco[] pocos)
        {
            _logicref.Add(pocos);
            return Ok();
        }

        [HttpPut]
        [Route("skill")]
        public ActionResult PutApplicantSkill([FromBody] ApplicantSkillPoco[] pocos)
        {
            _logicref.Update(pocos);
            return Ok();
        }

        [HttpDelete]
        [Route("skill")]
        public ActionResult DeleteApplicantSkill([FromBody] ApplicantSkillPoco[] pocos)
        {
            _logicref.Delete(pocos);
            return Ok();

        }
    }
}