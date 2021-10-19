using AutoMapper;
using SMPLX.ForecastingDashboard.Cases;
using SMPLX.ForecastingDashboard.ForecastData;
using SMPLX.ForecastingDashboard.Settings;

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
            CreateMap<CaseDto, CaseImportDto>();
            CreateMap<CaseImportDto, CaseInputDto>();
            CreateMap<Location, HeatMapDto>();
        }
    }
}