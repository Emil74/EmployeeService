using EmployeeService.Data;
using EmployeeService.Models.Dto;
using EmployeeService.Services;
using EmployeeService.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        #region Services 

        private readonly IDepartmentRepository _departmentRepository;

        #endregion

        #region Constructors

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        #endregion
        [HttpGet("departament/all")]
        public ActionResult<IList<DepartmentDto>> GetAllDepartament()
        {
            return Ok(_departmentRepository.GetAll().Select(et =>
                new DepartmentDto
                {
                    Id = et.Id,
                    Description = et.Description
                }
                ).ToList()); 
        }
        
        [HttpPost("department/create")]
        public ActionResult<int> CreateDepartment([FromQuery] string description)
        {
            return Ok(_departmentRepository.Create(new Department
            {
                Description = description
            }));
        }
        [HttpDelete("department/delete")]
        public ActionResult<bool> DeleteDepartment([FromQuery] int id)
        {
            return Ok(_departmentRepository.Delete(id));
        }

        [HttpPut("department/update")]
        public ActionResult<bool> UpdateDepartment([FromQuery] int id, [FromQuery] string description)
        {
            return Ok(_departmentRepository.Update(new Department()
            {
                Id = id,
                Description = description
            }));
        }

        [HttpGet("department/getById")]
        public ActionResult<DepartmentDto> GetByIdDepartment([FromQuery] int id)
        {
            return Ok(_departmentRepository.GetById(id));
        }


    }
}
