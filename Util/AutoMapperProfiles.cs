using AutoMapper;
using notes.Models;
using notes.DTOs;
using notes.DTOs.Note;

namespace notes.Utils {
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      // User Mapper
      CreateMap<Users, UserDTO>();
      CreateMap<RegisterDto, Users>();
      // Note Mapper
      CreateMap<Note, NoteDto>();
      CreateMap<NoteCreateDto, Note>();
      CreateMap<NoteUpdateDto, Note>();
    }
  }
}
