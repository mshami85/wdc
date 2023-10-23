using AutoMapper;
using StudentRegister.Dtos;
using StudentRegister.Models;

namespace StudentRegister.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CourseModel, Course>();
            CreateMap<Course, CourseViewModel>().AfterMap<CourseViewModelAction>();


            CreateMap<RegisterModel, Registeration>();
            CreateMap<Registeration, RegisterViewModel>().AfterMap((reg, regView) => regView.CourseName = reg.Course?.Name);
        }
    }


    public class CourseViewModelAction : IMappingAction<Course, CourseViewModel>
    {
        IMapper _mapper;
        public CourseViewModelAction(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void Process(Course source, CourseViewModel destination, ResolutionContext context)
        {
            destination.Registerations = source.Registerations?.Select(r => _mapper.Map<RegisterViewModel>(r));
        }
    }

}
