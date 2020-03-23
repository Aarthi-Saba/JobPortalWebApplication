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
using static CareerCloud.gRPC.Protos.ApplicantJobApplication;

namespace CareerCloud.gRPC.Services
{
    public class ApplicantJobApplicationService : ApplicantJobApplicationBase
    {
        private readonly ApplicantJobApplicationLogic _logic;

        public ApplicantJobApplicationService()
        {
            _logic = new ApplicantJobApplicationLogic(new EFGenericRepository<ApplicantJobApplicationPoco>());
        }
        public override Task<AppJobProto> GetApplicantJobApplication(AppJobIdRequest request, ServerCallContext context)
        {
            ApplicantJobApplicationPoco poco = _logic.Get(Guid.Parse(request.Id));
            if(poco is null)
            {
                throw new ArgumentOutOfRangeException("Id entered in input not found in system");
            }
            return new Task<AppJobProto>(
                () => new AppJobProto()
                {
                    Id = poco.Id.ToString(),
                    Applicant = poco.Applicant.ToString(),
                    Job = poco.Job.ToString(),
                    ApplicationDate = Timestamp.FromDateTime((DateTime)poco.ApplicationDate)
                });
        }
        public override Task<Empty> CreateApplicantJobApplication(AppJobProto request, ServerCallContext context)
        {
            ApplicantJobApplicationPoco[] pocos = new ApplicantJobApplicationPoco[1];
            foreach(var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Applicant = Guid.Parse(request.Applicant);
                poco.Job = Guid.Parse(request.Job);
                poco.ApplicationDate = request.ApplicationDate.ToDateTime();
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateApplicantJobApplication(AppJobProto request, ServerCallContext context)
        {
            ApplicantJobApplicationPoco[] pocos = new ApplicantJobApplicationPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Applicant = Guid.Parse(request.Applicant);
                poco.Job = Guid.Parse(request.Job);
                poco.ApplicationDate = request.ApplicationDate.ToDateTime();
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteApplicantJobApplication(AppJobProto request, ServerCallContext context)
        {
            ApplicantJobApplicationPoco[] pocos = new ApplicantJobApplicationPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Applicant = Guid.Parse(request.Applicant);
                poco.Job = Guid.Parse(request.Job);
                poco.ApplicationDate = request.ApplicationDate.ToDateTime();
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
