using Microsoft.AspNetCore.Http.HttpResults;
using notes.Repository;
using notes.DTOs.Note;
using FluentValidation;
using AutoMapper;
using notes.Models;
using notes.Utils;
using Microsoft.AspNetCore.OutputCaching;

namespace notes.Routes
{
  public static class NoteRoutes
  {
    public static RouteGroupBuilder MapNotes(this RouteGroupBuilder group)
    {
      group.MapPost("/", Create);
      group.MapGet("/", ListAll).CacheOutput(x => x.Expire(TimeSpan.FromMinutes(30)).Tag("Note-All"));
      group.MapGet("/{id:int}", GetByID);
      group.MapPut("/{id:int}", UpdateByID);
      group.MapDelete("/{id:int}", DeleteByID);
      return group;
    }

    private static async Task<Results<Created<NoteDto>, BadRequest, ValidationProblem>> Create(
      NoteCreateDto noteCreateDto,
      IMapper mapper,
      INoteRepository noteRepository,
      IValidator<NoteCreateDto> validator,
      HttpContext httpContext,
      IOutputCacheStore cacheStore
    )
    {
      var results = await validator.ValidateAsync(noteCreateDto);
      if (!results.IsValid)
      {
        return TypedResults.ValidationProblem(results.ToDictionary());
      }
      int userID = Methods.GetUserID(httpContext);
      var note = await noteRepository.Create(mapper.Map<Note>(noteCreateDto), userID);
      if (note == null)
      {
        return TypedResults.BadRequest();
      }
      await cacheStore.EvictByTagAsync("Note-All", default);
      return TypedResults.Created($"/notes/{note.Id}", mapper.Map<NoteDto>(note));
    }

    private static async Task<Ok<List<NoteDto>>> ListAll(
      INoteRepository noteRepository,
      IMapper mapper,
      HttpContext httpContext
    )
    {
      var notes = await noteRepository.ListAll(Methods.GetUserID(httpContext));
      var notesDto = notes.Select(n => mapper.Map<NoteDto>(n)).ToList();
      return TypedResults.Ok(notesDto);
    }

    private static async Task<Results<Ok<NoteDto>, NotFound>> GetByID(
      int id,
      INoteRepository  noteRepository,
      IMapper mapper,
      HttpContext httpContext
    )
    {
      var note = await noteRepository.GetById(id, Methods.GetUserID(httpContext));
      if (note == null)
      {
        return TypedResults.NotFound();
      }
      return TypedResults.Ok(mapper.Map<NoteDto>(note));
    }

    private static async Task<Results<NoContent, BadRequest>> UpdateByID(
      int id,
      NoteUpdateDto noteUpdateDto,
      IMapper mapper,
      INoteRepository noteRepository,
      HttpContext httpContext,
      IOutputCacheStore cacheStore
    )
    {
      int userID = Methods.GetUserID(httpContext);
      bool result = await noteRepository.UpdateById(id, userID, mapper.Map<Note>(noteUpdateDto));
      if (result)
      {
        await cacheStore.EvictByTagAsync("Note-All", default);
        return TypedResults.NoContent();
      }
      return TypedResults.BadRequest();
    }

    private static async Task<Ok> DeleteByID(
      int id,
      INoteRepository noteRepository,
      HttpContext httpContext,
      IOutputCacheStore cacheStore
    )
    {
      int userID = Methods.GetUserID(httpContext);
      await noteRepository.DeleteById(id, userID);
      await cacheStore.EvictByTagAsync("Note-All", default);
      return TypedResults.Ok();
    }
  }
}
