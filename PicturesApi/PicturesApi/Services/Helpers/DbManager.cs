using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;

namespace PicturesAPI.Services.Helpers;

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