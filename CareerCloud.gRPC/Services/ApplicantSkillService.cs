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
        public override Task<AppSkillArray> GetAllApplicantSkill(Empty request, ServerCallContext context)
        {
            List<AppSkillProto> appSkillList = new List<AppSkillProto>();
            List<ApplicantSkillPoco> pocos = _logic.GetAll();
            foreach (var poco in pocos)
            {
                AppSkillProto appSkill = new AppSkillProto();
                appSkill.Id = poco.Id.ToString();
                appSkill.Applicant = poco.Applicant.ToString();
                appSkill.Skill = poco.Skill;
                appSkill.SkillLevel = poco.SkillLevel;
                appSkill.StartMonth = Convert.ToInt32(poco.StartMonth);
                appSkill.StartYear = poco.StartYear;
                appSkill.EndMonth = Convert.ToInt32(poco.EndMonth);
                appSkill.EndYear = poco.EndYear;
                appSkillList.Add(appSkill);
            }
            AppSkillArray appSkillArray = new AppSkillArray();
            appSkillArray.AppSkill.AddRange(appSkillList);
            return new Task<AppSkillArray>(() => appSkillArray);
        }
        public override Task<Empty> CreateApplicantSkill(AppSkillArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateApplicantSkill(AppSkillArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteApplicantSkill(AppSkillArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<ApplicantSkillPoco> ProtoToPoco(AppSkillArray request)
        {
            List<ApplicantSkillPoco> pocos = new List<ApplicantSkillPoco>();
            var inputList = request.AppSkill.ToList();
            foreach (var item in inputList)
            {
                ApplicantSkillPoco poco = new ApplicantSkillPoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Applicant = Guid.Parse(item.Applicant);
                poco.Skill = item.Skill;
                poco.SkillLevel = item.SkillLevel;
                poco.StartMonth = Convert.ToByte(item.StartMonth);
                poco.StartYear = item.StartYear;
                poco.EndMonth = Convert.ToByte(item.EndMonth);
                poco.EndYear = item.EndYear;
                pocos.Add(poco);
            }
            return pocos;
        }
    }
}
