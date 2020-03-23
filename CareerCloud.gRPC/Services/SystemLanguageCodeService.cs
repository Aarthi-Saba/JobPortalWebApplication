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
using static CareerCloud.gRPC.Protos.SystemLanguageCode;

namespace CareerCloud.gRPC.Services
{
    public class SystemLanguageCodeService : SystemLanguageCodeBase
    {
        private readonly SystemLanguageCodeLogic _logic;

        public SystemLanguageCodeService()
        {
            _logic = new SystemLanguageCodeLogic(new EFGenericRepository<SystemLanguageCodePoco>());
        }
        public override Task<SysLangCodeProto> GetSystemLanguageCode(SysLangIdRequest request, ServerCallContext context)
        {
            SystemLanguageCodePoco poco = _logic.Get(request.LangId);
            if(poco is null)
            {
                throw new ArgumentOutOfRangeException("Invalid LangId ");
            }
            return new Task<SysLangCodeProto>(
                () => new SysLangCodeProto
                {
                    LangId = poco.LanguageID,
                    Name = poco.Name,
                    NativeName = poco.NativeName
                });
        }
        public override Task<Empty> CreateSystemKLanguageCode(SysLangCodeProto request, ServerCallContext context)
        {
            SystemLanguageCodePoco[] pocos = new SystemLanguageCodePoco[1];
            foreach(var poco in pocos)
            {
                poco.LanguageID = request.LangId;
                poco.Name = request.Name;
                poco.NativeName = request.NativeName;
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateSystemLanguageCode(SysLangCodeProto request, ServerCallContext context)
        {
            SystemLanguageCodePoco[] pocos = new SystemLanguageCodePoco[1];
            foreach (var poco in pocos)
            {
                poco.LanguageID = request.LangId;
                poco.Name = request.Name;
                poco.NativeName = request.NativeName;
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteSystemLanguageCode(SysLangCodeProto request, ServerCallContext context)
        {
            SystemLanguageCodePoco[] pocos = new SystemLanguageCodePoco[1];
            foreach (var poco in pocos)
            {
                poco.LanguageID = request.LangId;
                poco.Name = request.Name;
                poco.NativeName = request.NativeName;
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
