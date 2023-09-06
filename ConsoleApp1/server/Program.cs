using System.Net;
using System.Net.Sockets;
using System.Text;

const string serverIp = "127.0.0.1";   // localhost
const int port = 8080;

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverIp), port);

try
{
    socket.Bind(endPoint);
    socket.Listen(10);

    Console.WriteLine($"Server started at {serverIp}:{port}");

    await RunListenAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}

async Task RunListenAsync()
{
    while (true)
    {
        Socket remoteSocket = await socket.AcceptAsync();

        if (remoteSocket.RemoteEndPoint is IPEndPoint remoteEP)
        {
            Console.WriteLine($"Connection opened for remote --> {remoteEP.Address}:{remoteEP.Port}");
        }

        _ = Task.Run(() => HandleRequest(remoteSocket));
    }
}

void HandleRequest(Socket remoteSocket)
{
    byte[] buffer = new byte[256];
    int byteCount = 0;
    string message = string.Empty;
    DateTime currentTime = DateTime.Now;
    do
    {
        byteCount = remoteSocket.Receive(buffer);               // BLOCKING (Ожидание данных в сокете для чтения)
        message += Encoding.UTF8.GetString(buffer, 0, byteCount);

    } while (remoteSocket.Available > 0);

    Console.WriteLine($"{DateTime.Now.ToShortTimeString()} > {message}");

    //Thread.Sleep(10000);

    string response = "Текущее время: " + currentTime.ToString("HH:mm:ss");
    remoteSocket.Send(Encoding.UTF8.GetBytes(response));

    remoteSocket.Shutdown(SocketShutdown.Both);
    remoteSocket.Close();
    Console.WriteLine("Connection closed\n");
}