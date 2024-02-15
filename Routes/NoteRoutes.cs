using Microsoft.AspNetCore.Http.HttpResults;
using notes.Repository;
using notes.DTOs.Note;
using FluentValidation;
using AutoMapper;
using notes.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using notes.Utils;
using System.Net.Http;

namespace notes.Routes
{
  public static class NoteRoutes
  {
    public static RouteGroupBuilder MapNotes(this RouteGroupBuilder group)
    {
      group.MapPost("/", Create);
      group.MapGet("/", ListAll);
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
      HttpContext httpContext
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
      HttpContext httpContext
    )
    {
      int userID = Methods.GetUserID(httpContext);
      bool result = await noteRepository.UpdateById(id, userID, mapper.Map<Note>(noteUpdateDto));
      if (result)
      {
        return TypedResults.NoContent();
      }
      return TypedResults.BadRequest();
    }

    private static async Task<Ok> DeleteByID(
      int id,
      INoteRepository noteRepository,
      HttpContext httpContext
    )
    {
      int userID = Methods.GetUserID(httpContext);
      await noteRepository.DeleteById(id, userID);
      return TypedResults.Ok();
    }
  }
}
