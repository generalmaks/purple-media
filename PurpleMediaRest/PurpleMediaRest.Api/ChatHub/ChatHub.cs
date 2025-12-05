using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Api.ChatHub;

[Authorize]
public class ChatHub(IChatService chatService) : Hub
{
    public async Task SendPrivateMessage(int targetUserId, string message)
    {
        string? sender = Context.UserIdentifier;

        bool parsedSender = Int32.TryParse(sender, out var senderId);

        if (!parsedSender)
            return;

        await chatService.SendMessageAsync(senderId, targetUserId, message);
        
        await Clients.User(targetUserId.ToString()).SendAsync("ReceiveMessage", senderId, message);
        await Clients.Caller.SendAsync("ReceiveMessage", senderId, message);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}