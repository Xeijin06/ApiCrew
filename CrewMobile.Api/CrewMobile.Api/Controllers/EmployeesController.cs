using CrewMobile.Api.Models;
using CrewMobileApi.Apis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrewMobile.Api.Controllers
{
    /// <summary>
    /// Controller to provide information to App
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        #region Attributes

        private ICopaAPIs copaApis;

        private ApplicationDbContext db;

        #endregion

        #region Constructors
        public EmployeesController(ApplicationDbContext context, ICopaAPIs _copaApi)
        {
            copaApis = _copaApi;
            db = context;
        }
        #endregion

        #region DB Endpoints
        /// <summary>
        /// GetAllEmployees
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            var employees = db.Employees.ToList();
            return Ok(employees);
        }

        /// <summary>
        /// GetEmployees
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetEmployeeById/{Id}")]
        public IActionResult GetEmployeeById(int Id)
        {
            var employee = db.Employees.Find(Id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        /// <summary>
        /// AddEmployee
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPost]
        [Route("AddEmployee")]
        public IActionResult AddEmployee(Domain.Models.Employee employee)
        {
            db.Employees.Add(employee);
            db.SaveChanges();
            return Ok(employee);
        }

        /// <summary>
        /// UpdateEmployee
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateEmployee")]
        public IActionResult UpdateEmployee(Domain.Models.Employee employee)
        {
            var employeeOld = db.Employees.Find(employee.EmployeeId);
            if (employeeOld == null)
            {
                return NotFound();
            }
            db.Entry(employeeOld).CurrentValues.SetValues(employee);
            db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// DeleteEmployee
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpDelete]
        [Route("DeleteEmployee/{Id}")]
        public IActionResult DeleteEmployee(int Id)
        {
            var employee = db.Employees.Find(Id);
            if (employee != null)
            {
                db.Employees.Remove(employee);
                db.SaveChanges();
            }
            return NoContent();
        }
        #endregion
    }
}
