using AutoMapper;
using StudentAttendance.Dtos;
using StudentAttendance.Models;

namespace StudentAttendance.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SessionModel, Session>();
            CreateMap<Session, SessionViewModel>().AfterMap<SessionViewModelAction>();

            CreateMap<AttendanceModel, Attendance>();
            CreateMap<Attendance, AttendanceViewModel>().AfterMap<AttendanceViewModelAction>();

        }
    }

    class SessionViewModelAction : IMappingAction<Session, SessionViewModel>
    {
        IMapper _mapper;

        public SessionViewModelAction(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public void Process(Session source, SessionViewModel destination, ResolutionContext context)
        {
            destination.Attendances = source.Attendances.Select(a => _mapper.Map<AttendanceViewModel>(a));
        }
    }

    class AttendanceViewModelAction : IMappingAction<Attendance, AttendanceViewModel>
    {
        IMapper _mapper;

        public AttendanceViewModelAction(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public void Process(Attendance source, AttendanceViewModel destination, ResolutionContext context)
        {
            destination.CourseName = source.Session?.CourseName ?? string.Empty;
            destination.SessionDate = source.Session?.SessionDate;
            destination.SessionTitle = source.Session?.Title ?? string.Empty;
        }
    }
}
