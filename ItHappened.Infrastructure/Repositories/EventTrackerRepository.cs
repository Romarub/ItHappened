﻿using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.Repositories
{
    public class EventTrackerRepository : IEventTrackerRepository
    {
        private readonly Dictionary<Guid, EventTracker> _eventTrackers = new Dictionary<Guid, EventTracker>();

        public void SaveTracker(EventTracker newTracker)
        {
            _eventTrackers[newTracker.Id] = newTracker;
        }

        public EventTracker LoadTracker(Guid eventTrackerId)
        {
            return _eventTrackers[eventTrackerId];
        }

        public bool IsContainTracker(Guid trackerId)
        {
            return _eventTrackers.ContainsKey(trackerId);
        }
        
        public IEnumerable<EventTracker> LoadAllUserTrackers(Guid userId)
        {
            return _eventTrackers
                .Values.Where(tracker => tracker.CreatorId == userId).ToList();
        }

        public bool DeleteTracker(Guid eventId)
        {
            return _eventTrackers.Remove(eventId);
        }

        public IEnumerable<EventTracker> LoadAllTrackers()
        {
            return _eventTrackers.Values;
        }
    }
}