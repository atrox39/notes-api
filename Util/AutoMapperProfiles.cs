using AutoMapper;
using Notes.Data.Models;
using Notes.Data.DTOs;
using Notes.Data.DTOs.Note;

namespace Notes.Utils {
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      // User Mapper
      CreateMap<User, UserDto>();
      CreateMap<RegisterDto, User>();
      // Note Mapper
      CreateMap<Note, NoteDto>();
      CreateMap<NoteCreateDto, Note>();
      CreateMap<NoteUpdateDto, Note>();
    }
  }
}
