﻿using SinergijaSpeakers013.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SinergijaSpeakers013.Contracts.Services.Data
{
    public interface IConferenceDataService
    {
        Task<ConferenceDataModel> GetConfDataAsync(CancellationToken cancellationToken);
        Task<int> GetVersionAsync(CancellationToken cancellationToken);
    }
}
