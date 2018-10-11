using System;
using Infrastructure.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public static class SecurityService
    {
        public static bool IsAuthed(this HttpContext context, out Guid userId)
        {
            return Guid.TryParse(context.User.Identity.Name, out userId);
        }
    }
}
