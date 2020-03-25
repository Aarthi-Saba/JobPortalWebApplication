using CareerCloud.BusinessLogicLayer;
using CareerCloud.EntityFrameworkDataAccess;
using CareerCloud.gRPC.Protos;
using CareerCloud.Pocos;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CareerCloud.gRPC.Protos.CompanyProfile;

namespace CareerCloud.gRPC.Services
{
    public class CompanyProfileService : CompanyProfileBase
    {
        private readonly CompanyProfileLogic _logic;

        public CompanyProfileService()
        {
            _logic = new CompanyProfileLogic(new EFGenericRepository<CompanyProfilePoco>());
        }
        public override Task<CompProfileProto> GetCompanyProfile(CompProfIdRequest request, ServerCallContext context)
        {
            CompanyProfilePoco poco = _logic.Get(Guid.Parse(request.Id));
            if (poco is null)
            {
                throw new ArgumentOutOfRangeException("No such id found");
            }
            return new Task<CompProfileProto>(
                () => new CompProfileProto
                {
                    Id = poco.Id.ToString(),
                    RegistrationDate = Timestamp.FromDateTime(poco.RegistrationDate),
                    CompanyWebsite = poco.CompanyWebsite,
                    ContactName = poco.ContactName,
                    CompanyLogo = ByteString.CopyFrom(poco.CompanyLogo),
                    ContactPhone = poco.ContactPhone
                });
        }
        public override Task<CompProfileArray> GetAllCompanyProfile(Empty request, ServerCallContext context)
        {
            List<CompanyProfilePoco> pocos = _logic.GetAll();
            List<CompProfileProto> compProfileList = new List<CompProfileProto>();
            foreach (var poco in pocos)
            {
                CompProfileProto compProfile = new CompProfileProto();
                compProfile.Id = poco.Id.ToString();
                compProfile.RegistrationDate = Timestamp.FromDateTime(poco.RegistrationDate);
                compProfile.CompanyWebsite = poco.CompanyWebsite;
                compProfile.ContactName = poco.ContactName;
                compProfile.CompanyLogo = ByteString.CopyFrom(poco.CompanyLogo);
                compProfile.ContactPhone = poco.ContactPhone;
                compProfileList.Add(compProfile);
            }
            CompProfileArray compProfileArray = new CompProfileArray();
            compProfileArray.CompProfile.AddRange(compProfileList);
            return new Task<CompProfileArray>(() => compProfileArray);
        }
        public override Task<Empty> CreateCompanyProfile(CompProfileArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateCompanyProfile(CompProfileArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyProfile(CompProfileArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }

        public List<CompanyProfilePoco> ProtoToPoco(CompProfileArray request)
        {
            List<CompanyProfilePoco> pocos = new List<CompanyProfilePoco>();
            var inputList = request.CompProfile.ToList();
            foreach (var item in inputList)
            {
                var poco = new CompanyProfilePoco();
                poco.Id = Guid.Parse(item.Id);
                poco.RegistrationDate = item.RegistrationDate.ToDateTime();
                poco.CompanyWebsite = item.CompanyWebsite;
                poco.ContactName = item.ContactName;
                poco.ContactPhone = item.ContactPhone;
                poco.CompanyLogo = item.CompanyLogo.ToByteArray();
                pocos.Add(poco);
            }
            return pocos;

        }
    }
}
