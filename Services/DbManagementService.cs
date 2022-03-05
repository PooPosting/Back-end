using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities;

namespace PicturesAPI.Services;

public static class DbManagementService
{
    public static async void MigrationInit(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var db = serviceScope.ServiceProvider.GetRequiredService<PictureDbContext>();
        await db.Database.MigrateAsync();
        await db.SaveChangesAsync();

    }
}