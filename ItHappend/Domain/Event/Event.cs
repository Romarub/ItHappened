﻿using System;

namespace ItHappend.Domain
{
    public class Event
    {
        public const double MaxEvaluationValue = 5.0;
        public const double MinEvaluationValue = 0.0;

        public Guid Id { get; }
        public Guid CreatorId { get; }
        public DateTimeOffset HappensDate { get; set; }
        public string Title { get; set; }

        private double _evaluation;

        public double Evaluation
        {
            get => _evaluation;
            set
            {
                if (value < MinEvaluationValue)
                {
                    _evaluation = MinEvaluationValue;
                    return;
                }

                if (value > MaxEvaluationValue)
                {
                    _evaluation = MaxEvaluationValue;
                    return;
                }
                _evaluation = value;
            }
        }

        public Optional<Photo> Photo { get; set; }
        public Optional<Scale> Scale { get; set; }
        public Optional<Rating> Rating { get; set; }
        public Optional<GeoTag> GeoTag { get; set; }
        public Optional<Comment> Comment { get; set; }

        public Event(EventBuilder eventBuilder)
        {
            CreatorId = eventBuilder.CreatorId;
            Id = eventBuilder.Id;
            Title = eventBuilder.Title;
            HappensDate = eventBuilder.HappensDate;
            Evaluation = eventBuilder.Evaluation;
            Photo = eventBuilder.Photo;
            Scale = eventBuilder.Scale;
            Rating = eventBuilder.Rating;
            GeoTag = eventBuilder.GeoTag;
            Comment = eventBuilder.Comment;
        }

        public Event(Guid eventId, Guid creatorId, string title, double evaluation)
        {
            CreatorId = creatorId;
            Id = eventId;
            Title = title;
            HappensDate = DateTimeOffset.Now;
            Evaluation = evaluation;
        }
    }
}