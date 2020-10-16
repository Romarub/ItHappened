﻿using System;
using ItHappend.Domain;

namespace ItHappend.EventService
{
    public interface IEventService
    {
        (Event @event, EventServiceStatusCodes operationStatus) TryGetEvent(Guid eventId, Guid eventCreatorId);
        EventServiceStatusCodes CreateEvent(Event newEvent);
        EventServiceStatusCodes TryEditEvent(Guid eventId, Guid eventCreatorId, Event newEvent);
        EventServiceStatusCodes TryDeleteEvent(Guid eventId, Guid creatorId);
    }
}