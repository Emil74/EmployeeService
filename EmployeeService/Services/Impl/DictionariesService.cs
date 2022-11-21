using Azure;
using EmployeeService.Models.Dto;
using EmployeeServiceProto;
using Grpc.Core;
using static EmployeeServiceProto.DictionariesService;

namespace EmployeeService.Services.Impl
{
    public class DictionariesService : DictionariesServiceBase
    {
        #region Services 

        private readonly IEmployeeTypeRepository _employeeTypeRepository;

        #endregion

        #region Constructors

        public DictionariesService(IEmployeeTypeRepository employeeTypeRepository)
        {
            _employeeTypeRepository = employeeTypeRepository;
        }
        #endregion


        public override Task<CreateEmployeeTypeResponse> CreateEmployeeType(CreateEmployeeTypeRequest request, ServerCallContext context)
        {
            var id = _employeeTypeRepository.Create(new Data.EmployeeType
            {
                Description = request.Description
            });
            CreateEmployeeTypeResponse response = new CreateEmployeeTypeResponse();
            response.Id = id;
            return Task.FromResult(response);
        }

        public override Task<DeleteEmployeeTypeResponse> DeleteEmployeeType(DeleteEmployeeTypeRequest request, ServerCallContext context)
        {
            _employeeTypeRepository.Delete(request.Id);
            return Task.FromResult(new DeleteEmployeeTypeResponse());
        }

        public override Task<GetAllEmployeeTypesResponse> GetAllEmployeeTypes(GetAllEmployeeTypesRequest request, ServerCallContext context)
        {
            GetAllEmployeeTypesResponse response = new GetAllEmployeeTypesResponse();

            response.EmployeeTypes.AddRange(_employeeTypeRepository.GetAll().Select(et =>
                new EmployeeServiceProto.EmployeeType
                {
                    Id = et.Id,
                    Description = et.Description
                }).ToList());
            return Task.FromResult(response);
        }
    }
}
