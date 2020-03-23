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
            if(poco is null)
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
        public override Task<Empty> CreateCompanyJob(ComJobProto request, ServerCallContext context)
        {
            CompanyJobPoco[] pocos = new CompanyJobPoco[1];
            foreach(var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Company = Guid.Parse(request.Company);
                poco.ProfileCreated = request.ProfileCreated.ToDateTime();
                poco.IsInactive = request.IsInactive;
                poco.IsCompanyHidden = request.IsCompanyHidden;
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateCompanyJob(ComJobProto request, ServerCallContext context)
        {
            CompanyJobPoco[] pocos = new CompanyJobPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Company = Guid.Parse(request.Company);
                poco.ProfileCreated = request.ProfileCreated.ToDateTime();
                poco.IsInactive = request.IsInactive;
                poco.IsCompanyHidden = request.IsCompanyHidden;
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyJob(ComJobProto request, ServerCallContext context)
        {
            CompanyJobPoco[] pocos = new CompanyJobPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Company = Guid.Parse(request.Company);
                poco.ProfileCreated = request.ProfileCreated.ToDateTime();
                poco.IsInactive = request.IsInactive;
                poco.IsCompanyHidden = request.IsCompanyHidden;
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
