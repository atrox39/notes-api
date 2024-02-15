namespace notes.Routes
{
  public static class ApiRoutes
  {
    public static RouteGroupBuilder MapApi(this RouteGroupBuilder group) {
      group.MapGroup("/auth").MapAuth();
      group.MapGroup("/notes").MapNotes().RequireAuthorization();
      return group;
    }
  }
}
