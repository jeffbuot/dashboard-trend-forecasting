﻿using System;
using System.ComponentModel.DataAnnotations;
using SMPLX.ForecastingDashboard.Common;
using Volo.Abp.Application.Dtos;

namespace SMPLX.ForecastingDashboard.Cases
{
    public class CaseDto : FullAuditedEntityDto<Guid>
    {
        public int CaseId { get; set; }
        public DateTime DateRegistered { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Barangay { get; set; }
        public LifeStatus LifeStatus { get; set; }
        public Wellness Wellness { get; set; }
        public double Period { get; set; }
    }

    public class CaseGetListDto : ListResultDto<CaseDto>
    {
    }

    public class CaseInputDto
    {
        [Required] public int CaseId { get; set; }
        public string Barangay { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public LifeStatus LifeStatus { get; set; }
        public Wellness Wellness { get; set; }
        [Required] public DateTime DateRegistered { get; set; }
    }

    public class CaseImportDto : CaseDto
    {
        public string ErrorMessage { get; set; }
    }
}