using CrewMobile.Api.Models;
using CrewMobile.Common.Models;
using CrewMobile.Domain.Models;
using CrewMobileApi.Apis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrewMobile.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametersController : ControllerBase
    {
        #region Attributes

        private ICopaAPIs copaApis;

        private ApplicationDbContext db;

        #endregion

        #region Constructors

        public ParametersController(ApplicationDbContext context, ICopaAPIs _copaApi)
        {
            copaApis = _copaApi;
            db = context;
        }

        #endregion

        #region Endpoints
        #region Parameters
        //TODO: Validar agregar Authorize
        /// <summary>
        /// Get some parametes
        /// </summary>
        /// <returns>The parameters</returns>
        [HttpGet]
        [Route("GetSomeParameters")]
        public IActionResult GetSomeParameters()
        {
            var parameter = db.Parameters.FirstOrDefault();
            var groups = db.Groups.ToList();

            var groupsString = string.Empty;
            foreach (var group in groups)
            {
                groupsString += string.Format("{0},", group.GroupGuid);
            }

            groupsString = groupsString.Substring(0, groupsString.Length - 1);

            var response = new ParameterResponse
            {
                MinimumRequiredVersion = parameter.MinimumRequiredVersion,
                AvailableGroups = groupsString,
                ValidateUserGroups = parameter.ValidateUserGroups,
                AutoUpdateEveryMinutes = parameter.AutoUpdateEveryMinutes,
            };

            return Ok(response);
        }

        //TODO: Validar la necesidad de un metodo de actualizacion de valores

        /// <summary>
        /// GetParameters
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetParameters")]
        public IActionResult GetParameters()
        {
            var parameters = db.Parameters.ToList();
            return Ok(parameters);
        }

        /// <summary>
        /// GetParameters
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetParameters/{Id}")]
        public IActionResult GetParameters(int Id)
        {
            var parameter = db.Parameters.Find(Id);
            if (parameter == null)
            {
                return NotFound();
            }
            return Ok(parameter);
        }

        /// <summary>
        /// AddParameters
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPost]
        [Route("AddParameters")]
        public IActionResult AddParameters(CMParameter parameter)
        {
            db.Parameters.Add(parameter);
            db.SaveChanges();
            return Ok(parameter);
        }

        /// <summary>
        /// UpdateParameters
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateParameters")]
        public IActionResult UpdateParameters(CMParameter parameter)
        {
            var parameterOld = db.Parameters.Find(parameter.ParameterId);
            if (parameterOld == null)
            {
                return NotFound();
            }
            //db.Update(parameter);
            db.Entry(parameterOld).CurrentValues.SetValues(parameter);
            db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// DeleteParameters
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpDelete]
        [Route("DeleteParameters/{Id}")]
        public IActionResult DeleteParameters(int Id)
        {
            var parameter = db.Parameters.Find(Id);
            if (parameter != null)
            {
                db.Parameters.Remove(parameter);
                db.SaveChanges();
            }
            return NoContent();
        }
        #endregion

        #region Groups
        /// <summary>
        /// GetAllGroups
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetAllGroups")]
        public IActionResult GetAllGroups()
        {
            var groups = db.Groups.ToList();
            return Ok(groups);
        }

        /// <summary>
        /// GetGroups
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetGroupById/{Id}")]
        public IActionResult GetGroupById(int Id)
        {
            var group = db.Groups.Find(Id);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group);
        }

        /// <summary>
        /// AddGroup
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPost]
        [Route("AddGroup")]
        public IActionResult AddGroup(Domain.Models.Group group)
        {
            db.Groups.Add(group);
            db.SaveChanges();
            return Ok(group);
        }

        /// <summary>
        /// UpdateGroup
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateGroup")]
        public IActionResult UpdateGroup(Domain.Models.Group group)
        {
            var groupOld = db.Groups.Find(group.GroupId);
            if (groupOld == null)
            {
                return NotFound();
            }
            db.Entry(groupOld).CurrentValues.SetValues(group);
            db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// DeleteGroup
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpDelete]
        [Route("DeleteGroup/{Id}")]
        public IActionResult DeleteGroup(int Id)
        {
            var group = db.Groups.Find(Id);
            if (group != null)
            {
                db.Groups.Remove(group);
                db.SaveChanges();
            }
            return NoContent();
        }
        #endregion

        #endregion

        #region Methods

        #endregion
    }
}
