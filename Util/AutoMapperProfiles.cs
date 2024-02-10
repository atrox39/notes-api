using AutoMapper;
using notes.Models;
using notes.DTOs;

namespace notes.Utils {
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      // User Mapper
      CreateMap<Users, UserDTO>();
      CreateMap<RegisterDto, Users>();
    }
  }
}
