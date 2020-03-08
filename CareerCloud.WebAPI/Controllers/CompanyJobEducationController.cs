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
    public class CompanyJobEducationController : ControllerBase
    {
        private readonly CompanyJobEducationLogic _logicref;

        public CompanyJobEducationController()
        {
            var repo = new EFGenericRepository<CompanyJobEducationPoco>();
            _logicref = new CompanyJobEducationLogic(repo);
        }

        [HttpGet]
        [Route("jobeducation/{companyjobeducationid}")]
        public ActionResult GetCompanyJobEducation(Guid companyjobeducationid)
        {
            CompanyJobEducationPoco poco = _logicref.Get(companyjobeducationid);
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
        [Route("jobeducation")]
        public ActionResult GetAllCompanyJobEducation()
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
        [Route("jobeducation")]
        public ActionResult PostCompanyJobEducation([FromBody] CompanyJobEducationPoco[] pocos)
        {
            _logicref.Add(pocos);
            return Ok();
        }

        [HttpPut]
        [Route("jobeducation")]
        public ActionResult PutCompanyJobEducation([FromBody] CompanyJobEducationPoco[] pocos)
        {
            _logicref.Update(pocos);
            return Ok();
        }

        [HttpDelete]
        [Route("jobeducation")]
        public ActionResult DeleteCompanyJobEducation([FromBody] CompanyJobEducationPoco[] pocos)
        {
            _logicref.Delete(pocos);
            return Ok();

        }
    }
}