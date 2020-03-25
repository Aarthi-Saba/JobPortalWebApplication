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
using static CareerCloud.gRPC.Protos.ApplicantResume;

namespace CareerCloud.gRPC.Services
{
    public class ApplicantResumeService : ApplicantResumeBase
    {
        private readonly ApplicantResumeLogic _logic;

        public ApplicantResumeService()
        {
            _logic = new ApplicantResumeLogic(new EFGenericRepository<ApplicantResumePoco>());
        }
        public override Task<AppResumeProto> GetApplicantResume(AppResumeIdRequest request, ServerCallContext context)
        {
            ApplicantResumePoco poco = _logic.Get(Guid.Parse(request.Id));
            if (poco is null)
            {
                throw new ArgumentOutOfRangeException("Id not found in system");
            }
            return new Task<AppResumeProto>(
                () => new AppResumeProto
                {
                    Id = poco.Id.ToString(),
                    Applicant = poco.Applicant.ToString(),
                    Resume = poco.Resume,
                    LastUpdated = poco.LastUpdated is null ? null : Timestamp.FromDateTime((DateTime)poco.LastUpdated)
                });
        }
        public override Task<AppResumeArray> GetAllApplicantResume(Empty request, ServerCallContext context)
        {
            List<ApplicantResumePoco> pocos = _logic.GetAll();
            List<AppResumeProto> appResumeList = new List<AppResumeProto>();
            foreach (var poco in pocos)
            {
                AppResumeProto appResume = new AppResumeProto();
                appResume.Id = poco.Id.ToString();
                appResume.Applicant = poco.Applicant.ToString();
                appResume.Resume = poco.Resume;
                appResume.LastUpdated = poco.LastUpdated is null ? null : Timestamp.FromDateTime((DateTime)poco.LastUpdated);
                appResumeList.Add(appResume);
            }
            AppResumeArray appResumeArray = new AppResumeArray();
            appResumeArray.AppResume.AddRange(appResumeList);
            return new Task<AppResumeArray>(() => appResumeArray);
        }

        public override Task<Empty> CreateApplicantResume(AppResumeArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateApplicantResume(AppResumeArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteApplicantResume(AppResumeArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<ApplicantResumePoco> ProtoToPoco(AppResumeArray request)
        {
            List<ApplicantResumePoco> pocos = new List<ApplicantResumePoco>();
            var inputList = request.AppResume.ToList();
            foreach (var item in inputList)
            {
                var poco = new ApplicantResumePoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Applicant = Guid.Parse(item.Applicant);
                poco.Resume = item.Resume;
                poco.LastUpdated = item.LastUpdated.ToDateTime();
                pocos.Add(poco);
            }            
            return pocos;
        }
    }

}
