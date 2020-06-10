﻿using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace SignalChat.Libs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
