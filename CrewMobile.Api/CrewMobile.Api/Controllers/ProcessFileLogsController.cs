using CrewMobile.Api.Models;
using CrewMobileApi.Apis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrewMobile.Api.Controllers
{
    /// <summary>
    /// Controller to provide information to App
    /// </summary>
    /// 

    //TODO: Validar el set de endpoints de BD a utilizar del logueo de archivos

    [Route("api/[controller]")]
    [ApiController]
    public class ProcessFileLogsController : ControllerBase
    {       
        #region Attributes

        private ICopaAPIs copaApis;

        private ApplicationDbContext db;

        #endregion

        #region Constructors
        public ProcessFileLogsController(ApplicationDbContext context, ICopaAPIs _copaApi)
        {
            copaApis = _copaApi;
            db = context;
        }
        #endregion

        #region DB Endpoints
        /// <summary>
        /// GetAllProcessFileLogs
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetAllProcessFileLogs")]
        public IActionResult GetAllProcessFileLogs()
        {
            var processFileLogs = db.ProccessFileLogs.ToList();
            return Ok(processFileLogs);
        }

        /// <summary>
        /// GetProcessFileLogs
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetProcessFileLogById/{Id}")]
        public IActionResult GetProcessFileLogById(int Id)
        {
            var processFileLog = db.ProccessFileLogs.Find(Id);
            if (processFileLog == null)
            {
                return NotFound();
            }
            return Ok(processFileLog);
        }

        /// <summary>
        /// AddProcessFileLog
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPost]
        [Route("AddProcessFileLog")]
        public IActionResult AddProcessFileLog(Domain.Models.ProccessFileLog processFileLog)
        {
            db.ProccessFileLogs.Add(processFileLog);
            db.SaveChanges();
            return Ok(processFileLog);
        }

        /// <summary>
        /// UpdateProcessFileLog
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateProcessFileLog")]
        public IActionResult UpdateProcessFileLog(Domain.Models.ProccessFileLog processFileLog)
        {
            var processFileLogOld = db.ProccessFileLogs.Find(processFileLog.ProccessFileLogId);
            if (processFileLogOld == null)
            {
                return NotFound();
            }
            db.Entry(processFileLogOld).CurrentValues.SetValues(processFileLog);
            db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// DeleteProcessFileLog
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpDelete]
        [Route("DeleteProcessFileLog/{Id}")]
        public IActionResult DeleteProcessFileLog(int Id)
        {
            var processFileLog = db.ProccessFileLogs.Find(Id);
            if (processFileLog != null)
            {
                db.ProccessFileLogs.Remove(processFileLog);
                db.SaveChanges();
            }
            return NoContent();
        }
        #endregion
    }
}
