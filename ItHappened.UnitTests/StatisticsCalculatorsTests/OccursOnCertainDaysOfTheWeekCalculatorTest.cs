﻿using System;
using System.Collections.Generic;
using System.Linq;
using ItHappend.Domain.Statistics;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class OccursOnCertainDaysOfTheWeekCalculatorTest
    {
        private IEventRepository _eventRepository;
        
        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
        }
        [Test]
        public void EventTrackerHasTwoRatingAndEvents_CalculateSuccess()
        {
            //arrange 
            var eventTracker = CreateTracker();
            var eventList = CreateEvents_10Events_7OnMonday_3OnWednesday_1Tuesday(eventTracker.Id);
            _eventRepository.AddRangeOfEvents(eventList);

            //act 
            var fact = new OccursOnCertainDaysOfTheWeekCalculator(_eventRepository)
                .Calculate(eventTracker).ConvertTo<OccursOnCertainDaysOfTheWeekFact>();
           
            //assert 
            Assert.True(fact.IsSome);

            fact.Do(f =>
            {
                Assert.AreEqual("Происходит в определённые дни недели", f.FactName);
                Assert.AreEqual("В 90% случаев событие TrackerName происходит в понедельник, в среду", f.Description);
                Assert.AreEqual(12.6, f.Priority, 1e-5);
                Assert.AreEqual(new[] {DayOfWeek.Monday, DayOfWeek.Wednesday}, f.DaysOfTheWeek);
                Assert.AreEqual(90.0, f.Percentage, 1e-5);
            });
        }
        
        [Test]
        public void AllEventNotPasses_25PercentHitToWeekOfDayThreshold_CalculateFailure()
        {
            //arrange 
            var eventTracker = CreateTracker();
            var eventList = CreateEvents_10Events_7OnMonday_3OnWednesday_1Tuesday(eventTracker.Id);
            _eventRepository.AddRangeOfEvents(eventList);

            //act 
            var fact = new OccursOnCertainDaysOfTheWeekCalculator(_eventRepository)
                .Calculate(eventTracker).ConvertTo<OccursOnCertainDaysOfTheWeekFact>();

            //assert 
            Assert.True(fact.IsSome);

            fact.Do(f =>
            {
                Assert.AreEqual("Происходит в определённые дни недели", f.FactName);
                Assert.AreEqual("В 90% случаев событие TrackerName происходит в понедельник, в среду", f.Description);
                Assert.AreEqual(12.6, f.Priority, 1e-5);
                Assert.AreEqual(new[] {DayOfWeek.Monday, DayOfWeek.Wednesday}, f.DaysOfTheWeek);
                Assert.AreEqual(90.0, f.Percentage, 1e-5);
            });
        }


        [Test]
        public void NotEnoughEvents_CalculateFailure()
        {
            //arrange 
            var eventTracker = CreateTracker();
            var eventList = CreateOneEventOnEveryDay(eventTracker.Id);
            _eventRepository.AddRangeOfEvents(eventList);

            //act 
            var fact = new OccursOnCertainDaysOfTheWeekCalculator(_eventRepository)
                .Calculate(eventTracker).ConvertTo<OccursOnCertainDaysOfTheWeekFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        private static EventTracker CreateTracker()
        {
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .Build();
            return eventTracker;
        }

        private static List<Event> CreateEvents_10Events_7OnMonday_3OnWednesday_1Tuesday(Guid trackerId)
        {
            var monday = new DateTime(2020, 10, 5);
            var tuesday = new DateTime(2020, 10, 6);
            var wednesday = new DateTime(2020, 10, 7);
            var dateList = new List<DateTimeOffset>
            {
                new DateTimeOffset(monday),
                new DateTimeOffset(monday),
                new DateTimeOffset(monday),
                new DateTimeOffset(monday),
                new DateTimeOffset(monday),
                new DateTimeOffset(monday),
                new DateTimeOffset(tuesday),
                new DateTimeOffset(wednesday),
                new DateTimeOffset(wednesday),
                new DateTimeOffset(wednesday),
            };
            return dateList
                .Select((t, i) =>
                    EventBuilder.Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, DateTimeOffset.Now , $"Event_{i}").Build())
                .ToList();
        }
        
        private static List<Event> CreateOneEventOnEveryDay(Guid trackerId)
        {
            var monday = new DateTime(2020, 10, 5);
            var dateList = new List<DateTimeOffset>
            {
                new DateTimeOffset(monday),
                new DateTimeOffset(monday.AddDays(1)),
                new DateTimeOffset(monday.AddDays(2)),
                new DateTimeOffset(monday.AddDays(3)),
                new DateTimeOffset(monday.AddDays(4)),
                new DateTimeOffset(monday.AddDays(5)),
                new DateTimeOffset(monday.AddDays(6)),
            };
            return dateList
                .Select((t, i) =>
                    EventBuilder.Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, DateTimeOffset.Now,  $"Event_{i}").Build())
                .ToList();
        }
    }
}

