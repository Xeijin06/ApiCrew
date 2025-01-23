using CrewMobile.Api.Models;
using CrewMobileApi.Apis.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrewMobile.Api.Controllers
{
    /// <summary>
    /// Controller to provide information to App
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class DescriptorsController : ControllerBase
    {
        #region Attributes

        private ICopaAPIs copaApis;

        private ApplicationDbContext db;

        #endregion

        #region Constructors
        public DescriptorsController(ApplicationDbContext context, ICopaAPIs _copaApi)
        {
            copaApis = _copaApi;
            db = context;
        }
        #endregion

        #region DB Endpoints
        /// <summary>
        /// GetAllDescriptors
        /// </summary>
        /// <returns>Json</returns>
        ////[Authorize]
        [HttpGet]
        [Route("GetAllDescriptors")]
        public IActionResult GetAllDescriptors()
        {
            var descriptors = db.Descriptors.ToList();
            return Ok(descriptors);
        }

        /// <summary>
        /// GetDescriptors
        /// </summary>
        /// <returns>Json</returns>
        ////[Authorize]
        [HttpGet]
        [Route("GetDescriptorById/{Id}")]
        public IActionResult GetDescriptorById(int Id)
        {
            var descriptor = db.Descriptors.Find(Id);
            if (descriptor == null)
            {
                return NotFound();
            }
            return Ok(descriptor);
        }

        /// <summary>
        /// AddDescriptor
        /// </summary>
        /// <returns>Json</returns>
        ////[Authorize]
        [HttpPost]
        [Route("AddDescriptor")]
        public IActionResult AddDescriptor(Domain.Models.Descriptor descriptor)
        {
            db.Descriptors.Add(descriptor);
            db.SaveChanges();
            return Ok(descriptor);
        }

        /// <summary>
        /// UpdateDescriptor
        /// </summary>
        /// <returns>Json</returns>
        ////[Authorize]
        [HttpPut]
        [Route("UpdateDescriptor")]
        public IActionResult UpdateDescriptor(Domain.Models.Descriptor descriptor)
        {
            var descriptorOld = db.Descriptors.Find(descriptor.DescriptorId);
            if (descriptorOld == null)
            {
                return NotFound();
            }
            db.Entry(descriptorOld).CurrentValues.SetValues(descriptor);
            db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// DeleteDescriptor
        /// </summary>
        /// <returns>Json</returns>
        ////[Authorize]
        [HttpDelete]
        [Route("DeleteDescriptor/{Id}")]
        public IActionResult DeleteDescriptor(int Id)
        {
            var descriptor = db.Descriptors.Find(Id);
            if (descriptor != null)
            {
                db.Descriptors.Remove(descriptor);
                db.SaveChanges();
            }
            return NoContent();
        }
        #endregion
    }
}
