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
using static CareerCloud.gRPC.Protos.CompanyLocation;

namespace CareerCloud.gRPC.Services
{
    public class CompanyLocationService : CompanyLocationBase
    {
        private readonly CompanyLocationLogic _logic;

        public CompanyLocationService()
        {
            _logic = new CompanyLocationLogic(new EFGenericRepository<CompanyLocationPoco>());
        }
        public override Task<CompLocProto> GetCompanyLocation(CompLocIdRequest request, ServerCallContext context)
        {
            CompanyLocationPoco poco = _logic.Get(Guid.Parse(request.Id));
            if(poco is null)
            {
                throw new ArgumentOutOfRangeException("Id does not exist");
            }
            return new Task<CompLocProto>(
                () => new CompLocProto
                {
                    Id = poco.Id.ToString(),
                    Company = poco.Company.ToString(),
                    PostalCode = poco.PostalCode,
                    Province = poco.Province,
                    Street = poco.Street,
                    City = poco.City,
                    CountryCode = poco.CountryCode
                });
        }
        public override Task<Empty> CreateCompanyLocation(CompLocProto request, ServerCallContext context)
        {
            CompanyLocationPoco[] pocos = new CompanyLocationPoco[1];
            foreach(var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Company = Guid.Parse(request.Company);
                poco.City = request.City;
                poco.PostalCode = request.PostalCode;
                poco.Province = request.Province;
                poco.Street = request.Street;
                poco.CountryCode = request.CountryCode;
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateCompanyLocation(CompLocProto request, ServerCallContext context)
        {
            CompanyLocationPoco[] pocos = new CompanyLocationPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Company = Guid.Parse(request.Company);
                poco.City = request.City;
                poco.PostalCode = request.PostalCode;
                poco.Province = request.Province;
                poco.Street = request.Street;
                poco.CountryCode = request.CountryCode;
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyLocation(CompLocProto request, ServerCallContext context)
        {
            CompanyLocationPoco[] pocos = new CompanyLocationPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Company = Guid.Parse(request.Company);
                poco.City = request.City;
                poco.PostalCode = request.PostalCode;
                poco.Province = request.Province;
                poco.Street = request.Street;
                poco.CountryCode = request.CountryCode;
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
