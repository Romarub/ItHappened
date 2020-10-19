﻿using System;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class SingleTrackerEventsCountCalculator : ISingleTrackerStatisticsCalculator
    {
        private bool CanCalculate(EventTracker eventTracker) => 
            eventTracker.Events.Count > 0;

        public Option<IStatisticsFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker))
                return Option<IStatisticsFact>.None;

            var factName = "Количество событий";
            var eventsCount = eventTracker.Events.Count;
            var description  = $"Событие {eventTracker.Name} произошло {eventsCount} раз";
            var priority = Math.Log(eventsCount);
            
            return Option<IStatisticsFact>
                .Some(new SingleTrackerEventsCountFact(factName, description, priority, eventsCount));
        }
    }
}