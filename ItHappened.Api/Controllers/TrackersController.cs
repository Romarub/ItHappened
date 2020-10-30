﻿﻿using System;
using System.Collections.Generic;
 using AutoMapper;
 using ItHappened.Api.Models.Requests;
using ItHappened.Api.Models.Responses;
using ItHappened.Application.Services.TrackerService;
using ItHappened.Domain;
using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class TrackersController : ControllerBase
    {
        private readonly ITrackerService _trackerService;
        private readonly IMapper _mapper;

        public TrackersController(ITrackerService trackerService, IMapper mapper)
        {
            _trackerService = trackerService;
            _mapper = mapper;
        }
        
        [HttpPost("/trackers")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult CreateTracker([FromBody]TrackerRequest request)
        {
            var userId = User.GetUserId();
            var customizationSettings = _mapper.Map<TrackerCustomizationSettings>(request.CustomizationSettings);
            var tracker = _trackerService.CreateEventTracker(userId, request.Name, customizationSettings);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }
        

        [HttpGet("/trackers")]
        [ProducesResponseType(200, Type = typeof(List<TrackerResponse>))]
        public IActionResult GetAllTrackers()
        {
            var userId = User.GetUserId();
            var trackers = _trackerService.GetEventTrackers(userId);
            return Ok(_mapper.Map<List<TrackerResponse>>(trackers));
        }
        
        [HttpGet("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult GetTracker([FromRoute]Guid trackerId)
        {
            var userId = User.GetUserId();
            var tracker = _trackerService.GetEventTracker(userId, trackerId);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }
        
        [HttpPut("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult UpdateTracker([FromRoute]Guid trackerId, [FromBody]TrackerRequest request)
        {
            var userId = User.GetUserId();
            var customizations = _mapper.Map<TrackerCustomizationSettings>(request.CustomizationSettings);
            _trackerService.EditEventTracker(userId, trackerId, request.Name, customizations);
            return Ok();
        }

        [HttpDelete("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult DeleteTracker([FromRoute]Guid trackerId)
        {
            var userId = User.GetUserId();
            _trackerService.DeleteEventTracker(userId, trackerId);
            return Ok();
        }
    }
}