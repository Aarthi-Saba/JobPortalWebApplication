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
        public override Task<CompDescArray> GetAllCompanyDescription(Empty request, ServerCallContext context)
        {
            List<CompanyDescriptionPoco> pocos = _logic.GetAll();
            List<CompDescProto> compDescList = new List<CompDescProto>();
            foreach(var poco in pocos)
            {
                CompDescProto compDesc = new CompDescProto();
                compDesc.Id = poco.Id.ToString();
                compDesc.Company = poco.Company.ToString();
                compDesc.LanguageId = poco.LanguageId;
                compDesc.CompanyName = poco.CompanyName;
                compDesc.CompanyDescription = poco.CompanyDescription;
                compDescList.Add(compDesc);
            }
            CompDescArray compDescArray = new CompDescArray();
            compDescArray.CompDesc.AddRange(compDescList);
            return new Task<CompDescArray>(() => compDescArray);
        }
        public override Task<Empty> CreateCompanyDescription(CompDescArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateCompanyDescription(CompDescArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyDescription(CompDescArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<CompanyDescriptionPoco> ProtoToPoco(CompDescArray request)
        {
            List<CompanyDescriptionPoco> pocos = new List<CompanyDescriptionPoco>();
            var inputList = request.CompDesc.ToList();
            foreach (var item in inputList)
            {
                var poco = new CompanyDescriptionPoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Company = Guid.Parse(item.Company);
                poco.LanguageId = item.LanguageId;
                poco.CompanyName = item.CompanyName;
                poco.CompanyDescription = item.CompanyDescription;
                pocos.Add(poco);
            }
            return pocos;
        }
    }
}
