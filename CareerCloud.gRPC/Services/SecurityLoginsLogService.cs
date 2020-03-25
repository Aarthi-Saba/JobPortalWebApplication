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
using static CareerCloud.gRPC.Protos.SecurityLoginsLog;

namespace CareerCloud.gRPC.Services
{
    public class SecurityLoginsLogService : SecurityLoginsLogBase
    {
        private readonly SecurityLoginsLogLogic _logic;

        public SecurityLoginsLogService()
        {
            _logic = new SecurityLoginsLogLogic(new EFGenericRepository<SecurityLoginsLogPoco>());
        }
        public override Task<SecLoginsLogProto> GetSecurityLoginsLog(SecLoginsLogIdRequest request, ServerCallContext context)
        {
            SecurityLoginsLogPoco poco = _logic.Get(Guid.Parse(request.Id));
            if (poco is null)
            {
                throw new ArgumentOutOfRangeException("Id does not exist in system");
            }
            return new Task<SecLoginsLogProto>(
                () => new SecLoginsLogProto
                {
                    Id = poco.Id.ToString(),
                    Login = poco.Login.ToString(),
                    SourceIP = poco.SourceIP,
                    LogonDate = Timestamp.FromDateTime(poco.LogonDate),
                    IsSuccesful = poco.IsSuccesful
                });
        }
        public override Task<SecLoginsLogArray> GetAllSecurityLoginsLog(Empty request, ServerCallContext context)
        {
            List<SecurityLoginsLogPoco> pocos = _logic.GetAll();
            List<SecLoginsLogProto> secLoginsLogList = new List<SecLoginsLogProto>();
            foreach(var poco in pocos)
            {
                SecLoginsLogProto secLoginsLog = new SecLoginsLogProto();
                secLoginsLog.Id = poco.Id.ToString();
                secLoginsLog.Login = poco.Login.ToString();
                secLoginsLog.SourceIP = poco.SourceIP;
                secLoginsLog.LogonDate = Timestamp.FromDateTime(poco.LogonDate);
                secLoginsLog.IsSuccesful = poco.IsSuccesful;
                secLoginsLogList.Add(secLoginsLog);
            }
            SecLoginsLogArray secLoginsLogArray = new SecLoginsLogArray();
            secLoginsLogArray.SecLoginsLog.AddRange(secLoginsLogList);
            return new Task<SecLoginsLogArray>(() => secLoginsLogArray);
        }
        public override Task<Empty> CreateSecurityLoginsLog(SecLoginsLogArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateSecurityLoginsLog(SecLoginsLogArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteSecurityLoginsLog(SecLoginsLogArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<SecurityLoginsLogPoco> ProtoToPoco(SecLoginsLogArray request)
        {
            List<SecurityLoginsLogPoco> pocos = new List<SecurityLoginsLogPoco>();
            var inputList = request.SecLoginsLog.ToList();
            foreach (var item in inputList)
            {
                var poco = new SecurityLoginsLogPoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Login = Guid.Parse(item.Login);
                poco.SourceIP = item.SourceIP;
                poco.LogonDate = item.LogonDate.ToDateTime();
                poco.IsSuccesful = item.IsSuccesful;
                pocos.Add(poco);
            }
            return pocos;
        }
    }
}
