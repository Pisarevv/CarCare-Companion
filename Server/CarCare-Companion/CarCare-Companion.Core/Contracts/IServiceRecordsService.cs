﻿namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.ServiceRecords;

public interface IServiceRecordsService
{
    public Task<string> CreateAsync(string userId, ServiceRecordFormRequestModel model);
}