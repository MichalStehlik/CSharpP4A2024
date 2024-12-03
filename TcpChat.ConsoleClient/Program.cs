// See https://aka.ms/new-console-template for more information
using TcpChat.ConsoleClient;

ChatClient client = new();
client.ConnectAsync().Wait();