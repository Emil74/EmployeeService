using EmployeeService.Controllers;
using EmployeeService.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Priority;

namespace EmployeeServiceTests
{
    public class SampleTest
    {
        private SampleController _sampleController;
        private SampleObjectPool _pool;

        public SampleTest()
        {
            _pool = SampleObjectPool.Instance;
            _sampleController = new SampleController(_pool);
        }

        [Theory, Priority(1)]
        [InlineData(5)]
        [InlineData(15)]
        [InlineData(20)]
        public void CreateSampleObjectTest(int id)
        {
            ActionResult<bool> result = _sampleController.Create(id);
            Assert.IsAssignableFrom<ActionResult<bool>>(result);
        }

        [Fact, Priority(2)]
        public void GetAllSampleObjectTest()
        {
            var response = _sampleController.GetAll();
            Assert.NotNull(response.Result);
            Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            Assert.IsAssignableFrom<List<SampleObject>>(((OkObjectResult)response.Result).Value);
            Assert.NotEmpty((List<SampleObject>)((OkObjectResult)response.Result).Value);
        }

    }
}

