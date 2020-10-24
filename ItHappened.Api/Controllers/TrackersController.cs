﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using ItHappened.Api.Authentication;
using ItHappened.Api.Models.Requests;
using ItHappened.Api.Models.Responses;
using ItHappened.Application.Services.StatisticService;
using ItHappened.Application.Services.TrackerService;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using LanguageExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class TrackersController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        private readonly ITrackerService _trackerService;
        private readonly ITrackerRepository _trackerRepository;
        private readonly IMapper _mapper;
        
        public TrackersController(ITrackerService trackerService,
            ITrackerRepository trackerRepository,
            IStatisticsService statisticsService,
            IMapper mapper)
        {
            _trackerService = trackerService;
            _trackerRepository = trackerRepository;
            _statisticsService = statisticsService;
            _mapper = mapper;
        }
        
        [HttpPost("/trackers")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult CreateTracker([FromBody]TrackerRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            //var customizations = _mapper.Map<TrackerCustomizationsSettings>(request.CustomizationSettings);
            var customizations = new TrackerCustomizationSettings(
                request.CustomizationSettings.ScaleMeasurementUnit,
                request.CustomizationSettings.PhotoIsOptional,
                request.CustomizationSettings.RatingIsOptional,
                request.CustomizationSettings.GeoTagIsOptional,
                request.CustomizationSettings.CommentIsOptional);
            
            var tracker = new EventTracker(Guid.NewGuid(), userId, request.Name, customizations);
            _trackerRepository.SaveTracker(tracker);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }
        

        [HttpGet("/trackers")]
        [ProducesResponseType(200, Type = typeof(List<TrackerResponse>))]
        public IActionResult GetAllTrackers()
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var trackers = _trackerService.GetEventTrackers(userId);
            return Ok(_mapper.Map<List<TrackerResponse>>(trackers));
        }
        
        [HttpGet("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult GetTracker([FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var tracker = _trackerService.GetEventTracker(userId, trackerId);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }
        
        [HttpPut("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult UpdateTracker([FromRoute]Guid trackerId, [FromBody]TrackerRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            //TODO REFACTOR MAPPING
            var customizations = new TrackerCustomizationSettings(
                request.CustomizationSettings.ScaleMeasurementUnit,
                request.CustomizationSettings.PhotoIsOptional,
                request.CustomizationSettings.RatingIsOptional,
                request.CustomizationSettings.GeoTagIsOptional,
                request.CustomizationSettings.CommentIsOptional);
            
            var tracker = _trackerService.EditEventTracker(userId, trackerId, request.Name, customizations);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }

        [HttpDelete("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult DeleteTracker([FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var deletedTracker = _trackerService.DeleteEventTracker(userId, trackerId);
            return Ok(_mapper.Map<TrackerResponse>(deletedTracker));
        }
    }
}