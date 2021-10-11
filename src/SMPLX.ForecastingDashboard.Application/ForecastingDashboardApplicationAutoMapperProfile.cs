using AutoMapper;
using SMPLX.ForecastingDashboard.Cases;

namespace SMPLX.ForecastingDashboard
{
    public class ForecastingDashboardApplicationAutoMapperProfile : Profile
    {
        public ForecastingDashboardApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<Case, CaseDto>().ReverseMap();
            CreateMap<CaseInputDto, Case>();
            CreateMap<CaseDto, CaseInputDto>();
        }
    }
}