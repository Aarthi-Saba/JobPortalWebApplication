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
using static CareerCloud.gRPC.Protos.CompanyJob;

namespace CareerCloud.gRPC.Services
{
    public class CompanyJobService : CompanyJobBase
    {
        private readonly CompanyJobLogic _logic;

        public CompanyJobService()
        {
            _logic = new CompanyJobLogic(new EFGenericRepository<CompanyJobPoco>());
        }
        public override Task<ComJobProto> GetCompanyJob(ComJobIdRequest request, ServerCallContext context)
        {
            CompanyJobPoco poco = _logic.Get(Guid.Parse(request.Id));
            if (poco is null)
            {
                throw new ArgumentOutOfRangeException("No such Id exist !");
            }
            return new Task<ComJobProto>(
                () => new ComJobProto
                {
                    Id = poco.Id.ToString(),
                    Company = poco.Company.ToString(),
                    ProfileCreated = Timestamp.FromDateTime(poco.ProfileCreated),
                    IsInactive = poco.IsInactive,
                    IsCompanyHidden = poco.IsCompanyHidden
                });
        }
        public override Task<ComJobArray> GetAllCompanyJob(Empty request, ServerCallContext context)
        {
            List<CompanyJobPoco> pocos = _logic.GetAll();
            List<ComJobProto> compJobList = new List<ComJobProto>();
            foreach (var poco in pocos)
            {
                var comJob = new ComJobProto();
                comJob.Id = poco.Id.ToString();
                comJob.Company = poco.Company.ToString();
                comJob.ProfileCreated = Timestamp.FromDateTime(poco.ProfileCreated);
                comJob.IsInactive = poco.IsInactive;
                comJob.IsCompanyHidden = poco.IsCompanyHidden;
                compJobList.Add(comJob);
            }
            ComJobArray comJobArray = new ComJobArray();
            comJobArray.ComJob.AddRange(compJobList);
            return new Task<ComJobArray>(() => comJobArray);
        }
        public override Task<Empty> CreateCompanyJob(ComJobArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateCompanyJob(ComJobArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyJob(ComJobArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<CompanyJobPoco> ProtoToPoco(ComJobArray request)
        {
            List<CompanyJobPoco> pocos = new List<CompanyJobPoco>();
            var inputList = request.ComJob.ToList();
            foreach (var item in inputList)
            {
                var poco = new CompanyJobPoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Company = Guid.Parse(item.Company);
                poco.ProfileCreated = item.ProfileCreated.ToDateTime();
                poco.IsInactive = item.IsInactive;
                poco.IsCompanyHidden = item.IsCompanyHidden;
                pocos.Add(poco);
            }
            return pocos;
        }
 
    }
}
