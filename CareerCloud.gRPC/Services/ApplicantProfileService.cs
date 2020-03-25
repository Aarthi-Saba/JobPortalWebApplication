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
        public override Task<AppProfileProto> GetApplicantProfile(AppProfIdRequest request, ServerCallContext context)
        {
            ApplicantProfilePoco poco = _logic.Get(Guid.Parse(request.Id));
            if (poco is null)
            {
                throw new ArgumentOutOfRangeException("Id entered in input not found in system");
            }
            return new Task<AppProfileProto>(
                () => new AppProfileProto
                {
                    Id = poco.Id.ToString(),
                    Login = poco.Login.ToString(),
                    CurrentSalary = (Protos.Decimal)poco.CurrentSalary,
                    CurrentRate = (Protos.Decimal)poco.CurrentRate,
                    Currency = poco.Currency,
                    Country = poco.Country,
                    Province = poco.Province,
                    Street = poco.Street,
                    City = poco.City,
                    PostalCode = poco.PostalCode
                });
        }
        public override Task<AppProfileArray> GetAllApplicantProfile(Empty request, ServerCallContext context)
        {
            List<AppProfileProto> appProfileList = new List<AppProfileProto>();
            List<ApplicantProfilePoco> pocos = _logic.GetAll();
            foreach(var poco in pocos)
            {
                AppProfileProto appProfile = new AppProfileProto();
                appProfile.Id = poco.Id.ToString();
                appProfile.Login = poco.Login.ToString();
                appProfile.CurrentSalary = (Protos.Decimal)poco.CurrentSalary;
                appProfile.CurrentRate = (Protos.Decimal)poco.CurrentRate;
                appProfile.Currency = poco.Currency;
                appProfile.Country = poco.Country;
                appProfile.Province = poco.Province;
                appProfile.Street = poco.Street;
                appProfile.City = poco.City;
                appProfile.PostalCode = poco.PostalCode;
            }
            AppProfileArray appProfileArray = new AppProfileArray();
            appProfileArray.AppProfile.AddRange(appProfileList);
            return new Task<AppProfileArray>(() => appProfileArray);
        }
        public override Task<Empty> CreateApplicantProfile(AppProfileArray request, ServerCallContext context)
        {
            var pocos = ProtosToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateApplicantProfile(AppProfileArray request, ServerCallContext context)
        {
            var pocos = ProtosToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteApplicantProfile(AppProfileArray request, ServerCallContext context)
        {
            var pocos = ProtosToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<ApplicantProfilePoco> ProtosToPoco(AppProfileArray request)
        {
            List<ApplicantProfilePoco> pocos = new List<ApplicantProfilePoco>();
            var inputList = request.AppProfile.ToList();
            foreach (var item in inputList)
            {
                var poco = new ApplicantProfilePoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Login = Guid.Parse(item.Login);
                poco.CurrentSalary = item.CurrentSalary;
                poco.CurrentRate = item.CurrentRate;
                poco.Currency = item.Currency;
                poco.Country = item.Country;
                poco.Province = item.Province;
                poco.Street = item.Street;
                poco.City = item.City;
                poco.PostalCode = item.PostalCode;
            }
            return pocos;
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