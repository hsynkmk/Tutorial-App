using App.Application.DTOs.Course;
using App.Application.DTOs.Identity;
using App.Application.DTOs.Order;
using App.Domain.Entities;
using AutoMapper;

namespace App.Application.Mappings;

internal class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User Mapping
        {
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.CartItems, opt => opt.Ignore());
        }

        // Course Mapping
        CreateMap<Course, CourseDto>()
            .ReverseMap();

        // Order Mapping
        CreateMap<Order, OrderDto>()
            .ReverseMap();

        // OrderDetail Mapping
        CreateMap<OrderDetail, OrderDetailDto>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name));
    }
}
