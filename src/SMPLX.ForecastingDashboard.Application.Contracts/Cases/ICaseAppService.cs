using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SMPLX.ForecastingDashboard.Cases
{
    public interface ICaseAppService : ICrudAppService<CaseDto, Guid, CaseGetListDto, CaseInputDto>
    {
        Task<IEnumerable<CaseDto>> CreateManyAsync(IEnumerable<CaseInputDto> cases);
    }
}