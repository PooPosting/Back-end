using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Entities;

namespace PooPosting.Api.Services.Helpers;

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