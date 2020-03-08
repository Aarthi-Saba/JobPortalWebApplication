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
    public class CompanyJobController : ControllerBase
    {
        private readonly CompanyJobLogic _logicref;

        public CompanyJobController()
        {
            var repo = new EFGenericRepository<CompanyJobPoco>();
            _logicref = new CompanyJobLogic(repo);
        }

        [HttpGet]
        [Route("job/{companyjobid}")]
        public ActionResult GetCompanyJob(Guid companyjobid)
        {
            CompanyJobPoco poco = _logicref.Get(companyjobid);
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
        [Route("job")]
        public ActionResult GetAllCompanyJob()
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
        [Route("job")]
        public ActionResult PostCompanyJob([FromBody] CompanyJobPoco[] pocos)
        {
            _logicref.Add(pocos);
            return Ok();
        }

        [HttpPut]
        [Route("job")]
        public ActionResult PutCompanyJob([FromBody] CompanyJobPoco[] pocos)
        {
            _logicref.Update(pocos);
            return Ok();
        }

        [HttpDelete]
        [Route("job")]
        public ActionResult DeleteCompanyJob([FromBody] CompanyJobPoco[] pocos)
        {
            _logicref.Delete(pocos);
            return Ok();

        }
    }
}