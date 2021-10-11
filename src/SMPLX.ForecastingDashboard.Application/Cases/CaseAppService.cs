using System;
using SMPLX.ForecastingDashboard.Permissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SMPLX.ForecastingDashboard.Cases
{
    public class CaseAppService : CrudAppService<Case, CaseDto, Guid, CaseGetListDto, CaseInputDto>, ICaseAppService
    {
        public CaseAppService(IRepository<Case, Guid> repository) : base(repository)
        {
            CreatePolicyName = ForecastingDashboardPermissions.Case.Create;
            UpdatePolicyName = ForecastingDashboardPermissions.Case.Edit;
            DeletePolicyName = ForecastingDashboardPermissions.Case.Delete;
        }
    }
}