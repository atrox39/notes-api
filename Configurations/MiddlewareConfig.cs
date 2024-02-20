namespace Notes.Configurations
{
  public static class MiddlewareConfig
  {
    public static WebApplication MiddlewareAppConfig(this WebApplication app)
    {
      app.UseSwagger();
      app.UseSwaggerUI();
      app.UseCors(x => x
        .WithOrigins("http://localhost:5173") // React with Vite Config
        .AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials()
      );
      app.UseWebSockets();
      app.UseMiddleware<WebSocketController>();
      return app;
    }
  }
}