using AutoMapper;
using StudentAdmission.Dtos;
using StudentAdmission.Models;

namespace StudentAdmission.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AdmissionModel, Admission>();
            CreateMap<Admission, AdmissionViewModel>().AfterMap<AdmissionViewModelAction>();

            CreateMap<AttachmentFile, AttachmentFileViewModel>();
        }
    }

    class AdmissionViewModelAction : IMappingAction<Admission, AdmissionViewModel>
    {
        IMapper _mapper;

        public AdmissionViewModelAction(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public void Process(Admission source, AdmissionViewModel destination, ResolutionContext context)
        {
            destination.AttachmentFiles = source.Attachments.Select(at => _mapper.Map<AttachmentFileViewModel>(at)).ToList();
        }
    }
}
