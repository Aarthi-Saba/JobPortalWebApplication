using CareerCloud.BusinessLogicLayer;
using CareerCloud.EntityFrameworkDataAccess;
using CareerCloud.gRPC.Protos;
using CareerCloud.Pocos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CareerCloud.gRPC.Protos.CompanyJobSkill;

namespace CareerCloud.gRPC.Services
{
    public class CompanyJobSkillService : CompanyJobSkillBase
    {
        private readonly CompanyJobSkillLogic _logic;

        public CompanyJobSkillService()
        {
            _logic = new CompanyJobSkillLogic(new EFGenericRepository<CompanyJobSkillPoco>());
        }
        public override Task<ComJobSkillProto> GetCompanyJobSkill(ComJobSkillIdRequest request, ServerCallContext context)
        {
            CompanyJobSkillPoco poco = _logic.Get(Guid.Parse(request.Id));
            if(poco is null)
            {
                throw new ArgumentOutOfRangeException("Id does not exist");
            }
            return new Task<ComJobSkillProto>(
                () => new ComJobSkillProto
                {
                    Id = poco.Id.ToString(),
                    Job = poco.Job.ToString(),
                    Skill = poco.Skill,
                    SkillLevel = poco.SkillLevel,
                    Importance = poco.Importance
                });
        }
        public override Task<Empty> CreateCompanyJobSkill(ComJobSkillProto request, ServerCallContext context)
        {
            CompanyJobSkillPoco[] pocos = new CompanyJobSkillPoco[1];
            foreach(var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Job = Guid.Parse(request.Job);
                poco.Skill = request.Skill;
                poco.SkillLevel = request.SkillLevel;
                poco.Importance = request.Importance;
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateCompanyJobSkill(ComJobSkillProto request, ServerCallContext context)
        {
            CompanyJobSkillPoco[] pocos = new CompanyJobSkillPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Job = Guid.Parse(request.Job);
                poco.Skill = request.Skill;
                poco.SkillLevel = request.SkillLevel;
                poco.Importance = request.Importance;
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyJobSkill(ComJobSkillProto request, ServerCallContext context)
        {
            CompanyJobSkillPoco[] pocos = new CompanyJobSkillPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Job = Guid.Parse(request.Job);
                poco.Skill = request.Skill;
                poco.SkillLevel = request.SkillLevel;
                poco.Importance = request.Importance;
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
