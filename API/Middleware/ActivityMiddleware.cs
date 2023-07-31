using API.Controllers;
using API.Data;
using API.Entities;
using API.Extensions;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;

namespace API.Middleware
{
    public class ActivityMiddleware
    {
        private readonly RequestDelegate _next;

        public ActivityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if(!context.User.Identity.IsAuthenticated) return;

            var userId = context.User.GetUserId();
            using (var dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>())
            {
                var user = dbContext.Users.Where(u => u.Id == userId).FirstOrDefault();
                user.LastActive = DateTime.Now;
                dbContext.SaveChanges();
            }
            
        }
    }
}