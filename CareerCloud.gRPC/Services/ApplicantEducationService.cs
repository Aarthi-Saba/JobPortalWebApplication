using CareerCloud.BusinessLogicLayer;
using CareerCloud.EntityFrameworkDataAccess;
using CareerCloud.gRPC.Protos;
using CareerCloud.Pocos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
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
                    CompletionDate = poco.CompletionDate is null ? null : Timestamp.FromDateTime((DateTime) poco.CompletionDate),
                    CompletionPercent = poco.CompletionPercent is null ? 0 : (int)poco.CompletionPercent
                }) ;
        }
        public override Task<Empty> CreateApplicantEducation(AppEduProto request, ServerCallContext context)
        {
            ApplicantEducationPoco[] pocos = new ApplicantEducationPoco[1];
            foreach(var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Applicant = Guid.Parse(request.Applicant);
                poco.Major = request.Major;
                poco.StartDate = request.StartDate.ToDateTime();
                poco.CompletionDate = request.CompletionDate.ToDateTime();
                poco.CompletionPercent = Convert.ToByte(request.CompletionPercent);

            }            
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateApplicantEducation(AppEduProto request, ServerCallContext context)
        {
            ApplicantEducationPoco[] pocos = new ApplicantEducationPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Applicant = Guid.Parse(request.Applicant);
                poco.Major = request.Major;
                poco.StartDate = request.StartDate.ToDateTime();
                poco.CompletionDate = request.CompletionDate.ToDateTime();
                poco.CompletionPercent = Convert.ToByte(request.CompletionPercent);
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteApplicantEducation(AppEduProto request, ServerCallContext context)
        {
            ApplicantEducationPoco[] pocos = new ApplicantEducationPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Applicant = Guid.Parse(request.Applicant);
                poco.Major = request.Major;
                poco.StartDate = request.StartDate.ToDateTime();
                poco.CompletionDate = request.CompletionDate.ToDateTime();
                poco.CompletionPercent = Convert.ToByte(request.CompletionPercent);

            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
