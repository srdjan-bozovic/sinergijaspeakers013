﻿using MsCampus.Win8.Shared.Contracts.Services;
using SinergijaSpeakers013.Contracts.Models;
using SinergijaSpeakers013.Contracts.Repositories;
using SinergijaSpeakers013.Contracts.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SinergijaSpeakers013.Repositories
{
    public class ConferenceRepository : IConferenceRepository
    {
        private const string ConferenceDataKey = "conferenceData";

        private IConferenceDataService _conferenceDataService;
        private ICacheService _cacheService;

        public ConferenceRepository(
            IConferenceDataService conferenceDataService,
            ICacheService cacheService)
        {
            _conferenceDataService = conferenceDataService;
            _cacheService = cacheService;
        }

        public async Task<List<Speaker>> GetSpeakersAsync()
        {
            var cts = new CancellationTokenSource();
            return await GetSpeakersAsync(cts.Token);
        }

        public async Task<List<Speaker>> GetSpeakersAsync(CancellationToken cancellationToken)
        {
            var item = await _cacheService.GetAsync<ConferenceDataModel>(ConferenceDataKey).ConfigureAwait(false);
            if (item.HasValue)
            {
                if (cancellationToken.IsCancellationRequested)
                    return item.Value.Speakers;
                var versionId = item.Value.Version;
                var latestVersionId = await _conferenceDataService.GetVersionAsync(cancellationToken).ConfigureAwait(false);
                if (versionId >= latestVersionId)
                {
                    return item.Value.Speakers;
                }
            }
            var data = await _conferenceDataService.GetConfDataAsync(cancellationToken).ConfigureAwait(false);
            await _cacheService.PutAsync(ConferenceDataKey, data).ConfigureAwait(false);
            return data.Speakers;
        }
    }
}
