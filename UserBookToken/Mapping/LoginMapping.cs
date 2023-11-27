using AutoMapper;
using UserBookToken.Entities;

namespace UserBookToken.Mapping
{
    public class LoginMapping : Profile
    {
        public LoginMapping()
        {
            CreateMap<LoginMapping,AppUser>();
        }
    }
}
