using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StudentAttendance.Classes;
using StudentAttendance.Data;
using StudentAttendance.Dtos;
using StudentAttendance.Models;

namespace StudentAttendance.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        AppSettings _appSettings;
        DataContext _dataContext;
        IMapper _mapper;
        public AttendanceController(IOptionsSnapshot<AppSettings> appSettings, DataContext dataContext, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _dataContext = dataContext;
            _mapper = mapper;
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateSession([FromBody] SessionModel model)
        {
            try
            {
                var session = _mapper.Map<Session>(model);
                if (string.IsNullOrEmpty(session.CourseName))
                {
                    return BadRequest("course not named");
                }
                if (session.SessionDate == null)
                {
                    return BadRequest("Session date not specified");
                }
                if (_dataContext.Sessions.Any(s => s.CourseName == model.CourseName && s.Title == model.Title))
                {
                    return Ok();
                }
                await _dataContext.Sessions.AddAsync(session);
                await _dataContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }

        [HttpGet]
        [Consumes("application/json")]
        public async Task<IActionResult> GetSessions()
        {
            try
            {
                var sessions = await _dataContext.Sessions.Include(s => s.Attendances).ToListAsync();
                var sessionViewModel = sessions.Select(s => _mapper.Map<SessionViewModel>(s)).ToList();
                return Ok(sessionViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }

        [HttpGet("{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetSession([FromRoute] int id)
        {
            try
            {
                var session = await _dataContext.Sessions.Include(s => s.Attendances).FirstOrDefaultAsync(s => s.Id == id);
                if (session == null)
                {
                    return BadRequest("Session not found");
                }
                var sessionViewModel = _mapper.Map<SessionViewModel>(session);
                return Ok(sessionViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }


        [HttpGet("{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetCourseSessions([FromRoute] int id)
        {
            try
            {
                var sessions = await _dataContext.Sessions.Include(s => s.Attendances).Where(s => s.CourseId == id).ToListAsync();
                if (sessions == null)
                {
                    return BadRequest("Session not found");
                }
                var sessionsViewModel = sessions.Select(s => _mapper.Map<SessionViewModel>(s));
                return Ok(sessionsViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }


        [HttpPut("{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> AttendSession([FromRoute] int id, [FromBody] AttendanceModel model)
        {
            try
            {
                var session = await _dataContext.Sessions.Include(s => s.Attendances).FirstOrDefaultAsync(s => s.Id == id);
                if (session == null)
                {
                    return BadRequest("Session not found");
                }
                if (session.Attendances.Any(a => a.AdmissionId == model.AdmissionId))
                {
                    return BadRequest("Already registered");
                }
                if (_dataContext.Attendances.Any(a => a.AdmissionId == model.AdmissionId && a.SessionId == id))
                {
                    return Ok();
                }
                var attendance = _mapper.Map<Attendance>(model);
                session.Attendances.Add(attendance);

                _dataContext.Sessions.Update(session);
                await _dataContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }

        [HttpGet("{admissionId}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAttendance([FromRoute] int admissionId)
        {
            try
            {
                var attendees = await _dataContext.Attendances.Include(a => a.Session).Where(a => a.AdmissionId == admissionId).ToListAsync();

                var attendeesViewModel = attendees.Select(a => _mapper.Map<AttendanceViewModel>(a));
                return Ok(attendeesViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }
    }
}
