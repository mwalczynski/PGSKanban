using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PgsKanban.Hubs.Interfaces;

namespace PgsKanban.Hubs.Hubs
{
    public class CardDetailsHub: Hub<ICardDetailsClientHandler>
    {
        public Task JoinGroup(int cardId)
        {
            return Groups.AddAsync(Context.ConnectionId, cardId.ToString());
        }

        public Task LeaveGroup(int cardId)
        {
            return Groups.RemoveAsync(Context.ConnectionId, cardId.ToString());
        }
    }
}
