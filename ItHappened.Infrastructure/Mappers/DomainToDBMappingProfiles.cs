﻿using AutoMapper;
using ItHappened.Domain;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Infrastructure.Mappers
{
    public class DomainToDbMappingProfiles : Profile
    {
        public DomainToDbMappingProfiles()
        {
            CreateMap<User, UserDto>();

            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(
                        src => src.CustomizationsParameters.Comment.Match(c => c.Text, () => null)))
                .ForMember(dest => dest.Photo, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.Photo.ValueUnsafe()))
                .ForMember(dest => dest.Rating, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.Rating.ValueUnsafe()))
                .ForMember(dest => dest.Scale, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.Scale.ValueUnsafe()))
                .ForMember(dest => dest.LatitudeGeo, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLat))
                .ForMember(dest => dest.LongitudeGeo, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLng));


            CreateMap<EventTracker, EventTrackerDto>()
                .ForMember(dest => dest.ScaleMeasurementUnit, opt => opt.MapFrom(
                    src => src.CustomizationSettings.ScaleMeasurementUnit.Match(measuringUnit => measuringUnit,
                        () => null)))
                .ForMember(dest => dest.IsCommentRequired, opt => opt.MapFrom(
                    src => src.CustomizationSettings.IsCommentRequired))
                .ForMember(dest => dest.IsGeotagRequired, opt => opt.MapFrom(
                    src => src.CustomizationSettings.IsGeotagRequired))
                .ForMember(dest => dest.IsPhotoRequired, opt => opt.MapFrom(
                    src => src.CustomizationSettings.IsPhotoRequired))
                .ForMember(dest => dest.IsRatingRequired, opt => opt.MapFrom(
                    src => src.CustomizationSettings.IsRatingRequired))
                .ForMember(dest => dest.IsScaleRequired, opt => opt.MapFrom(
                    src => src.CustomizationSettings.IsScaleRequired))
                .ForMember(dest => dest.IsCustomizationRequired, opt => opt.MapFrom(
                    src => src.CustomizationSettings.IsCustomizationRequired));
        }
    }
}