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
    public class SecurityLoginsRoleService : SecurityLoginsRoleBase
    {
        private readonly SecurityLoginsRoleLogic _logic;

        public SecurityLoginsRoleService()
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
        public override Task<SecLoginsRoleArray> GetAllSecurityLoginsRole(Empty request, ServerCallContext context)
        {
            List<SecurityLoginsRolePoco> pocos = _logic.GetAll();
            List<SecLoginsRoleProto> secLoginsRoleList = new List<SecLoginsRoleProto>();
            foreach(var poco in pocos)
            {
                SecLoginsRoleProto secLoginsRole = new SecLoginsRoleProto();
                secLoginsRole.Id = poco.Id.ToString();
                secLoginsRole.Login = poco.Login.ToString();
                secLoginsRole.Role = poco.Role.ToString();
                secLoginsRoleList.Add(secLoginsRole);
            }
            SecLoginsRoleArray secLoginsRoleArray = new SecLoginsRoleArray();
            secLoginsRoleArray.SecLoginsRole.AddRange(secLoginsRoleList);
            return new Task<SecLoginsRoleArray>(() => secLoginsRoleArray);
        }
        public override Task<Empty> CreateSecurityLoginsRole(SecLoginsRoleArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateSecurityLoginsRole(SecLoginsRoleArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteSecurityLoginsRole(SecLoginsRoleArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<SecurityLoginsRolePoco> ProtoToPoco(SecLoginsRoleArray request)
        {
            List<SecurityLoginsRolePoco> pocos = new List<SecurityLoginsRolePoco>();
            var inputList = request.SecLoginsRole.ToList();
            foreach (var item in inputList)
            {
                var poco = new SecurityLoginsRolePoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Login = Guid.Parse(item.Login);
                poco.Role = Guid.Parse(item.Role);
                pocos.Add(poco);
            }
            return pocos;
        }
    }
}
