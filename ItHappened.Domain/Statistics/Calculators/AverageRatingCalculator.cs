﻿using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics
{
    public class AverageRatingCalculator : ISingleTrackerStatisticsCalculator
    {
        public Option<IStatisticsFact> Calculate(EventTracker eventTracker)
        {
            
            var a = new List<Option<int>>();
            // var c = new Option<int>.Some(10);
            a.Add(Option<int>.Some(10));
            a.Add(Option<int>.None);
            a.Add(Option<int>.Some(10));
            a.Add(Option<int>.Some(10));
            a.Add(Option<int>.None);
            var c = a.Where(x => x.IsSome).Select(x=>x.ValueUnsafe()).ToList();
            var c1 = a.Somes().ToList();
            
            
            if (!CanCalculate(eventTracker)) return Option<IStatisticsFact>.None;
            var averageRating = eventTracker.Events.Average(x => x.Rating.ValueUnsafe());
            return Option<IStatisticsFact>.Some(new AverageRatingFact(
                "Среднее значение оценки",
                $"Средний рейтинг для события {eventTracker.Name} равен {averageRating}",
                Math.Sqrt(averageRating),
                averageRating
            ));
        }
        
        private bool CanCalculate(EventTracker eventTracker)
        {
            if (!eventTracker.HasRating)
            {
                return false;
            }

            if (eventTracker.Events.Any(@event => @event.Rating == Option<double>.None))
            {
                return false;
            }

            return eventTracker.Events.Count > 1;
        }
    }
}