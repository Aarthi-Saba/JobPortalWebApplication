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
            if (poco is null)
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
        public override Task<ComJobSkillArray> GetAllCompanyJobSkill(Empty request, ServerCallContext context)
        {
            List<CompanyJobSkillPoco> pocos = _logic.GetAll();
            List<ComJobSkillProto> compJobSkillList = new List<ComJobSkillProto>();
            foreach (var poco in pocos)
            {
                ComJobSkillProto compJobSkill = new ComJobSkillProto();
                compJobSkill.Id = poco.Id.ToString();
                compJobSkill.Job = poco.Job.ToString();
                compJobSkill.Skill = poco.Skill;
                compJobSkill.SkillLevel = poco.SkillLevel;
                compJobSkill.Importance = poco.Importance;
                compJobSkillList.Add(compJobSkill);
            }
            ComJobSkillArray comJobSkillArray = new ComJobSkillArray();
            comJobSkillArray.ComJobSkill.AddRange(compJobSkillList);
            return new Task<ComJobSkillArray>(() => comJobSkillArray);
        }
        public override Task<Empty> CreateCompanyJobSkill(ComJobSkillArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateCompanyJobSkill(ComJobSkillArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyJobSkill(ComJobSkillArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<CompanyJobSkillPoco> ProtoToPoco(ComJobSkillArray request)
        {
            List<CompanyJobSkillPoco> pocos = new List<CompanyJobSkillPoco>();
            var inputList = request.ComJobSkill.ToList();
            foreach (var item in inputList)
            {
                var poco = new CompanyJobSkillPoco();
                poco.Job = Guid.Parse(item.Job);
                poco.Skill = item.Skill;
                poco.SkillLevel = item.SkillLevel;
                poco.Importance = item.Importance;
                pocos.Add(poco);
            }
            return pocos;
        }
    }
}
