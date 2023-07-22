using AutoMapper;

using thu6_pvo_dictionary.Models.Entity;
using thu6_pvo_dictionary.Models.Request;

namespace thu6_pvo_dictionary.Mapper
{
    public class MappingContext : Profile
    {
        public MappingContext()
        {
            // user request
            CreateMap<UserRegisterRequest, User>();
            CreateMap<UserStoreRequest, User>();
        }
    }
}
