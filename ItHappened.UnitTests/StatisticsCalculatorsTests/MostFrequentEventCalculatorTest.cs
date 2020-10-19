﻿using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class MostFrequentEventCalculatorTest
    {
        private IEventRepository _eventRepository;
        
        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
        }
        [Test]
        public void CreateTwoEventTrackersWithHeadacheAndToothacheEvents_GetMostFrequentEventFact_CheckAllProperties()
        {
            var userId = Guid.NewGuid();
            var eventTracker1 = EventTrackerBuilder
                .Tracker(userId, Guid.NewGuid(), "Pains after drinking sugar water")
                .Build();
            var eventTracker2 = EventTrackerBuilder
                .Tracker(userId, Guid.NewGuid(), "Pains after eating sugar")
                .Build();

            var headacheEventYesterday =
                CreateEventWithNameAndDateTime(userId, eventTracker1.Id,"headache", DateTimeOffset.Now.AddDays(-1));
            var headacheEventYesterdayAgain =
                CreateEventWithNameAndDateTime(userId, eventTracker1.Id,"headache", DateTimeOffset.Now.AddDays(-1));
            var headacheEventTwoDaysAgo =
                CreateEventWithNameAndDateTime(userId, eventTracker1.Id,"headache", DateTimeOffset.Now.AddDays(-2));
            var headacheEventThreeDaysAgo =
                CreateEventWithNameAndDateTime(userId, eventTracker1.Id,"headache", DateTimeOffset.Now.AddDays(-3));

            var toothacheEventYesterday =
                CreateEventWithNameAndDateTime(userId, eventTracker2.Id,"toothache", DateTimeOffset.Now.AddDays(-1));
            var toothacheEventTwoDaysAgo =
                CreateEventWithNameAndDateTime(userId, eventTracker2.Id,"toothache", DateTimeOffset.Now.AddDays(-2));
            var toothacheEventTwoDaysAgoAgain =
                CreateEventWithNameAndDateTime(userId, eventTracker2.Id,"toothache", DateTimeOffset.Now.AddDays(-2));
            _eventRepository.AddRangeOfEvents(new []
            {
                headacheEventYesterday, headacheEventYesterdayAgain, headacheEventTwoDaysAgo,
                headacheEventThreeDaysAgo, toothacheEventYesterday, toothacheEventTwoDaysAgo, 
                headacheEventYesterday, toothacheEventTwoDaysAgoAgain 
            });
           var mostFrequentEvent = new MostFrequentEventCalculator(_eventRepository).Calculate(new[] {eventTracker1, eventTracker2})
                .ConvertTo<MostFrequentEventFact>();

            Assert.IsTrue(mostFrequentEvent.IsSome);
            mostFrequentEvent.Do(e =>
            {
                Assert.AreEqual("Чаще всего у вас происходит событие \"Pains after eating sugar\"" +
                                " - раз в 0.4 дней", e.Description);
                Assert.AreEqual(25, e.Priority);
                Assert.AreEqual("Pains after eating sugar", e.TrackingName);
                Assert.AreEqual(0.4, e.EventsPeriod, 0.01);
                Assert.AreEqual(0.75, e.EventTrackersWithPeriods
                    .First(q => q.TrackingName == "Pains after drinking sugar water")
                    .EventPeriod, 0.01);
            });
        }

        private static Event CreateEventWithNameAndDateTime(Guid userId, Guid trackerId, string title, DateTimeOffset dateTime)
        {
            return EventBuilder.Event(Guid.NewGuid(), userId, trackerId, dateTime, title).Build();
        }
    }
}