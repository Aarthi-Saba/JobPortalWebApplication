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
        public override Task<Empty> CreateCompanyJobEducation(ComJobEduProto request, ServerCallContext context)
        {
            CompanyJobEducationPoco[] pocos = new CompanyJobEducationPoco[1];
            foreach(var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Job = Guid.Parse(request.Job);
                poco.Major = request.Major;
                poco.Importance = Convert.ToInt16(request.Importance);
            }
            _logic.Add(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> UpdateCompanyJobEducation(ComJobEduProto request, ServerCallContext context)
        {
            CompanyJobEducationPoco[] pocos = new CompanyJobEducationPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Job = Guid.Parse(request.Job);
                poco.Major = request.Major;
                poco.Importance = Convert.ToInt16(request.Importance);
            }
            _logic.Update(pocos);
            return new Task<Empty>(() => new Empty());
        }
        public override Task<Empty> DeleteCompanyJobEducation(ComJobEduProto request, ServerCallContext context)
        {
            CompanyJobEducationPoco[] pocos = new CompanyJobEducationPoco[1];
            foreach (var poco in pocos)
            {
                poco.Id = Guid.Parse(request.Id);
                poco.Job = Guid.Parse(request.Job);
                poco.Major = request.Major;
                poco.Importance = Convert.ToInt16(request.Importance);
            }
            _logic.Delete(pocos);
            return new Task<Empty>(() => new Empty());
        }
    }
}
