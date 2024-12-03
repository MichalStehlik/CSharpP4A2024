using TcpChat.Server;
using TcpChat.Utils;

// See https://aka.ms/new-console-template for more information
ChatServer server = new();
server.StartAsync().Wait();
