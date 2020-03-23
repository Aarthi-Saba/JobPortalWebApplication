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
using static CareerCloud.gRPC.Protos.SecurityRole;

namespace CareerCloud.gRPC.Services
{
    public class SecurityRoleService : SecurityRoleBase
    {
        private readonly SecurityRoleLogic _logic;

        public SecurityRoleService()
        {
            _logic = new SecurityRoleLogic(new EFGenericRepository<SecurityRolePoco>());
        }
        public override Task<SecRoleProto> GetSecurityRole(SecRoleIdRequest request, ServerCallContext context)
        {
            SecurityRolePoco poco = _logic.Get(Guid.Parse(request.Id));
            if(poco is null)
            {
                throw new ArgumentOutOfRangeException("Invalid Id");
            }
            return new Task<SecRoleProto>(
                () => new SecRoleProto
                {
                    Id = poco.Id.ToString(),
                    Role = poco.Role,
                    IsInactive = poco.IsInactive
                });
        }
        public override Task<Empty> CreateSecurityRole(SecRoleProto request, ServerCallContext context)
        {
            SecurityRolePoco[] pocos = new SecurityRolePoco[1];
            foreach(var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Role = request.Role;
                poco.IsInactive = request.IsInactive;
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateSecurityRole(SecRoleProto request, ServerCallContext context)
        {
            SecurityRolePoco[] pocos = new SecurityRolePoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Role = request.Role;
                poco.IsInactive = request.IsInactive;
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteSecurityRole(SecRoleProto request, ServerCallContext context)
        {
            SecurityRolePoco[] pocos = new SecurityRolePoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Role = request.Role;
                poco.IsInactive = request.IsInactive;
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
