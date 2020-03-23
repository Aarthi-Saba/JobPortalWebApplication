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
using static CareerCloud.gRPC.Protos.CompanyJobDescription;

namespace CareerCloud.gRPC.Services
{
    public class CompanyJobDescriptionService : CompanyJobDescriptionBase
    {
        private readonly CompanyJobDescriptionLogic _logic;

        public CompanyJobDescriptionService()
        {
            _logic = new CompanyJobDescriptionLogic(new EFGenericRepository<CompanyJobDescriptionPoco>());
        }
        public override Task<CompJobDescProto> GetCompanyJobDescription(CompJobDescIdRequest request, ServerCallContext context)
        {
            CompanyJobDescriptionPoco poco = _logic.Get(Guid.Parse(request.Id));
            if (poco is null)
            {
                throw new ArgumentOutOfRangeException("Id in input not found");
            }
            return new Task<CompJobDescProto>(
                () => new CompJobDescProto
                {
                    Id = poco.Id.ToString(),
                    Job = poco.Job.ToString(),
                    JobName = poco.JobName,
                    JobDescriptions = poco.JobDescriptions
                });
        }
        public override Task<Empty> CreateCompanyJobDescription(CompJobDescProto request, ServerCallContext context)
        {
            CompanyJobDescriptionPoco[] pocos = new CompanyJobDescriptionPoco[1];
            foreach(var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Job = Guid.Parse(request.Job);
                poco.JobName = request.JobName;
                poco.JobDescriptions = request.JobDescriptions;
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateCompanyJobDescription(CompJobDescProto request, ServerCallContext context)
        {
            CompanyJobDescriptionPoco[] pocos = new CompanyJobDescriptionPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Job = Guid.Parse(request.Job);
                poco.JobName = request.JobName;
                poco.JobDescriptions = request.JobDescriptions;
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyJobDescription(CompJobDescProto request, ServerCallContext context)
        {
            CompanyJobDescriptionPoco[] pocos = new CompanyJobDescriptionPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Job = Guid.Parse(request.Job);
                poco.JobName = request.JobName;
                poco.JobDescriptions = request.JobDescriptions;
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
