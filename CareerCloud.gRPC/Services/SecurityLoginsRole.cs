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
using static CareerCloud.gRPC.Protos.SecurityLoginsRole;

namespace CareerCloud.gRPC.Services
{
    public class SecurityLoginsRole : SecurityLoginsRoleBase
    {
        private readonly SecurityLoginsRoleLogic _logic;

        public SecurityLoginsRole()
        {
            _logic = new SecurityLoginsRoleLogic(new EFGenericRepository<SecurityLoginsRolePoco>());
        }
        public override Task<SecLoginsRoleProto> GetSecurityLoginsRole(SecLoginsRoleIdRequest request, ServerCallContext context)
        {
            SecurityLoginsRolePoco poco = _logic.Get(Guid.Parse(request.Id));
            if (poco is null)
            {
                throw new ArgumentOutOfRangeException("No matching id found");
            }
            return new Task<SecLoginsRoleProto>(
                () => new SecLoginsRoleProto
                {
                    Id = poco.Id.ToString(),
                    Login = poco.Login.ToString(),
                    Role = poco.Role.ToString()
                });  
        }

        public override Task<Empty> CreateSecurityLoginsRole(SecLoginsRoleProto request, ServerCallContext context)
        {
            SecurityLoginsRolePoco[] pocos = new SecurityLoginsRolePoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Login = Guid.Parse(request.Login);
                poco.Role = Guid.Parse(request.Role);
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateSecurityLoginsRole(SecLoginsRoleProto request, ServerCallContext context)
        {
            SecurityLoginsRolePoco[] pocos = new SecurityLoginsRolePoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Login = Guid.Parse(request.Login);
                poco.Role = Guid.Parse(request.Role);
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteSecurityLoginsRole(SecLoginsRoleProto request, ServerCallContext context)
        {
            SecurityLoginsRolePoco[] pocos = new SecurityLoginsRolePoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Login = Guid.Parse(request.Login);
                poco.Role = Guid.Parse(request.Role);
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
