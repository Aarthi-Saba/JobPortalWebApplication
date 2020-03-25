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
            if (poco is null)
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
        public override Task<SecRoleArray> GetAllSecurityRole(Empty request, ServerCallContext context)
        {
            List<SecurityRolePoco> pocos = _logic.GetAll();
            List<SecRoleProto> secRoleList = new List<SecRoleProto>();
            foreach (var poco in pocos)
            {
                SecRoleProto secRole = new SecRoleProto();
                secRole.Id = poco.Id.ToString();
                secRole.Role = poco.Role;
                secRole.IsInactive = poco.IsInactive;
                secRoleList.Add(secRole);
            }
            SecRoleArray secRoleArray = new SecRoleArray();
            secRoleArray.SecRole.AddRange(secRoleList);
            return new Task<SecRoleArray>(() => secRoleArray);
        }
        public override Task<Empty> CreateSecurityRole(SecRoleArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateSecurityRole(SecRoleArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteSecurityRole(SecRoleArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }

        public List<SecurityRolePoco> ProtoToPoco(SecRoleArray request)
        {
            List<SecurityRolePoco> pocos = new List<SecurityRolePoco>();
            var inputList = request.SecRole.ToList();
            foreach (var item in inputList)
            {
                var poco = new SecurityRolePoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Role = item.Role;
                poco.IsInactive = item.IsInactive;
                pocos.Add(poco);
            }
            return pocos;
        }
    }
}
