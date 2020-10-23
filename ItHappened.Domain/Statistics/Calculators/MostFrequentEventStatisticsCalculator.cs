﻿using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class MostFrequentEventStatisticsCalculator : IMultipleTrackersStatisticsCalculator
    {
        private readonly IEventRepository _eventRepository;

        public MostFrequentEventStatisticsCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Option<IMultipleTrackersFact> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            var trackers = eventTrackers.ToList();
            if (!CanCalculate(trackers))
            {
                return Option<IMultipleTrackersFact>.None;
            }

            var trackingNameWithEventsPeriod = trackers
                .Select(eventTracker => (trackingName: eventTracker.Name, eventsPeriod: 1.0 * (DateTime.Now -
                        _eventRepository.LoadAllTrackerEvents(eventTracker.Id)
                            .OrderBy(e => e.HappensDate)
                            .First()
                            .HappensDate)
                    .TotalDays / _eventRepository.LoadAllTrackerEvents(eventTracker.Id).Count)
                );

            var eventTrackersWithPeriods = trackingNameWithEventsPeriod.ToList();
            var (trackingName, eventsPeriod) = eventTrackersWithPeriods
                .OrderBy(e => e.eventsPeriod)
                .FirstOrDefault();
            return Option<IMultipleTrackersFact>.Some(new MostFrequentEventFact(
                "Самое частое событие",
                $"Чаще всего у вас происходит событие {trackingName} - раз в {eventsPeriod:0.#} дней",
                10 / eventsPeriod,
                trackingName,
                eventsPeriod
            ));
        }

        private bool CanCalculate(IReadOnlyCollection<EventTracker> eventTrackers)
        {
            return eventTrackers.Count > 1 &&
                   eventTrackers.All(tracker => _eventRepository.LoadAllTrackerEvents(tracker.Id).Count > 3);
        }
    }
}