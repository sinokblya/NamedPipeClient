using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

class Program
{
    static void Main()
    {
        using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "MyPipe", PipeDirection.InOut))
        {
            Console.WriteLine("Подключение к серверу...");
            pipeClient.Connect();

            Console.WriteLine("Сервер подключен.");

            // Отправка данных серверу
            string message = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            pipeClient.Write(buffer, 0, buffer.Length);

            pipeClient.WaitForPipeDrain();

            // Чтение ответа от сервера
            byte[] responseBuffer = new byte[256];
            int bytesRead = pipeClient.Read(responseBuffer, 0, responseBuffer.Length);

            string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
            Console.WriteLine("Получено от сервера: " + response);
        }

        Console.WriteLine("Клиент завершил работу.");
    }
}
