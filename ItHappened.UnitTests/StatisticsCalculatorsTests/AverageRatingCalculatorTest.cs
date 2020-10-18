﻿using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using LanguageExt;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class AverageRatingCalculatorTest
    {
        [Test]
        public void EventTrackerHasTwoRatingAndEvents_CalculateSuccess()
        {
            //arrange 
            var ratings = new List<double> {2.0, 5.0};
            var eventList = CreateTwoEvents(ratings);
            var eventTracker = EventTrackerBuilder
                .TrackerEmpty(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .WithRating()
                .Build();
            foreach (var @event in eventList)
            {
                eventTracker.TryAddEvent(@event);
            }

            //act 
            var fact = new AverageRatingCalculator().Calculate(eventTracker);
            var averageFact = fact.ConvertTo<AverageRatingFact>();
            //assert 
            Assert.True(fact.IsSome);
            averageFact.Do(f =>
            {
                Assert.AreEqual(Math.Sqrt(ratings.Average()), f.Priority);
                Assert.AreEqual(ratings.Average(), f.AverageRating);
            });
        }

        [Test]
        public void EventTrackerHasNoRationCustomization_CalculateFailed()
        {
            //arrange 
            var eventList = CreateTwoEvents();
            var eventTracker = EventTrackerBuilder
                .TrackerEmpty(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .Build();
            foreach (var @event in eventList)
            {
                eventTracker.TryAddEvent(@event);
            }

            //act 
            var fact = new AverageRatingCalculator().Calculate(eventTracker);

            //assert 
            Assert.True(fact.IsNone);
        }

        [Test]
        public void EventTrackerHasOneEvent_CalculateFailed()
        {
            //arrange 
            var ratings = new List<double> {2.0};
            var @event = EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event1").WithRating(ratings[0])
                .Build();
            var eventTracker = EventTrackerBuilder
                .TrackerEmpty(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .WithRating()
                .Build();
            eventTracker.TryAddEvent(@event);

            //act 
            var fact = new AverageRatingCalculator().Calculate(eventTracker);

            //assert 
            Assert.True(fact.IsNone);
        }


        [Test]
        public void SomeEventHasNoCustomizationRating_CalculateFailed()
        {
            //arrange 
            var eventList = CreateTwoEventOneOfThemWithoutRating();
            var eventTracker = EventTrackerBuilder
                .TrackerEmpty(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .Build();
            foreach (var @event in eventList)
            {
                eventTracker.TryAddEvent(@event);
            }

            //act 
            var fact = new AverageRatingCalculator().Calculate(eventTracker);

            //assert 
            Assert.True(fact.IsNone);
        }

        private static List<Event> CreateTwoEventOneOfThemWithoutRating()
        {
            var ratings = new List<double> {2.0};
            return new List<Event>
            {
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event1")
                    .Build(),
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event2")
                    .WithRating(ratings[0])
                    .Build(),
            };
        }

        private static List<Event> CreateTwoEvents()
        {
            var ratings = new List<double> {2.0, 5.0};
            return new List<Event>
            {
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event1").WithRating(ratings[0])
                    .Build(),
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event1").WithRating(ratings[1])
                    .Build()
            };
        }

        private static List<Event> CreateTwoEvents(IList<double> ratings)
        {
            return new List<Event>
            {
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event1").WithRating(ratings[0])
                    .Build(),
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, "Event1").WithRating(ratings[1])
                    .Build()
            };
        }
    }
}