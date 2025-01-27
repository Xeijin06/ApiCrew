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
    public class SecuritySSRsController : ControllerBase
    {
        #region Attributes

        private ICopaAPIs copaApis;

        private ApplicationDbContext db;

        #endregion

        #region Constructors
        public SecuritySSRsController(ApplicationDbContext context, ICopaAPIs _copaApi)
        {
            copaApis = _copaApi;
            db = context;
        }
        #endregion

        #region DB Endpoints
        /// <summary>
        /// GetAllSecuritySSRs
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetAllSecuritySSRs")]
        public IActionResult GetAllSecuritySSRs()
        {
            var securitySSRs = db.SecuritySSRs.ToList();
            return Ok(securitySSRs);
        }

        /// <summary>
        /// GetSecuritySSRs
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetSecuritySSRById/{Id}")]
        public IActionResult GetSecuritySSRById(int Id)
        {
            var securitySSR = db.SecuritySSRs.Find(Id);
            if (securitySSR == null)
            {
                return NotFound();
            }
            return Ok(securitySSR);
        }

        /// <summary>
        /// AddSecuritySSR
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPost]
        [Route("AddSecuritySSR")]
        public IActionResult AddSecuritySSR(Domain.Models.SecuritySSR securitySSR)
        {
            db.SecuritySSRs.Add(securitySSR);
            db.SaveChanges();
            return Ok(securitySSR);
        }

        /// <summary>
        /// UpdateSecuritySSR
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateSecuritySSR")]
        public IActionResult UpdateSecuritySSR(Domain.Models.SecuritySSR securitySSR)
        {
            var securitySSROld = db.SecuritySSRs.Find(securitySSR.SecuritySSId);
            if (securitySSROld == null)
            {
                return NotFound();
            }
            db.Entry(securitySSROld).CurrentValues.SetValues(securitySSR);
            db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// DeleteSecuritySSR
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpDelete]
        [Route("DeleteSecuritySSR/{Id}")]
        public IActionResult DeleteSecuritySSR(int Id)
        {
            var securitySSR = db.SecuritySSRs.Find(Id);
            if (securitySSR != null)
            {
                db.SecuritySSRs.Remove(securitySSR);
                db.SaveChanges();
            }
            return NoContent();
        }
        #endregion
    }
}
