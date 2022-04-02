using System;
using SMPLX.ForecastingDashboard.Common;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SMPLX.ForecastingDashboard.Cases
{
    public class Case : FullAuditedAggregateRoot<Guid>
    {
        public int CaseId { get; set; }
        public string Barangay { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public LifeStatus LifeStatus { get; set; }
        public Wellness Wellness { get; set; }
        public DateTime DateRegistered { get; set; }
    }
}