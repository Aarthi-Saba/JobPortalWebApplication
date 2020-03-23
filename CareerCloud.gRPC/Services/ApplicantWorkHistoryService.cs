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
using static CareerCloud.gRPC.Protos.ApplicantWorkHistory;

namespace CareerCloud.gRPC.Services
{
    public class ApplicantWorkHistoryService : ApplicantWorkHistoryBase
    {
        private readonly ApplicantWorkHistoryLogic _logic;

        public ApplicantWorkHistoryService()
        {
            _logic = new ApplicantWorkHistoryLogic(new EFGenericRepository<ApplicantWorkHistoryPoco>());
        }
        public override Task<AppWorkProto> GetApplicantWorkHistory(AppWorkHistIdRequest request, ServerCallContext context)
        {
            ApplicantWorkHistoryPoco poco = _logic.Get(Guid.Parse(request.Id));
            if(poco is null)
            {
                throw new ArgumentOutOfRangeException("Id in input not found");
            }
            return new Task<AppWorkProto>(
                () => new AppWorkProto
                {
                    Id = poco.Id.ToString(),
                    Applicant = poco.Applicant.ToString(),
                    CompanyName = poco.CompanyName,
                    CountryCode = poco.CountryCode,
                    Location = poco.Location,
                    JobTitle = poco.JobTitle,
                    JobDescription = poco.JobDescription,
                    StartMonth = Convert.ToInt32(poco.StartMonth),
                    StartYear = poco.StartYear,
                    EndMonth = Convert.ToInt32(poco.EndMonth),
                    EndYear = poco.EndYear
                });
        }
        public override Task<Empty> CreateApplicantWorkHistory(AppWorkProto request, ServerCallContext context)
        {
            ApplicantWorkHistoryPoco[] pocos = new ApplicantWorkHistoryPoco[1];
            foreach(var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Applicant = Guid.Parse(request.Applicant);
                poco.CompanyName = request.CompanyName;
                poco.CountryCode = request.CountryCode;
                poco.Location = request.Location;
                poco.JobTitle = request.JobTitle;
                poco.JobDescription = request.JobDescription;
                poco.StartMonth = Convert.ToInt16(request.StartMonth);
                poco.StartYear = request.StartYear;
                poco.EndMonth = Convert.ToInt16(request.EndMonth);
                poco.EndYear = request.EndYear;
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateApplicantWorkHistory(AppWorkProto request, ServerCallContext context)
        {
            ApplicantWorkHistoryPoco[] pocos = new ApplicantWorkHistoryPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Applicant = Guid.Parse(request.Applicant);
                poco.CompanyName = request.CompanyName;
                poco.CountryCode = request.CountryCode;
                poco.Location = request.Location;
                poco.JobTitle = request.JobTitle;
                poco.JobDescription = request.JobDescription;
                poco.StartMonth = Convert.ToInt16(request.StartMonth);
                poco.StartYear = request.StartYear;
                poco.EndMonth = Convert.ToInt16(request.EndMonth);
                poco.EndYear = request.EndYear;
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteApplicantWorkHistory(AppWorkProto request, ServerCallContext context)
        {
            ApplicantWorkHistoryPoco[] pocos = new ApplicantWorkHistoryPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Applicant = Guid.Parse(request.Applicant);
                poco.CompanyName = request.CompanyName;
                poco.CountryCode = request.CountryCode;
                poco.Location = request.Location;
                poco.JobTitle = request.JobTitle;
                poco.JobDescription = request.JobDescription;
                poco.StartMonth = Convert.ToInt16(request.StartMonth);
                poco.StartYear = request.StartYear;
                poco.EndMonth = Convert.ToInt16(request.EndMonth);
                poco.EndYear = request.EndYear;
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
