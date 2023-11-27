using AutoMapper;
using UserBookToken.DTO;
using UserBookToken.Entities;

namespace UserBookToken.Mapping
{
    public class UserCreateMapping : Profile
    {
        public UserCreateMapping()
        {
            CreateMap<CreateUserDto, AppUser>();
        }
    }
}
