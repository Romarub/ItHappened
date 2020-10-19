﻿using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class EventsCountCalculator : IMultipleTrackersStatisticsCalculator
    {
        private readonly IEventRepository _eventRepository;

        public EventsCountCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Option<IStatisticsFact> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            if (!CanCalculate(eventTrackers))
                return Option<IStatisticsFact>.None;

            var factName = "Зафиксировано уже N событий";
            var eventsCount = eventTrackers.Sum(tracker => _eventRepository.LoadAllTrackerEvents(tracker.Id).Count);
            var description = $"У вас произошло уже {eventsCount} событий!";
            var priority = Math.Log(eventsCount);

            return Option<IStatisticsFact>.Some(new EventsCountFact(factName, description, priority, eventsCount));
        }

        private bool CanCalculate(IEnumerable<EventTracker> eventTrackers)
        {
            var eventsCount = eventTrackers.Sum(tracker => _eventRepository.LoadAllTrackerEvents(tracker.Id).Count);
            return eventTrackers.Any() &&
                   eventsCount > 5;
        }
    }
}