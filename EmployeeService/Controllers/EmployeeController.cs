using EmployeeService.Data;
using EmployeeService.Models.Dto;
using EmployeeService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        #region Services 

        private readonly IEmployeeRepository _employeeRepository;

        #endregion

        #region Constructors

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        #endregion

        [HttpGet("employee/all")]
        public ActionResult<IList<EmployeeDto>> GetAllEmployee()
        {
            return Ok(_employeeRepository.GetAll().Select(et =>
                new EmployeeDto
                {
                    Id = et.Id,
                    EmployeeTypeId = et.EmployeeTypeId,
                    DepartmentId = et.DepartmentId,
                    FirstName = et.FirstName,
                    Surname = et.Surname,
                    Patronymic = et.Patronymic

                }
                ).ToList());
        }
        [HttpPost("employe/create")]
        public ActionResult<bool> CreateEmployee([FromQuery] int departmenId,              //CreateEmployeeRequest
                                                 [FromQuery] int employeTypeId,
                                                 [FromQuery] string firstName,
                                                 [FromQuery] string surname,
                                                 [FromQuery] string patrnyomic,
                                                 [FromQuery] decimal salary)
        {
            return Ok(_employeeRepository.Create(new Employee()
            {
                DepartmentId = departmenId,
                EmployeeTypeId = employeTypeId,
                FirstName = firstName,
                Surname = surname,
                Patronymic = patrnyomic,
                Salary = salary
            }));
        }
        [HttpDelete("employe/delete")]
        public ActionResult<bool> DeleteEmploye([FromQuery] int id)
        {
            return Ok(_employeeRepository.Delete(id));
        }

        [HttpPut("employe/update")]
        public ActionResult<bool> UpdateEmploye(
                                                 [FromQuery] int id,
                                                 [FromQuery] int departmenId,
                                                 [FromQuery] int employeTypeId,
                                                 [FromQuery] string firstName,
                                                 [FromQuery] string surname,
                                                 [FromQuery] string patrnyomic,
                                                 [FromQuery] decimal salary)
        {
            return Ok(_employeeRepository.Update(new Employee()
            {
                Id = id,
                DepartmentId = departmenId,
                EmployeeTypeId = employeTypeId,
                FirstName = firstName,
                Surname = surname,
                Patronymic = patrnyomic,
                Salary = salary
            })); ;
        }

        [HttpGet("employe/getById")]
        public ActionResult<EmployeeDto> GetByIdEmploye([FromQuery] int id)
        {
            return Ok(_employeeRepository.GetById(id));
        }

    }
}
