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
        public override Task<SysCountryCodeArray> GetAllSystemCountryCode(Empty request, ServerCallContext context)
        {
            List<SystemCountryCodePoco> pocos = _logic.GetAll();
            List<SysCountryCodeProto> sysCountryCodeList = new List<SysCountryCodeProto>();
            foreach (var poco in pocos)
            {
                SysCountryCodeProto sysCountryCode = new SysCountryCodeProto();
                sysCountryCode.Code = poco.Code;
                sysCountryCode.Name = poco.Name;
                sysCountryCodeList.Add(sysCountryCode);
            }
            SysCountryCodeArray sysCountryCodeArray = new SysCountryCodeArray();
            sysCountryCodeArray.SysCountryCode.AddRange(sysCountryCodeList);
            return new Task<SysCountryCodeArray>(() => sysCountryCodeArray);
        }
        public override Task<Empty> CreateSystemCountryCode(SysCountryCodeArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateSystemCountryCode(SysCountryCodeArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteSystemCountryCode(SysCountryCodeArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<SystemCountryCodePoco> ProtoToPoco(SysCountryCodeArray request)
        {
            List<SystemCountryCodePoco> pocos = new List<SystemCountryCodePoco>();
            var inputList = request.SysCountryCode.ToList();
            foreach (var item in inputList)
            {
                var poco = new SystemCountryCodePoco();
                poco.Code = item.Code;
                poco.Name = item.Name;
                pocos.Add(poco);
            }
            return pocos;
        }
    }
}
