using EmployeeService.Models;
using EmployeeService.Models.Dto;
using EmployeeService.Services;
using EmployeeService.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        #region Services 

        private readonly SampleObjectPool _pool;

        #endregion

        #region Constructors

        public SampleController(SampleObjectPool pool)
        {
            _pool = pool;
        }

        #endregion

        [HttpPost("create")]
        public ActionResult<bool> Create(int id)
        {
            return Ok(_pool.Add(id));
        }


        [HttpGet("get/all")]
        public ActionResult<IList<EmployeeDto>> GetAll()
        {
            return Ok(_pool.GetAll());
        }
    }
}
