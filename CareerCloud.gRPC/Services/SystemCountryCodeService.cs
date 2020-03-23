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
using static CareerCloud.gRPC.Protos.SystemCountryCode;

namespace CareerCloud.gRPC.Services
{
    public class SystemCountryCodeService : SystemCountryCodeBase
    {
        private readonly SystemCountryCodeLogic _logic;

        public SystemCountryCodeService()
        {
            _logic = new SystemCountryCodeLogic(new EFGenericRepository<SystemCountryCodePoco>());
        }
        public override Task<SysCountryCodeProto> GetSystemCountryCode(SysCountryIdRequest request, ServerCallContext context)
        {
            SystemCountryCodePoco poco = _logic.Get(request.Id);
            if(poco is null)
            {
                throw new ArgumentOutOfRangeException("No such Country code exists");
            }
            return new Task<SysCountryCodeProto>(
                () => new SysCountryCodeProto
                {
                    Code = poco.Code,
                    Name = poco.Name
                });
        }
        public override Task<Empty> CreateSystemCountryCode(SysCountryCodeProto request, ServerCallContext context)
        {
            SystemCountryCodePoco[] pocos = new SystemCountryCodePoco[1];
            foreach(var poco in pocos)
            {
                poco.Code = request.Code;
                poco.Name = request.Name;
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());            
        }
        public override Task<Empty> UpdateSystemCountryCode(SysCountryCodeProto request, ServerCallContext context)
        {
            SystemCountryCodePoco[] pocos = new SystemCountryCodePoco[1];
            foreach (var poco in pocos)
            {
                poco.Code = request.Code;
                poco.Name = request.Name;
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteSystemCountryCode(SysCountryCodeProto request, ServerCallContext context)
        {
            SystemCountryCodePoco[] pocos = new SystemCountryCodePoco[1];
            foreach (var poco in pocos)
            {
                poco.Code = request.Code;
                poco.Name = request.Name;
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
