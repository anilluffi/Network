using System.Net;
using System.Net.Sockets;
using System.Text;

const string serverIp = "127.0.0.1";   // localhost
const int serverPort = 8080;

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

try
{
    while(true)
    {
        clientF();


    }
    
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");

}

Console.ReadLine();



void clientF()
{
    Console.Write("Do you want to know the time? (yes/no) ");
    string? message = Console.ReadLine();

    if (message == "y" || message == "yes")
    {
        socket.Connect(serverEP);                       // BLOCKING
        socket.Send(Encoding.UTF8.GetBytes(message));   // BLOCKING

        byte[] buffer = new byte[256];
        int byteCount = 0;
        string response = string.Empty;

        do
        {
            byteCount = socket.Receive(buffer);               // BLOCKING (Ожидание данных в сокете для чтения)
            response += Encoding.UTF8.GetString(buffer, 0, byteCount);

        } while (socket.Available > 0);

        Console.WriteLine($"RESPONSE: {response}");

        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
    else if (message == "n" || message == "no")
    {
        Console.Write("Are you sure you don't want to know the time? (yes/no) ");
        message = Console.ReadLine();
        if (message == "y" || message == "yes")
        { 
            clientF(); 
        }
        else if (message == "n" || message == "no")
        {
            Console.WriteLine("You need to think carefully about whether or not you want to see the time. I'll give you time to think");
            Thread.Sleep(10000);
            clientF();
        }
        else { Console.WriteLine("Incorrect input"); }
    }
    else { Console.WriteLine("Incorrect input"); }
}