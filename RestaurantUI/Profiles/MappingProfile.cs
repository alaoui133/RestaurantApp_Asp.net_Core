using AutoMapper;
using Restaurant.Models;
using Restaurant.Models.ViewModels;

namespace RestaurantUI.Profiles
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        { 
            CreateMap<MenuItem,MenuItemViewModel>().ReverseMap();
        }
    }
}
