using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;

namespace PicturesAPI.Services;

public static class DbManagementService
{
    public static void UpdateDb(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var db = serviceScope.ServiceProvider.GetRequiredService<PictureDbContext>();
        db.Database.Migrate();
        db.SaveChanges();
    }
}