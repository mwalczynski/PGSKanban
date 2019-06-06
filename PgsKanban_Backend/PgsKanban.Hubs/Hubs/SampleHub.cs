using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PgsKanban.Hubs.Interfaces;

namespace PgsKanban.Hubs.Hubs
{
    [Authorize]
    public class SampleHub : Hub<ISampleClientHandler>
    {
        public override Task OnConnectedAsync()
        {
            return Clients.All.SayHello("Hello World");
        }
    }
}
