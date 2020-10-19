﻿using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public interface IStatisticsService
    {
        IReadOnlyCollection<IStatisticsFact> GetStatisticFacts(Guid userId);
        IReadOnlyCollection<IStatisticsFact> GetMultipleTrackersFacts(Guid userId);
        IReadOnlyCollection<IStatisticsFact> GetSingleTrackerFacts(Guid userId);
    }
}