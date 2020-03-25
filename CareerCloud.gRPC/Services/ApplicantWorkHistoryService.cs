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
        public override Task<AppWorkHistoryArray> GetAllApplicantWorkHistory(Empty request, ServerCallContext context)
        {
            List<AppWorkProto> appWorkList = new List<AppWorkProto>();
            List<ApplicantWorkHistoryPoco> pocos = _logic.GetAll();
            foreach (var poco in pocos)
            {
                AppWorkProto appWork = new AppWorkProto();
                appWork.Id = poco.Id.ToString();
                appWork.Applicant = poco.Applicant.ToString();
                appWork.CompanyName = poco.CompanyName;
                appWork.CountryCode = poco.CountryCode;
                appWork.Location = poco.Location;
                appWork.JobTitle = poco.JobTitle;
                appWork.JobDescription = poco.JobDescription;
                appWork.StartMonth = Convert.ToInt32(poco.StartMonth);
                appWork.StartYear = poco.StartYear;
                appWork.EndMonth = Convert.ToInt32(poco.EndMonth);
                appWork.EndYear = poco.EndYear;
                appWorkList.Add(appWork);
            }
            AppWorkHistoryArray appWorkArray = new AppWorkHistoryArray();
            appWorkArray.AppWork.AddRange(appWorkList);
            return new Task<AppWorkHistoryArray>(() => appWorkArray);
        }
        public override Task<Empty> CreateApplicantWorkHistory(AppWorkHistoryArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateApplicantWorkHistory(AppWorkHistoryArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteApplicantWorkHistory(AppWorkHistoryArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<ApplicantWorkHistoryPoco> ProtoToPoco(AppWorkHistoryArray request)
        {
            List<ApplicantWorkHistoryPoco> pocos = new List<ApplicantWorkHistoryPoco>();
            var inputList = request.AppWork.ToList();
            foreach (var item in inputList)
            {
                ApplicantWorkHistoryPoco poco = new ApplicantWorkHistoryPoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Applicant = Guid.Parse(item.Applicant);
                poco.CompanyName = item.CompanyName;
                poco.CountryCode = item.CountryCode;
                poco.Location = item.Location;
                poco.JobTitle = item.JobTitle;
                poco.JobDescription = item.JobDescription;
                poco.StartMonth = Convert.ToInt16(item.StartMonth);
                poco.StartYear = item.StartYear;
                poco.EndMonth = Convert.ToInt16(item.EndMonth);
                poco.EndYear = item.EndYear;
                pocos.Add(poco);
            }
            return pocos;
        }

    }
}
