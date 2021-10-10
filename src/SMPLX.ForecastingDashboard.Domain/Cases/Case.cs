using System;
using SMPLX.ForecastingDashboard.Common;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SMPLX.ForecastingDashboard.Cases
{
    public class Case : AuditedAggregateRoot<Guid>
    {
        public int CaseId { get; set; }
        public string Barangay { get; set; }
        public int Age { get; set; }
        public LifeStatus LifeStatus { get; set; }
        public DateTime DateRegistered { get; set; }
    }
}