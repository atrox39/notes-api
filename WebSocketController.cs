using System.Net.WebSockets;
using System.Text;

namespace notes
{
  public class WebSocketController (RequestDelegate next)
  {
    public async Task Invoke(HttpContext httpContext)
    {
      if (!httpContext.WebSockets.IsWebSocketRequest)
      {
        await next.Invoke(httpContext);
        return;
      }
      var ct = httpContext.RequestAborted;
      var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
      var message = await ReceiveStringAsync(socket, ct);
      if (message == null) return;
      switch (message.ToLower())
      {
        case "test1":
          await SendStringAsync(socket, "Test Result OK", ct);
          break;
        case "close":
          await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnect", ct);
          break;
        default:
          await SendStringAsync(socket, "Uknown message", ct);
          break;
      }
    }

    private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default)
    {
      var buffer = new ArraySegment<byte>(new byte[8192]);
      var ms = new MemoryStream();
      WebSocketReceiveResult result;
      do {
        ct.ThrowIfCancellationRequested();
        result = await socket.ReceiveAsync(buffer, ct);
        ms.Write(buffer.Array!, buffer.Offset, result.Count);
      } while(!result.EndOfMessage);
      ms.Seek(0, SeekOrigin.Begin);
      if (result.MessageType != WebSocketMessageType.Text)
        throw new Exception("Error, uknown message");
      var reader = new StreamReader(ms, Encoding.UTF8);
      return await reader.ReadToEndAsync();
    }

    private static Task SendStringAsync(WebSocket socket, string message, CancellationToken ct = default)
    {
      var buffer = Encoding.UTF8.GetBytes(message);
      var segment = new ArraySegment<byte>(buffer);
      return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
    }
  }
}
