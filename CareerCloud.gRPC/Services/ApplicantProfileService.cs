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
using static CareerCloud.gRPC.Protos.ApplicantProfile;

namespace CareerCloud.gRPC.Services
{
    public class ApplicantProfileService : ApplicantProfileBase
    {
        private readonly ApplicantProfileLogic _logic;

        public ApplicantProfileService()
        {
            _logic = new ApplicantProfileLogic(new EFGenericRepository<ApplicantProfilePoco>());
        }
        public override Task<AppProfProto> GetApplicantProfile(AppProfIdRequest request, ServerCallContext context)
        {
            ApplicantProfilePoco poco = _logic.Get(Guid.Parse(request.Id));
            if(poco is null)
            {
                throw new ArgumentOutOfRangeException("Id entered in input not found in system");
            }
            Protos.Decimal grpcsalary = poco.CurrentSalary;
            Protos.Decimal grpcrate = poco.CurrentRate;
            return new Task<AppProfProto>(
                () => new AppProfProto
                {
                    Id = poco.Id.ToString(),
                    Login = poco.Login.ToString(),
                    CurrentSalary = grpcsalary,
                    CurrentRate = grpcrate,
                    Currency = poco.Currency,
                    Country = poco.Country,
                    Province = poco.Province,
                    Street = poco.Street,
                    City = poco.City,
                    PostalCode = poco.PostalCode
                });
        }
        public override Task<Empty> CreateApplicantProfile(AppProfProto request, ServerCallContext context)
        {
            ApplicantProfilePoco[] pocos = new ApplicantProfilePoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Login = Guid.Parse(request.Login);
                poco.CurrentSalary = request.CurrentSalary;
                poco.CurrentRate =request.CurrentRate;
                poco.Currency = request.Currency;
                poco.Country = request.Country;
                poco.Province = request.Province;
                poco.Street = request.Street;
                poco.City = request.City;
                poco.PostalCode = request.PostalCode;
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }

        public override Task<Empty> UpdateApplicantProfile(AppProfProto request, ServerCallContext context)
        {
            ApplicantProfilePoco[] pocos = new ApplicantProfilePoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Login = Guid.Parse(request.Login);
                poco.CurrentSalary = request.CurrentSalary;
                poco.CurrentRate = request.CurrentRate;
                poco.Currency = request.Currency;
                poco.Country = request.Country;
                poco.Province = request.Province;
                poco.Street = request.Street;
                poco.City = request.City;
                poco.PostalCode = request.PostalCode;
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteApplicantProfile(AppProfProto request, ServerCallContext context)
        {
            ApplicantProfilePoco[] pocos = new ApplicantProfilePoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Login = Guid.Parse(request.Login);
                poco.CurrentSalary = request.CurrentSalary;
                poco.CurrentRate = request.CurrentRate;
                poco.Currency = request.Currency;
                poco.Country = request.Country;
                poco.Province = request.Province;
                poco.Street = request.Street;
                poco.City = request.City;
                poco.PostalCode = request.PostalCode;
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
namespace CareerCloud.gRPC.Protos
{
    public partial class Decimal
    {
        private const decimal NanoFactor = 1_000_000_000;
        public Decimal(long units, int nanos)
        {
            grpcUnits = units;
            grpcNanos = nanos;
        }
        public long grpcUnits { get; }
        public int grpcNanos { get; }

        public static implicit operator decimal(CareerCloud.gRPC.Protos.Decimal Decimal)
        {
            return Decimal.grpcUnits + Decimal.grpcNanos / NanoFactor;
        }

        public static implicit operator CareerCloud.gRPC.Protos.Decimal(decimal value)
        {
            var units = decimal.ToInt64(value);
            var nanos = decimal.ToInt32((value - units) * NanoFactor);
            return new CareerCloud.gRPC.Protos.Decimal(units, nanos);
        }
    }
}