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
    [Route("api/careercloud/system/v1")]
    [ApiController]
    public class SystemCountryCodeController : ControllerBase
    {
        private readonly SystemCountryCodeLogic _logicref;

        public SystemCountryCodeController()
        {
            var repo = new EFGenericRepository<SystemCountryCodePoco>();
            _logicref = new SystemCountryCodeLogic(repo);
        }
        [HttpGet]
        [Route("countrycode/{systemcountrycode}")]
        public ActionResult GetSystemCountryCode(string systemcountrycode)
        {
            SystemCountryCodePoco poco = _logicref.Get(systemcountrycode);
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
        [Route("countrycode")]
        public ActionResult GetAllSystemCountryCode()
        {
            var countries = _logicref.GetAll();
            if (countries == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(countries);
            }
        }

        [HttpPost]
        [Route("countrycode")]
        public ActionResult PostSystemCountryCode([FromBody] SystemCountryCodePoco[] pocos)
        {
            _logicref.Add(pocos);
            return Ok();
        }
        [HttpDelete]
        [Route("countrycode")]
        public ActionResult DeleteSystemCountryCode([FromBody] SystemCountryCodePoco[] pocos)
        {
            _logicref.Delete(pocos);
            return Ok();
        }
        [HttpPut]
        [Route("countrycode")]
        public ActionResult PutSystemCountryCode([FromBody] SystemCountryCodePoco[] pocos)
        {
            _logicref.Update(pocos);
            return Ok();
        }
    }
}