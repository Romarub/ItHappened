﻿using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class MultipleTrackersStatisticsEventsCountCalculator : IMultipleTrackersStatisticsCalculator
    {
        private readonly IEventRepository _eventRepository;

        public MultipleTrackersStatisticsEventsCountCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Option<IMultipleTrackersFact> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            if (!CanCalculate(eventTrackers))
                return Option<IMultipleTrackersFact>.None;

            var factName = "Зафиксировано уже N событий";
            var eventsCount = eventTrackers.Sum(tracker => _eventRepository.LoadAllTrackerEvents(tracker.Id).Count);
            var description = $"У вас произошло уже {eventsCount} событий!";
            var priority = Math.Log(eventsCount);

            return Option<IMultipleTrackersFact>.Some(new EventsCountFact(factName, description, priority, eventsCount));
        }

        private bool CanCalculate(IEnumerable<EventTracker> eventTrackers)
        {
            var eventsCount = eventTrackers.Sum(tracker => _eventRepository.LoadAllTrackerEvents(tracker.Id).Count);
            return eventTrackers.Any() &&
                   eventsCount > 5;
        }
    }
}