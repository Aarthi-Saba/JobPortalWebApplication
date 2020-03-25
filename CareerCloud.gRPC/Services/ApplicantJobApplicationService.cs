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
            if (poco is null)
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
        public override Task<AppJobArray> GetAllApplicantJobApplication(Empty request, ServerCallContext context)
        {
            List<AppJobProto> appjoblist = new List<AppJobProto>();
            List<ApplicantJobApplicationPoco> pocos = _logic.GetAll();
            foreach (var poco in pocos)
            {
                AppJobProto appjob = new AppJobProto();
                appjob.Id = poco.Id.ToString();
                appjob.Applicant = poco.Applicant.ToString();
                appjob.Job = poco.Job.ToString();
                appjob.ApplicationDate = Timestamp.FromDateTime((DateTime)poco.ApplicationDate);
                appjoblist.Add(appjob);
            }
            AppJobArray appjobarray = new AppJobArray();
            appjobarray.AppJob.AddRange(appjoblist);
            return new Task<AppJobArray>(() => appjobarray); 
        }
        public override Task<Empty> CreateApplicantJobApplication(AppJobArray request, ServerCallContext context)
        {
            var pocos = PrototoPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateApplicantJobApplication(AppJobArray request, ServerCallContext context)
        {
            var pocos = PrototoPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteApplicantJobApplication(AppJobArray request, ServerCallContext context)
        {
            var pocos = PrototoPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<ApplicantJobApplicationPoco> PrototoPoco(AppJobArray request)
        {
            List<ApplicantJobApplicationPoco> pocos = new List<ApplicantJobApplicationPoco>();

            var inputlist = request.AppJob.ToList();
            foreach (var item in inputlist)
            {
                var poco = new ApplicantJobApplicationPoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Applicant = Guid.Parse(item.Applicant);
                poco.Job = Guid.Parse(item.Job);
                poco.ApplicationDate = item.ApplicationDate.ToDateTime();
                pocos.Add(poco);
            }
            return pocos;
        }
    }
}
