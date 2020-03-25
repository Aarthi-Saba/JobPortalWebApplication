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
        public override Task<CompJobDescArray> GetAllCompanyDescription(Empty request, ServerCallContext context)
        {
            List<CompanyJobDescriptionPoco> pocos = _logic.GetAll();
            List<CompJobDescProto> compJobDescList = new List<CompJobDescProto>();
            foreach (var poco in pocos)
            {
                CompJobDescProto compJobDesc = new CompJobDescProto();
                compJobDesc.Id = poco.Id.ToString();
                compJobDesc.Job = poco.Job.ToString();
                compJobDesc.JobName = poco.JobName;
                compJobDesc.JobDescriptions = poco.JobDescriptions;
                compJobDescList.Add(compJobDesc);
            }
            CompJobDescArray compJobDescArray = new CompJobDescArray();
            compJobDescArray.CompJobDesc.AddRange(compJobDescList);
            return new Task<CompJobDescArray>(() => compJobDescArray);
        }
        public override Task<Empty> CreateCompanyJobDescription(CompJobDescArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateCompanyJobDescription(CompJobDescArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyJobDescription(CompJobDescArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<CompanyJobDescriptionPoco> ProtoToPoco(CompJobDescArray request)
        {
            List<CompanyJobDescriptionPoco> pocos = new List<CompanyJobDescriptionPoco>();
            var inputList = request.CompJobDesc.ToList();
            foreach (var item in inputList)
            {
                var poco = new CompanyJobDescriptionPoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Job = Guid.Parse(item.Job);
                poco.JobName = item.JobName;
                poco.JobDescriptions = item.JobDescriptions;
                pocos.Add(poco);
            }
            return pocos;
        }
    }
}
