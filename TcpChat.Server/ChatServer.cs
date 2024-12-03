using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
            var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            string? userName = null;
            try
            {
                userName = await reader.ReadLineAsync();
                if (String.IsNullOrEmpty(userName) || _clients.ContainsKey(userName))
                {
                    Console.WriteLine($"Invalid or already used username: {userName}");
                    client.Close();
                    return;
                }
                _clients.TryAdd(userName, (client, writer));
                Console.WriteLine($"{userName} joined");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
