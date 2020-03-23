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
            if(poco is null)
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
        public override Task<Empty> CreateCompanyProfile(CompProfileProto request, ServerCallContext context)
        {
            CompanyProfilePoco[] pocos = new CompanyProfilePoco[1];
            foreach(var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.RegistrationDate = request.RegistrationDate.ToDateTime();
                poco.CompanyWebsite = request.CompanyWebsite;
                poco.ContactName = request.ContactName;
                poco.ContactPhone = request.ContactPhone;
                poco.CompanyLogo = request.CompanyLogo.ToByteArray();
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateCompanyProfile(CompProfileProto request, ServerCallContext context)
        {
            CompanyProfilePoco[] pocos = new CompanyProfilePoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.RegistrationDate = request.RegistrationDate.ToDateTime();
                poco.CompanyWebsite = request.CompanyWebsite;
                poco.ContactName = request.ContactName;
                poco.ContactPhone = request.ContactPhone;
                poco.CompanyLogo = request.CompanyLogo.ToByteArray();
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyProfile(CompProfileProto request, ServerCallContext context)
        {
            CompanyProfilePoco[] pocos = new CompanyProfilePoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.RegistrationDate = request.RegistrationDate.ToDateTime();
                poco.CompanyWebsite = request.CompanyWebsite;
                poco.ContactName = request.ContactName;
                poco.ContactPhone = request.ContactPhone;
                poco.CompanyLogo = request.CompanyLogo.ToByteArray();
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
