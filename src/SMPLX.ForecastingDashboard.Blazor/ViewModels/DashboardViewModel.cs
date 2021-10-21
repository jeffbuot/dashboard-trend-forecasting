using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMPLX.ForecastingDashboard.Cases;
using Volo.Abp.DependencyInjection;

namespace SMPLX.ForecastingDashboard.Blazor.ViewModels
{
    public class DashboardViewModel : ForecastingDashboardComponentBase
    {
        protected IReadOnlyList<CaseDto> Cases { get; set; }
        protected ICaseAppService CaseAppService => LazyGetRequiredService(ref _caseAppService);
        protected DateTime OldestRecordDate = DateTime.Now.AddYears(-5);

        private ICaseAppService _caseAppService;

        public DashboardViewModel() {
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadCaseData();
            OldestRecordDate = Cases.Min(_ => _.DateRegistered);
        }

        protected async Task LoadCaseData()
        {
            var res = await CaseAppService.GetListAsync(new CaseGetListDto());
            Cases = res.Items;
        }

        protected string FlattenString(string s) => s.ToLower().Replace(" ", "");

        protected bool FlatMatch(string baseString, string searchString) =>
            FlattenString(baseString).StartsWith(FlattenString(searchString));
    }
}