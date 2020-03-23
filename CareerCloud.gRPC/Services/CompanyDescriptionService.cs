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
using static CareerCloud.gRPC.Protos.CompanyDescription;

namespace CareerCloud.gRPC.Services
{
    public class CompanyDescriptionService : CompanyDescriptionBase
    {
        private readonly CompanyDescriptionLogic _logic;

        public CompanyDescriptionService()
        {
            _logic = new CompanyDescriptionLogic(new EFGenericRepository<CompanyDescriptionPoco>());
        }
        public override Task<CompDescProto> GetCompanyDescription(CompDescIdRequest request, ServerCallContext context)
        {
            CompanyDescriptionPoco poco = _logic.Get(Guid.Parse(request.Id));
            if (poco is null)
            {
                throw new ArgumentOutOfRangeException("Id in input not found");
            }
            return new Task<CompDescProto>(
                () => new CompDescProto
                {
                    Id = poco.Id.ToString(),
                    Company = poco.Company.ToString(),
                    LanguageId = poco.LanguageId,
                    CompanyName = poco.CompanyName,
                    CompanyDescription = poco.CompanyDescription
        });
        }
        public override Task<Empty> CreateCompanyDescription(CompDescProto request, ServerCallContext context)
        {
            CompanyDescriptionPoco[] pocos = new CompanyDescriptionPoco[1];
            foreach(var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Company = Guid.Parse(request.Company);
                poco.LanguageId = request.LanguageId;
                poco.CompanyName = request.CompanyName;
                poco.CompanyDescription = request.CompanyDescription;
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty()) ;
        }
        public override Task<Empty> UpdateCompanyDescription(CompDescProto request, ServerCallContext context)
        {
            CompanyDescriptionPoco[] pocos = new CompanyDescriptionPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Company = Guid.Parse(request.Company);
                poco.LanguageId = request.LanguageId;
                poco.CompanyName = request.CompanyName;
                poco.CompanyDescription = request.CompanyDescription;
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyDescription(CompDescProto request, ServerCallContext context)
        {
            CompanyDescriptionPoco[] pocos = new CompanyDescriptionPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Company = Guid.Parse(request.Company);
                poco.LanguageId = request.LanguageId;
                poco.CompanyName = request.CompanyName;
                poco.CompanyDescription = request.CompanyDescription;
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
