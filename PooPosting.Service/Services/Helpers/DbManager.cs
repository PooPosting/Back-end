using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PooPosting.Data.DbContext;

namespace PooPosting.Service.Services.Helpers;

public static class DbManager
{
    public static void UpdateDb(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var db = serviceScope.ServiceProvider.GetRequiredService<PictureDbContext>();
        db.Database.Migrate();
        db.SaveChanges();
    }
}