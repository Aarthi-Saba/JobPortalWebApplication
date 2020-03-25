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
            if (poco is null)
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
        public override Task<CompLocArray> GetAllCompanyLocation(Empty request, ServerCallContext context)
        {
            List<CompanyLocationPoco> pocos = _logic.GetAll();
            List<CompLocProto> compLocList = new List<CompLocProto>();
            foreach (var poco in pocos)
            {
                CompLocProto compLoc = new CompLocProto();
                compLoc.Id = poco.Id.ToString();
                compLoc.Company = poco.Company.ToString();
                compLoc.PostalCode = poco.PostalCode;
                compLoc.Province = poco.Province;
                compLoc.Street = poco.Street;
                compLoc.City = poco.City;
                compLoc.CountryCode = poco.CountryCode;
                compLocList.Add(compLoc);
            }
            CompLocArray compLocArray = new CompLocArray();
            compLocArray.CompLoc.AddRange(compLocList);
            return new Task<CompLocArray>(() => compLocArray);
        }
        public override Task<Empty> CreateCompanyLocation(CompLocArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateCompanyLocation(CompLocArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyLocation(CompLocArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<CompanyLocationPoco> ProtoToPoco(CompLocArray request)
        {
            List<CompanyLocationPoco> pocos = new List<CompanyLocationPoco>();
            var inputList = request.CompLoc.ToList();
            foreach (var item in inputList)
            {
                var poco = new CompanyLocationPoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Company = Guid.Parse(item.Company);
                poco.City = item.City;
                poco.PostalCode = item.PostalCode;
                poco.Province = item.Province;
                poco.Street = item.Street;
                poco.CountryCode = item.CountryCode;
                pocos.Add(poco);
            }
            return pocos;
        }
    }
}
