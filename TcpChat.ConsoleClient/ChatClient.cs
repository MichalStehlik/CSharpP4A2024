using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TcpChat.Utils;

namespace TcpChat.ConsoleClient
{
    public class ChatClient
    {
        private readonly string _serverIp;
        private readonly int _serverPort;
        private TcpClient? _client;
        private StreamReader? _reader;
        private StreamWriter? _writer;

        public ChatClient(string serverIp, int serverPort)
        {
            _serverIp = serverIp;
            _serverPort = serverPort;
        }

        public async Task StartAsync()
        {
            Console.WriteLine("Enter your username:");
            var userName = Console.ReadLine();
            if (String.IsNullOrEmpty(userName))
            {
                Console.WriteLine("Invalid username");
                return;
            }
            try
            {
                _client = new TcpClient(_serverIp, _serverPort);
                var stream = _client.GetStream();
                _reader = new StreamReader(stream, new UTF8Encoding(false));
                _writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };
                await _writer.WriteLineAsync(userName);
                Console.WriteLine("Connected to server");
                var receiveTask = ReceiveMessagesAsync();
                var sendtask = SendMessagesAsync();
                await Task.WhenAll(receiveTask, sendtask);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (_client != null)
                {
                    _client.Close();
                }
            }
        }

        private async Task ReceiveMessagesAsync()
        {

        }

        public async Task SendMessagesAsync()
        {
            if (_writer == null)
            {
                Console.WriteLine("Not connected to server");
                return;
            }
            try
            {
                while (true)
                {
                    string? messageContent = Console.ReadLine();
                    if (String.IsNullOrEmpty(messageContent))
                    {
                        continue;
                    }
                    Message message = new Message { Content = messageContent };
                    await _writer.WriteLineAsync(JsonSerializer.Serialize(message));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending message:" + ex.Message);
            }
        }
    }
}
