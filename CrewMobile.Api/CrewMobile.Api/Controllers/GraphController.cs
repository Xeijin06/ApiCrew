using CrewMobile.Api.Models;
using CrewMobileApi.Apis.Interfaces;
using CrewMobileApi.Apis;
using CrewMobileApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Graph;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace CrewMobile.Api.Controllers
{
    /// <summary>
    /// Controller to provide information to App
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        #region Attributes

        /// <summary>
        /// The Graph Service
        /// </summary>
        private GraphService _graphService;

        /// <summary>
        /// Read Configuration Values
        /// </summary>
        private IConfiguration _configuration;

        #endregion

        #region Constructors

        public GraphController(IConfiguration configuration, GraphService graphService)
        {

            _configuration = configuration;
            _graphService = graphService;

        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Get User Information from Graph
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetUserInformation/{email}")]
        public async Task<IActionResult> GetUserAsync(string email)
        {
            //dynamic jsonObject = form;
            try
            {
                //var user = await _graphService.GetUserInformationByEmailAsync(jsonObject.Email.Value);
                var user = await _graphService.GetUserInformationByEmailAsync(email);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Methods
        #endregion
    }
}
