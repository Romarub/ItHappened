﻿using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface IMultipleTrackersStatisticsCalculatorContainer
    {
        void Add(IMultipleTrackersStatisticsCalculator calculator);

        IReadOnlyCollection<Option<IMultipleTrackersStatisticsFact>> GetFacts(
            IEnumerable<EventTracker> eventTrackers);
    }

    public class MultipleTrackersStatisticsCalculatorContainer : IMultipleTrackersStatisticsCalculatorContainer
    {
        public void Add(IMultipleTrackersStatisticsCalculator calculator)
        {
            _calculators.Add(calculator);
        }
        
        private List<IMultipleTrackersStatisticsCalculator> _calculators =
            new List<IMultipleTrackersStatisticsCalculator>();

        public IReadOnlyCollection<Option<IMultipleTrackersStatisticsFact>> GetFacts(
            IEnumerable<EventTracker> eventTrackers) =>
            _calculators.Select(calculator => calculator.Calculate(eventTrackers)).ToList();
    }
}