namespace notes
{
  public static class ApiRoutes
  {
    public static RouteGroupBuilder MapApi(this RouteGroupBuilder group) {
      group.MapGroup("/auth").MapAuth();
      return group;
    }
  }
}
