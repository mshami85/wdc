using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StudentRegister.Classes;
using StudentRegister.Data;
using StudentRegister.Dtos;
using StudentRegister.Models;

namespace StudentRegister.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        AppSettings _appSettings;
        DataContext _dataContext;
        IMapper _mapper;
        public RegisterController(IOptionsSnapshot<AppSettings> appSettings, DataContext dataContext, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _dataContext = dataContext;
            _mapper = mapper;
        }

        [HttpGet]
        [Consumes("application/json")]
        public async Task<IActionResult> GetCourses()
        {
            try
            {
                var courses = await _dataContext.Courses.Include(c => c.Registerations).ToListAsync();
                var viewModels = courses.Select(c => _mapper.Map<CourseViewModel>(c));
                return Ok(viewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }

        [HttpGet("{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetCourse([FromRoute] int id)
        {
            try
            {
                var course = await _dataContext.Courses.Include(c => c.Registerations).FirstOrDefaultAsync(c => c.Id == id);
                if (course == null)
                {
                    return BadRequest("Course not found");
                }
                var viewModels = _mapper.Map<CourseViewModel>(course);
                return Ok(viewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseModel model)
        {
            try
            {
                var exists = await _dataContext.Courses.FirstOrDefaultAsync(c => c.Name == model.Name);
                if (exists != null)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, model.Name);
                }
                var course = _mapper.Map<Course>(model);
                await _dataContext.Courses.AddAsync(course);
                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var course = await _dataContext.Courses.FindAsync(model.CourseId);
                if (course == null)
                {
                    return BadRequest("Course not found");
                }

                var regeistered = await _dataContext.Registerations.Include(r => r.Course)
                                                                   .FirstOrDefaultAsync(r => r.CourseId == course.Id && r.AdmissionId == model.AdmissionId);
                if (regeistered != null)
                {
                    return Ok();
                }

                var reg = _mapper.Map<Registeration>(model);
                reg.RegisterationDate = DateTime.Now;
                reg.Course = course;

                await _dataContext.Registerations.AddAsync(reg);
                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRegisterations()
        {
            try
            {
                var reg = await _dataContext.Registerations.Include(r => r.Course).OrderBy(r => r.StudentName).ToListAsync();
                var regViewModels = reg.Select(r => _mapper.Map<RegisterViewModel>(r)).ToList();
                return Ok(regViewModels);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetDeepMessage());
            }
        }
    }
}
