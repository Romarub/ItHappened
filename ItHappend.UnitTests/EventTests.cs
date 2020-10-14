using System;
using ItHappend.Domain;
using NUnit.Framework;

namespace ItHappend.UnitTests
{
    public class EventTests
    {
        private Guid _eventId;
        
        [Test]
        public void CreationEventWithRequiredParameters()
        {
            //arrange
            _eventId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            const string title = "Title";
            const double evaluation = 0.5;

            //act
            var newEvent = EventBuilder.Event(_eventId, creatorId, date, title, evaluation).Build();

            //assert
            Assert.AreEqual(_eventId, newEvent.Id);
            Assert.AreEqual(creatorId, newEvent.CreatorId);
            Assert.AreEqual(date, newEvent.HappensDate);
            Assert.AreEqual(title, newEvent.Title);
            Assert.AreEqual(evaluation, newEvent.Evaluation);

            Assert.IsFalse(newEvent.Comment.HasValue);
            Assert.IsFalse(newEvent.Scale.HasValue);
            Assert.IsFalse(newEvent.Photo.HasValue);
            Assert.IsFalse(newEvent.Raiting.HasValue);
            Assert.IsFalse(newEvent.GeoTag.HasValue);
        }

        [Test]
        public void CreationEventWithAllOptionalParameters()
        {
            //arrange
            var eventId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            const string title = "Title";
            const double evaluation = 0.5;
            const string textComment = "Comment For Event";
            const double scale = 15;
            byte[] photo = {0x1, 0x2, 0x3};
            const double rating = 299;
            var geoTag = new GeoTag(55.790514, 37.584822);


            //act
            var newEvent = EventBuilder
                .Event(eventId, creatorId, date, title, evaluation)
                .WithComment(textComment)
                .WithScale(scale)
                .WithPhoto(photo)
                .WithRating(rating)
                .WithGeoTag(geoTag)
                .Build();

            //assert
            Assert.AreEqual(eventId, newEvent.Id);
            Assert.AreEqual(creatorId, newEvent.CreatorId);
            Assert.AreEqual(date, newEvent.HappensDate);
            Assert.AreEqual(title, newEvent.Title);
            Assert.AreEqual(evaluation, newEvent.Evaluation);

            Assert.IsTrue(newEvent.Comment.HasValue);
            Assert.IsTrue(newEvent.Scale.HasValue);
            Assert.IsTrue(newEvent.Photo.HasValue);
            Assert.IsTrue(newEvent.Raiting.HasValue);
            Assert.IsTrue(newEvent.GeoTag.HasValue);

            newEvent.Comment.Do(val => Assert.AreEqual(textComment, val.Text));
            newEvent.Scale.Do(val => Assert.AreEqual(scale, val.Value));
            newEvent.Photo.Do(val => Assert.AreEqual(photo, val.PhotoBytes));
            newEvent.Raiting.Do(val => Assert.AreEqual(rating, val.Value));
            newEvent.GeoTag.Do(val => Assert.AreEqual(geoTag, val));
        }


        [Test]
        public void CreationEventWithNullTitle()
        {
            //arrange
            var eventId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            const string title = null;
            const double evaluation = 0.5;

            //act

            //assert
            Assert.Throws<NullReferenceException>(() =>
                EventBuilder.Event(eventId, creatorId, date, title, evaluation).Build()
            );
        }


        [Test]
        public void CreationEventAllParametersButSkipPhotoParameter()
        {
            //arrange
            var eventId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            const string title = "Title";
            const double evaluation = 0.5;
            const string textComment = "Comment For Event";
            const double scale = 15;
            const double rating = 299;
            var geoTag = new GeoTag(55.790514, 37.584822);


            //act
            var newEvent = EventBuilder
                .Event(eventId, creatorId, date, title, evaluation)
                .WithComment(textComment)
                .WithScale(scale)
                .WithRating(rating)
                .WithGeoTag(geoTag)
                .Build();

            //assert
            Assert.AreEqual(eventId, newEvent.Id);
            Assert.AreEqual(creatorId, newEvent.CreatorId);
            Assert.AreEqual(date, newEvent.HappensDate);
            Assert.AreEqual(title, newEvent.Title);
            Assert.AreEqual(evaluation, newEvent.Evaluation);

            Assert.IsTrue(newEvent.Comment.HasValue);
            Assert.IsTrue(newEvent.Scale.HasValue);
            Assert.IsFalse(newEvent.Photo.HasValue);
            Assert.IsTrue(newEvent.Raiting.HasValue);
            Assert.IsTrue(newEvent.GeoTag.HasValue);

            newEvent.Comment.Do(val => Assert.AreEqual(textComment, val.Text));
            newEvent.Scale.Do(val => Assert.AreEqual(scale, val.Value));
            newEvent.Photo.Do(val => Assert.Pass());
            newEvent.Raiting.Do(val => Assert.AreEqual(rating, val.Value));
            newEvent.GeoTag.Do(val => Assert.AreEqual(geoTag, val));
        }

        [Test]
        public void CreationEventEvaluationLessThenInRange()
        {
            //arrange
            _eventId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            const string title = "Title";
            const double lessThenMinEvaluation = Event.MinEvaluationValue - 1.0;
            const double expectedEvaluation = 0.0;

            //act
            var newEventLessThenMinEvaluation =
                EventBuilder.Event(_eventId, creatorId, date, title, lessThenMinEvaluation).Build();

            //assert
            Assert.AreEqual(_eventId, newEventLessThenMinEvaluation.Id);
            Assert.AreEqual(creatorId, newEventLessThenMinEvaluation.CreatorId);
            Assert.AreEqual(date, newEventLessThenMinEvaluation.HappensDate);
            Assert.AreEqual(title, newEventLessThenMinEvaluation.Title);
            Assert.AreEqual(expectedEvaluation, newEventLessThenMinEvaluation.Evaluation);
        }

        [Test]
        public void CreationEventEvaluationMoreThenInRange()
        {
            //arrange
            _eventId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var date = DateTimeOffset.Now;
            const string title = "Title";
            const double moreThenMaxEvaluation = Event.MaxEvaluationValue + 1.0;
            const double expectedEvaluation = 0.0;

            //act
            var newEventMoreThenMaxEvaluation =
                EventBuilder.Event(_eventId, creatorId, date, title, moreThenMaxEvaluation).Build();

            //assert
            Assert.AreEqual(_eventId, newEventMoreThenMaxEvaluation.Id);
            Assert.AreEqual(creatorId, newEventMoreThenMaxEvaluation.CreatorId);
            Assert.AreEqual(date, newEventMoreThenMaxEvaluation.HappensDate);
            Assert.AreEqual(title, newEventMoreThenMaxEvaluation.Title);
            Assert.AreEqual(expectedEvaluation, newEventMoreThenMaxEvaluation.Evaluation);
        }
    }
}