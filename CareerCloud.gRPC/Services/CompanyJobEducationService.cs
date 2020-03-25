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
using static CareerCloud.gRPC.Protos.CompanyJobEducation;

namespace CareerCloud.gRPC.Services
{
    public class CompanyJobEducationService : CompanyJobEducationBase
    {
        private readonly CompanyJobEducationLogic _logic;

        public CompanyJobEducationService()
        {
            _logic = new CompanyJobEducationLogic(new EFGenericRepository<CompanyJobEducationPoco>());
        }
        public override Task<ComJobEduProto> GetCompanyJobEducation(ComJobEduIdRequest request, ServerCallContext context)
        {
            CompanyJobEducationPoco poco = _logic.Get(Guid.Parse(request.Id));
            if (poco is null)
            {
                throw new ArgumentOutOfRangeException("Id in input not found");
            }
            return new Task<ComJobEduProto>(
                () => new ComJobEduProto
                {
                    Id = poco.Id.ToString(),
                    Job = poco.Job.ToString(),
                    Major = poco.Major,
                    Importance = poco.Importance
                });
        }
        public override Task<ComJobEduArray> GetAllCompanyJobEducation(Empty request, ServerCallContext context)
        {
            List<CompanyJobEducationPoco> pocos = _logic.GetAll(); 
            List<ComJobEduProto> comJobEduList = new List<ComJobEduProto>();
            foreach (var poco in pocos)
            {
                ComJobEduProto comJobEdu = new ComJobEduProto();
                comJobEdu.Id = poco.Id.ToString();
                comJobEdu.Job = poco.Job.ToString();
                comJobEdu.Major = poco.Major;
                comJobEdu.Importance = poco.Importance;
                comJobEduList.Add(comJobEdu);
            }
            ComJobEduArray comJobEduArray = new ComJobEduArray();
            comJobEduArray.ComJobEdu.AddRange(comJobEduList);
            return new Task<ComJobEduArray>(() => comJobEduArray);
        }
        public override Task<Empty> CreateCompanyJobEducation(ComJobEduArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Add(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateCompanyJobEducation(ComJobEduArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Update(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyJobEducation(ComJobEduArray request, ServerCallContext context)
        {
            var pocos = ProtoToPoco(request);
            _logic.Delete(pocos.ToArray());
            return new Task<Empty>(() => new Empty());
        }
        public List<CompanyJobEducationPoco> ProtoToPoco(ComJobEduArray request)
        {
            List<CompanyJobEducationPoco> pocos = new List<CompanyJobEducationPoco>();
            var inputList = request.ComJobEdu.ToList();
            foreach (var item in inputList)
            {
                var poco = new CompanyJobEducationPoco();
                poco.Id = Guid.Parse(item.Id);
                poco.Job = Guid.Parse(item.Job);
                poco.Major = item.Major;
                poco.Importance = Convert.ToInt16(item.Importance);
                pocos.Add(poco);
            }
            return pocos;
        }
    }
}
