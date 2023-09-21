using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StudentAdmission.Classes;
using StudentAdmission.Data;

namespace StudentAttendance.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        AppSettings _appSettings;
        DataContext _dataContext;
        public RegisterController(IOptionsSnapshot<AppSettings> appSettings, DataContext dataContext)
        {
            _appSettings = appSettings.Value;
            _dataContext = dataContext;
        }

        [HttpGet("{studentId}")]
        [Consumes("application/json")]
        public IActionResult Get([FromRoute] int studentId)
        {
            return Ok("reg: " + studentId + 1);
        }
    }
}
