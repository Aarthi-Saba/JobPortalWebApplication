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
            if(poco is null)
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
        public override Task<Empty> CreateSecurityLoginsLog(SecLoginsLogProto request, ServerCallContext context)
        {
            SecurityLoginsLogPoco[] pocos = new SecurityLoginsLogPoco[1];
            foreach(var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Login = Guid.Parse(request.Login);
                poco.SourceIP = request.SourceIP;
                poco.LogonDate = request.LogonDate.ToDateTime();
                poco.IsSuccesful = request.IsSuccesful;
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateSecurityLoginsLog(SecLoginsLogProto request, ServerCallContext context)
        {
            SecurityLoginsLogPoco[] pocos = new SecurityLoginsLogPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Login = Guid.Parse(request.Login);
                poco.SourceIP = request.SourceIP;
                poco.LogonDate = request.LogonDate.ToDateTime();
                poco.IsSuccesful = request.IsSuccesful;
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteSecurityLoginsLog(SecLoginsLogProto request, ServerCallContext context)
        {
            SecurityLoginsLogPoco[] pocos = new SecurityLoginsLogPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Login = Guid.Parse(request.Login);
                poco.SourceIP = request.SourceIP;
                poco.LogonDate = request.LogonDate.ToDateTime();
                poco.IsSuccesful = request.IsSuccesful;
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
