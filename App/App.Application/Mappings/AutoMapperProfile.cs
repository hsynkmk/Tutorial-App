using App.Application.DTOs;
using App.Domain.Entities;
using AutoMapper;

namespace App.Application.Mappings;

internal class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User Mapping
        CreateMap<ApplicationUser, UserDto>()
            .ReverseMap();

        // Course Mapping
        CreateMap<Course, CourseDto>()
            .ReverseMap();

        // Order Mapping
        CreateMap<Order, OrderDto>()
            .ReverseMap();

        // OrderDetail Mapping
        CreateMap<OrderDetail, OrderDetailDto>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name));

        // CartItem Mapping
        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name));
    }
}
