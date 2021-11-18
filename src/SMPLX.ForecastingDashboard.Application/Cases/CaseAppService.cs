using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMPLX.ForecastingDashboard.Permissions;
using Volo.Abp.Application.Dtos;
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

        // public override async Task<PagedResultDto<CaseDto>> GetListAsync(CaseGetListDto input)
        // {
        //     await CheckGetListPolicyAsync();
        //
        //     var query = await CreateFilteredQueryAsync(input);
        //
        //     var totalCount = await AsyncExecuter.CountAsync(query);
        //
        //     query = ApplySorting(query, input);
        //     query = ApplyPaging(query, input);
        //
        //     var entities = await AsyncExecuter.ToListAsync(query);
        //     var entityDtos = await MapToGetListOutputDtosAsync(entities);
        //     double i = 1;
        //     var res = entityDtos.OrderBy(_ => _.DateRegistered).ToList();
        //     res.ForEach(_ => _.Period = i++);
        //     return new PagedResultDto<CaseDto>(
        //         totalCount,
        //         res
        //     );
        // }

        public async Task<IEnumerable<CaseDto>> CreateManyAsync(IEnumerable<CaseInputDto> cases)
        {
            await CheckCreatePolicyAsync();

            var entities = new List<Case>();
            foreach (var c in cases)
            {
                var entity = await MapToEntityAsync(c);
                TryToSetTenantId(entity);
                entities.Add(entity);
            }

            await Repository.InsertManyAsync(entities, true);

            return await MapToGetListOutputDtosAsync(entities);
        }
    }
}