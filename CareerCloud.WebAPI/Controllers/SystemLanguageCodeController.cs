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
    public class SystemLanguageCodeController : ControllerBase
    {
        private readonly SystemLanguageCodeLogic _logicref;

        public SystemLanguageCodeController()
        {
            var repo = new EFGenericRepository<SystemLanguageCodePoco>();
            _logicref = new SystemLanguageCodeLogic(repo);
        }
        [HttpGet]
        [Route("languagecode/{systemlanguagecode}")]
        public ActionResult GetSystemLanguageCode(string systemlanguagecode)
        {
            SystemLanguageCodePoco poco = _logicref.Get(systemlanguagecode);
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
        [Route("languagecode")]
        public ActionResult GetAllSystemLanguageCode()
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
        [Route("languagecode")]
        public ActionResult PostSystemLanguageCode([FromBody] SystemLanguageCodePoco[] pocos)
        {
            _logicref.Add(pocos);
            return Ok();
        }
        [HttpDelete]
        [Route("languagecode")]
        public ActionResult DeleteSystemLanguageCode([FromBody] SystemLanguageCodePoco[] pocos)
        {
            _logicref.Delete(pocos);
            return Ok();
        }
        [HttpPut]
        [Route("languagecode")]
        public ActionResult PutSystemLanguageCode([FromBody] SystemLanguageCodePoco[] pocos)
        {
            _logicref.Update(pocos);
            return Ok();
        }
    }
}