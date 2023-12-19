using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Retail.Model;
using Retail.Repository;

namespace Retail.Utils
{
    public static class Extensions
    {
        public static ObjectResult HandleException(this Exception e, string? errorMessage = null)
        {
            var message = string.IsNullOrWhiteSpace(errorMessage) ? e.Message : errorMessage;

            return new ErrorResult(
                StatusCodes.Status500InternalServerError,
                new
                {
                    Status = StatusCodes.Status500InternalServerError,
                    message,
                    Data = new { UserData = e.Data, e.Message, e.StackTrace }
                });
        }

        public static IApplicationBuilder DbMigrate(this IApplicationBuilder app)
        {  
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
            var context = serviceScope?.ServiceProvider?.GetRequiredService<RetailDbContext>();
            if (context == null) return app;
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
          // context.Database.Migrate();
            return app;
        }
    }
}
