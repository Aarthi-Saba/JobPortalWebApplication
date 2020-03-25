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
            if (poco is null)
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
        public override Task<SysLangCodeArray> GetAllSystemLanguageCode(Empty request, ServerCallContext context)
        {
            List<SystemLanguageCodePoco> pocos = _logic.GetAll();
            List<SysLangCodeProto> sysLangCodeList = new List<SysLangCodeProto>();
            foreach (var poco in pocos)
            {
                SysLangCodeProto sysLangCode = new SysLangCodeProto();
                sysLangCode.LangId = poco.LanguageID;
                sysLangCode.Name = poco.Name;
                sysLangCode.NativeName = poco.NativeName;
                sysLangCodeList.Add(sysLangCode);
            }
            SysLangCodeArray sysLangCodeArray = new SysLangCodeArray();
            sysLangCodeArray.SysLangCode.AddRange(sysLangCodeList);
            return new Task<SysLangCodeArray>(() => sysLangCodeArray);
        }
        public override Task<Empty> CreateSystemKLanguageCode(SysLangCodeArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateSystemLanguageCode(SysLangCodeArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteSystemLanguageCode(SysLangCodeArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<SystemLanguageCodePoco> ProtoToPoco(SysLangCodeArray request)
        {
            List<SystemLanguageCodePoco> pocos = new List<SystemLanguageCodePoco>();
            var inputList = request.SysLangCode.ToList();
            foreach (var item in inputList)
            {
                var poco = new SystemLanguageCodePoco();
                poco.LanguageID = item.LangId;
                poco.Name = item.Name;
                poco.NativeName = item.NativeName;
                pocos.Add(poco);
            }
            return pocos;
        }
    }
}
