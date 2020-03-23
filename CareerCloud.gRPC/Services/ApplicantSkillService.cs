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
using static CareerCloud.gRPC.Protos.ApplicantSkill;

namespace CareerCloud.gRPC.Services
{
    public class ApplicantSkillService : ApplicantSkillBase
    {
        private readonly ApplicantSkillLogic _logic;

        public ApplicantSkillService()
        {
            _logic = new ApplicantSkillLogic(new EFGenericRepository<ApplicantSkillPoco>());
        }
        public override Task<AppSkillProto> GetApplicantSkill(AppSkillIdRequest request, ServerCallContext context)
        {
            ApplicantSkillPoco poco = _logic.Get(Guid.Parse(request.Id));
            if (poco is null)
            {
                throw new ArgumentOutOfRangeException("Id not found !!");
            }
            return new Task<AppSkillProto>(
                () => new AppSkillProto
                {
                    Id = poco.Id.ToString(),
                    Applicant = poco.Applicant.ToString(),
                    Skill = poco.Skill,
                    SkillLevel = poco.SkillLevel,
                    StartMonth = Convert.ToInt32(poco.StartMonth),
                    StartYear = poco.StartYear,
                    EndMonth = Convert.ToInt32(poco.EndMonth),
                    EndYear = poco.EndYear
                });
        }
        public override Task<Empty> CreateApplicantSkill(AppSkillProto request, ServerCallContext context)
        {
            ApplicantSkillPoco[] pocos = new ApplicantSkillPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Applicant = Guid.Parse(request.Applicant);
                poco.Skill = request.Skill;
                poco.SkillLevel = request.SkillLevel;
                poco.StartMonth = Convert.ToByte(request.StartMonth);
                poco.StartYear = request.StartYear;
                poco.EndMonth = Convert.ToByte(request.EndMonth);
                poco.EndYear = request.EndYear;
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateApplicantSkill(AppSkillProto request, ServerCallContext context)
        {
            ApplicantSkillPoco[] pocos = new ApplicantSkillPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Applicant = Guid.Parse(request.Applicant);
                poco.Skill = request.Skill;
                poco.SkillLevel = request.SkillLevel;
                poco.StartMonth = Convert.ToByte(request.StartMonth);
                poco.StartYear = request.StartYear;
                poco.EndMonth = Convert.ToByte(request.EndMonth);
                poco.EndYear = request.EndYear;
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteApplicantSkill(AppSkillProto request, ServerCallContext context)
        {
            ApplicantSkillPoco[] pocos = new ApplicantSkillPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Applicant = Guid.Parse(request.Applicant);
                poco.Skill = request.Skill;
                poco.SkillLevel = request.SkillLevel;
                poco.StartMonth = Convert.ToByte(request.StartMonth);
                poco.StartYear = request.StartYear;
                poco.EndMonth = Convert.ToByte(request.EndMonth);
                poco.EndYear = request.EndYear;
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
