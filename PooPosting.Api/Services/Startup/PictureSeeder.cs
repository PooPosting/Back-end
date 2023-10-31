using PooPosting.Api.Entities;

namespace PooPosting.Api.Services.Startup;

public class PictureSeeder
{
    private readonly PictureDbContext _dbContext;

    public PictureSeeder(PictureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Seed(bool isDev)
    {
        
        if (_dbContext.Database.CanConnect())
        {
            if (!_dbContext.Roles.Any())
            {
                Console.WriteLine("Seeding the database...");
                var roles = GetRoles();
                _dbContext.Roles.AddRange(roles);
                _dbContext.SaveChanges();
            }

            if (isDev && !_dbContext.Accounts.Any())
            {
                var accounts = GetAccounts();
                _dbContext.Accounts.AddRange(accounts);

                _dbContext.SaveChanges();
            }
        }
            
    }
        
    private IEnumerable<Account> GetAccounts()
    {
        var accounts = new List<Account>()
        {
            new Account()
            {
                Id = 1,
                Nickname = "ShrekTheCreator",
                Email = "ILoveShrex@isMyPassword.IAmAdmin",
                PasswordHash = "AQAAAAEAACcQAAAAENlo2UxvxGMwuRoOfyF6ZT5jxmB9zXLrQnLYCQpnfb0R/+NudFjqsTc+YkzDm7G0sQ==",
                Pictures = new List<Picture>()
                {
                    new Picture()
                    {
                        Name = "Shrek",
                        Description = "Shrek",
                        Url =
                            "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Focs-pl.oktawave.com%2Fv1%2FAUTH_2887234e-384a-4873-8bc5-405211db13a2%2Fsplay%2F2018%2F11%2Fshrek-reboot-film.jpeg&f=1&nofb=1",
                        // Tags = "shrek green handsomeMan original",
                        PictureAdded = DateTime.Now,
                    },
                    new Picture()
                    {
                        Name = "Shrek stoned",
                        Description = "Shrek is stoned",
                        Url =
                            "http://3.bp.blogspot.com/_GoN5EPxM4Y8/S-3O8XQippI/AAAAAAAAAJI/HkXJaFXTr1g/w1200-h630-p-k-no-nu/shrek1ta5.jpg",
                        // Tags = "shrek 420 stoned green handsomeMan",
                        PictureAdded = DateTime.Now,
                    }
                },
                AccountCreated = DateTime.Now,
                RoleId = 3,
                BackgroundPicUrl = Path.Combine("wwwroot", "accounts", "background_pictures", $"default{new Random().Next(0, 5)}-bgp.webp"),
                ProfilePicUrl = Path.Combine("wwwroot", "accounts", "profile_pictures", $"default{new Random().Next(0, 5)}-pfp.webp"),
            },
            new Account()
            {
                Id = 2,
                Nickname = "ShrekTheManager",
                Email = "ILoveShrex@isMyPassword.IAmManager",
                PasswordHash = "AQAAAAEAACcQAAAAENlo2UxvxGMwuRoOfyF6ZT5jxmB9zXLrQnLYCQpnfb0R/+NudFjqsTc+YkzDm7G0sQ==",
                Pictures = new List<Picture>()
                {
                    new Picture()
                    {
                        Name = "Shrek Managing",
                        Description = "Shrek is managing things",
                        Url =
                            "https://eskipaper.com/images/shrek-5.jpg",
                        // Tags = "shrek green handsomeMan manager",
                        PictureAdded = DateTime.Now,
                    }
                },
                AccountCreated = DateTime.Now,
                RoleId = 3,
                BackgroundPicUrl = Path.Combine("wwwroot", "accounts", "background_pictures", $"default{new Random().Next(0, 5)}-bgp.webp"),
                ProfilePicUrl = Path.Combine("wwwroot", "accounts", "profile_pictures", $"default{new Random().Next(0, 5)}-pfp.webp"),
            },
            new Account()
            {
                Id = 3,
                Nickname = "ShrekTheUser",
                Email = "ILoveShrex@isMyPassword.IAmUser",
                PasswordHash = "AQAAAAEAACcQAAAAENlo2UxvxGMwuRoOfyF6ZT5jxmB9zXLrQnLYCQpnfb0R/+NudFjqsTc+YkzDm7G0sQ==",
                Pictures = new List<Picture>()
                {
                    new Picture()
                    {
                        Name = "Shrek on a mountain",
                        Description = "Shrek is on a mountain",
                        Url =
                            "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2F64.media.tumblr.com%2Fd32b6f7c291461fc12dda99c15d5fe1d%2Fd2859800230a5e2e-80%2Fs400x600%2F71db3c4f14aeacfe654cb9ada922648a59efb3d9.jpg&f=1&nofb=1",
                        // Tags = "shrek green mountain climbing",
                        PictureAdded = DateTime.Now,
                    },
                    new Picture()
                    {
                        Name = "Shrek with glasses",
                        Description = "Shrek wearing glasses",
                        Url = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fih0.redbubble.net%2Fimage.191779839.7433%2Fflat%2C1000x1000%2C075%2Cf.u1.jpg&f=1&nofb=1",
                        // Tags = "shrek green sun sunglasses glasses sexy",
                        PictureAdded = DateTime.Now,
                    }
                },
                AccountCreated = DateTime.Now,
                RoleId = 1,
                BackgroundPicUrl = Path.Combine("wwwroot", "accounts", "background_pictures", $"default{new Random().Next(0, 5)}-bgp.webp"),
                ProfilePicUrl = Path.Combine("wwwroot", "accounts", "profile_pictures", $"default{new Random().Next(0, 5)}-pfp.webp"),
            }
                
        };
        return accounts;
    }
        
    private IEnumerable<Role> GetRoles()
    {
        var roles = new List<Role>()
        {
            new Role()
            {
                Name = "User"
            },
            new Role()
            {
                Name = "Moderator"
            },
            new Role()
            {
                Name = "Administrator"
            }
        };
        return roles;
    }

}