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
using static CareerCloud.gRPC.Protos.SecurityLogin;

namespace CareerCloud.gRPC.Services
{
    public class SecurityLoginService : SecurityLoginBase
    {
        private readonly SecurityLoginLogic _logic;

        public SecurityLoginService()
        {
            _logic = new SecurityLoginLogic(new EFGenericRepository<SecurityLoginPoco>());
        }
        public override Task<SecLoginProto> GetSecurityLogin(SecLogIdRequest request, ServerCallContext context)
        {
            SecurityLoginPoco poco = _logic.Get(Guid.Parse(request.Id));
            if(poco is null)
            {
                throw new ArgumentOutOfRangeException("The security Id entered does not exist in system");
            }
            return new Task<SecLoginProto>(
                () => new SecLoginProto
                {
                    Id = poco.Id.ToString(),
                    Login = poco.Login,
                    Password = poco.Password,
                    Created = Timestamp.FromDateTime((DateTime)poco.Created),
                    PasswordUpdate = poco.PasswordUpdate is null ? null : Timestamp.FromDateTime((DateTime)poco.PasswordUpdate),
                    AgreementAccepted = poco.AgreementAccepted is null ? null : Timestamp.FromDateTime((DateTime)poco.AgreementAccepted),
                    IsLocked = poco.IsLocked,
                    IsInactive = poco.IsInactive,
                    EmailAddress = poco.EmailAddress,
                    PhoneNumber = poco.PhoneNumber,
                    FullName = poco.FullName,
                    ForceChangePassword = poco.ForceChangePassword,
                    PrefferredLanguage = poco.PrefferredLanguage
                });
        }
        public override Task<SecLoginArray> GetAllSecurityLogin(Empty request, ServerCallContext context)
        {
            List<SecurityLoginPoco> pocos = _logic.GetAll();
            List<SecLoginProto> secLoginList = new List<SecLoginProto>();
            foreach (var poco in pocos)
            {
                SecLoginProto secLogin = new SecLoginProto();
                secLogin.Id = poco.Id.ToString();
                secLogin.Login = poco.Login;
                secLogin.Password = poco.Password;
                secLogin.Created = Timestamp.FromDateTime((DateTime)poco.Created);
                secLogin.PasswordUpdate = poco.PasswordUpdate is null ? null : Timestamp.FromDateTime((DateTime)poco.PasswordUpdate);
                secLogin.AgreementAccepted = poco.AgreementAccepted is null ? null : Timestamp.FromDateTime((DateTime)poco.AgreementAccepted);
                secLogin.IsLocked = poco.IsLocked;
                secLogin.IsInactive = poco.IsInactive;
                secLogin.EmailAddress = poco.EmailAddress;
                secLogin.PhoneNumber = poco.PhoneNumber;
                secLogin.FullName = poco.FullName;
                secLogin.ForceChangePassword = poco.ForceChangePassword;
                secLogin.PrefferredLanguage = poco.PrefferredLanguage;
                secLoginList.Add(secLogin);
            }
            SecLoginArray secLoginArray = new SecLoginArray();
            secLoginArray.SecLogin.AddRange(secLoginList);
            return new Task<SecLoginArray>(() => secLoginArray);
        }
        public override Task<Empty> CreateSecurityLogin(SecLoginArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateSecurityLogin(SecLoginArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteSecurityLogin(SecLoginArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<SecurityLoginPoco> ProtoToPoco(SecLoginArray request)
        {
            List<SecurityLoginPoco> pocos = new List<SecurityLoginPoco>();
            var inputList = request.SecLogin.ToList();
            foreach(var item in inputList)
            {
                var poco = new SecurityLoginPoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Login = item.Login;
                poco.Password = item.Password;
                poco.Created = item.Created.ToDateTime();
                poco.PasswordUpdate = item.PasswordUpdate.ToDateTime();
                poco.AgreementAccepted = item.AgreementAccepted.ToDateTime();
                poco.IsLocked = item.IsLocked;
                poco.IsInactive = item.IsInactive;
                poco.EmailAddress = item.EmailAddress;
                poco.PhoneNumber = item.PhoneNumber;
                poco.FullName = item.FullName;
                poco.ForceChangePassword = item.ForceChangePassword;
                poco.PrefferredLanguage = item.PrefferredLanguage;
                pocos.Add(poco);
            }
            return pocos;
        }
    }
}
