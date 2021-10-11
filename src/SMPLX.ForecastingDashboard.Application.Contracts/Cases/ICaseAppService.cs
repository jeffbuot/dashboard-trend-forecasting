using System;
using Volo.Abp.Application.Services;

namespace SMPLX.ForecastingDashboard.Cases
{
    public interface ICaseAppService : ICrudAppService<CaseDto, Guid, CaseGetListDto, CaseInputDto>
    {
    }
}