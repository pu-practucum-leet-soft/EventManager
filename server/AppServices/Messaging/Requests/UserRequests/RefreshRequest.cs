﻿namespace EventManager.AppServices.Messaging.Requests.UserRequests
{
    public sealed class RefreshRequest
    {
        public string RefreshToken { get; set; } = default!;
    }
}
