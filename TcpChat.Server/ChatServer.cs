using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TcpChat.Utils;

namespace TcpChat.Server
{
    internal class ChatServer
    {
        private readonly TcpListener _listener;
        private readonly ConcurrentDictionary<string, (TcpClient Client, StreamWriter Writer)> _clients = new();

        public ChatServer(string? ip = null, int port = 6464)
        {
            if (String.IsNullOrEmpty(ip))
            {
                ip = Utils.Utils.GetLocalIPAddress();
                ip = "127.0.0.1";
            }
            _listener = new TcpListener(System.Net.IPAddress.Parse(ip), port);
        }

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine("Server started");
            Console.WriteLine($"Listening on {_listener.LocalEndpoint}");
            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            Console.WriteLine("New client connected");
            var stream = client.GetStream();
            using var reader = new StreamReader(stream, new UTF8Encoding(false)); // new UTF8Encoding(false) = Encoding.UTF8 
            using var writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };
            string? userName = null;
            try
            {
                userName = await reader.ReadLineAsync();
                Console.WriteLine($"New connection from {userName}");
                if (String.IsNullOrEmpty(userName) || _clients.ContainsKey(userName))
                {
                    Console.WriteLine($"Invalid or already used username: {userName}");
                    client.Close();
                    return;
                }
                _clients.TryAdd(userName, (client, writer));
                Console.WriteLine($"{userName} joined");
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var message = JsonSerializer.Deserialize<Message>(line);
                    if (message == null)
                    {
                        Console.WriteLine("Invalid message");
                        continue;
                    }
                    Console.WriteLine($"Received message from {userName}: {message.Content}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
