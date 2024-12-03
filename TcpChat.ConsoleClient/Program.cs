// See https://aka.ms/new-console-template for more information

using TcpChat.ConsoleClient;

ChatClient client = new ChatClient("127.0.0.1", 6464);
client.StartAsync().Wait();