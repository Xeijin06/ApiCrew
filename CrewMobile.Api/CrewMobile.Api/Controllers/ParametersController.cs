using CrewMobile.Api.Models;
using CrewMobile.Common.Models;
using CrewMobileApi.Apis.Interfaces;
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

        //TODO: Validar agregar Authorize, http method y Route
        /// <summary>
        /// Get parametes
        /// </summary>
        /// <returns>The parameters</returns>
        public IActionResult GetParameters()
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

        #endregion

        #region Methods

        #endregion
    }
}
