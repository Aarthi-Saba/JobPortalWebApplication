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
using static CareerCloud.gRPC.Protos.ApplicantEducation;

namespace CareerCloud.gRPC.Services
{
    public class ApplicantEducationService : ApplicantEducationBase
    {
        private readonly ApplicantEducationLogic _logic;

        public ApplicantEducationService()
        {
            _logic = new ApplicantEducationLogic(new EFGenericRepository<ApplicantEducationPoco>());
        }
        public override Task<AppEduProto> GetApplicantEducation(AppEduIdRequest request, ServerCallContext context)
        {
            ApplicantEducationPoco poco = _logic.Get(Guid.Parse(request.Id));
            if (poco is null)
            {
                throw new ArgumentOutOfRangeException("Given Id does not exist in system");
            }
            return new Task<AppEduProto>(
                () => new AppEduProto()
                {
                    Id = poco.Id.ToString(),
                    Applicant = poco.ToString(),
                    Major = poco.Major,
                    CertificateDiploma = poco.CertificateDiploma,
                    StartDate = poco.StartDate is null ? null : Timestamp.FromDateTime((DateTime)poco.StartDate),
                    CompletionDate = poco.CompletionDate is null ? null : Timestamp.FromDateTime((DateTime)poco.CompletionDate),
                    CompletionPercent = poco.CompletionPercent is null ? 0 : (int)poco.CompletionPercent
                });
        }
        public override Task<AppEduArray> GetAllApplicantEducation(Empty request, ServerCallContext context)
        {
            List<AppEduProto> appEduList = new List<AppEduProto>();
            List<ApplicantEducationPoco> pocos = _logic.GetAll();
            foreach(var poco in pocos)
            {
                AppEduProto appEdu  = new AppEduProto();
                appEdu.Id = poco.Id.ToString();
                appEdu.Applicant = poco.Applicant.ToString();
                appEdu.Major = poco.Major;
                appEdu.CertificateDiploma = poco.CertificateDiploma;
                appEdu.StartDate = poco.StartDate is null ? null : Timestamp.FromDateTime((DateTime)poco.StartDate);
                appEdu.CompletionDate = poco.CompletionDate is null ? null : Timestamp.FromDateTime((DateTime)poco.CompletionDate);
                appEdu.CompletionPercent = poco.CompletionPercent is null ? 0 : (int)poco.CompletionPercent;
                appEduList.Add(appEdu);
            }
            AppEduArray appEduArray = new AppEduArray();
            appEduArray.AppEdu.AddRange(appEduList);
            return new Task<AppEduArray>(() => appEduArray);
        }
        public override Task<Empty> CreateApplicantEducation(AppEduArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateApplicantEducation(AppEduArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteApplicantEducation(AppEduArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<ApplicantEducationPoco> ProtoToPoco(AppEduArray request)
        {
            List<ApplicantEducationPoco> pocos = new List<ApplicantEducationPoco>();

            var inputList = request.AppEdu.ToList();
            foreach (var item in inputList)
            {
                var poco = new ApplicantEducationPoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Applicant = Guid.Parse(item.Applicant);
                poco.Major = item.Major;
                poco.StartDate = item.StartDate.ToDateTime();
                poco.CompletionDate = item.CompletionDate.ToDateTime();
                poco.CompletionPercent = Convert.ToByte(item.CompletionPercent);
                pocos.Add(poco);
            }
            return pocos;
        }
    }
    }
