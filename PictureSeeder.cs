using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using PicturesAPI.Entities;

namespace PicturesAPI;

public class PictureSeeder
{
    private readonly PictureDbContext _dbContext;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public PictureSeeder(PictureDbContext dbContext, IPasswordHasher<Account> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public void Seed()
    {
        if (!_dbContext.Roles.Any())
        {
            var roles = GetRoles();
            _dbContext.Roles.AddRange(roles);
            _dbContext.SaveChanges();
        }
            
        if (_dbContext.Database.CanConnect())
        {
            if (!_dbContext.Accounts.Any())
            {
                var accounts = GetAccounts();
                _dbContext.Accounts.AddRange(accounts);

                var likes = GetLikes();
                // _dbContext.Likes.AddRange(likes);
                    
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
                        Tags = "Shrek Green HandsomeMan",
                        PictureAdded = DateTime.Now,
                        // Likes = new List<Like>(),
                        // Dislikes = new List<Dislike>()
                    },
                    new Picture()
                    {
                        Name = "Shrek stoned",
                        Description = "Shrek is stoned",
                        Url =
                            "http://3.bp.blogspot.com/_GoN5EPxM4Y8/S-3O8XQippI/AAAAAAAAAJI/HkXJaFXTr1g/w1200-h630-p-k-no-nu/shrek1ta5.jpg",
                        Tags = "Shrek 420 Stoned Green HandsomeMan",
                        PictureAdded = DateTime.Now,
                        // Likes = new List<Like>(),
                        // Dislikes = new List<Dislike>()
                    }
                },
                AccountCreated = DateTime.Now,
                RoleId = 3
            },
            new Account()
            {
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
                        Tags = "Shrek Green HandsomeMan Manager",
                        PictureAdded = DateTime.Now,
                        // Likes = new List<Like>(),
                        // Dislikes = new List<Dislike>()
                    }
                },
                AccountCreated = DateTime.Now,
                RoleId = 2
            },
            new Account()
            {
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
                        Tags = "Shrek Green HandsomeMan Manager",
                        PictureAdded = DateTime.Now,
                        //     Likes = new List<Like>(),
                        //     Dislikes = new List<Dislike>()
                    },
                    new Picture()
                    {
                        Name = "Shrek with glasses",
                        Description = "Shrek wearing glasses",
                        Url = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fih0.redbubble.net%2Fimage.191779839.7433%2Fflat%2C1000x1000%2C075%2Cf.u1.jpg&f=1&nofb=1",
                        Tags = "Shrek Green HandsomeMan Manager",
                        PictureAdded = DateTime.Now,
                        // Likes = new List<Like>(),
                        // Dislikes = new List<Dislike>()
                    }
                },
                AccountCreated = DateTime.Now,
                RoleId = 1
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

    private IEnumerable<Like> GetLikes()
    {
        var likes = new List<Like>()
        {
            new Like()
            {
                Liked = GetAccounts().ToList()[0].Pictures.ToList()[0],
                Liker = GetAccounts().ToList()[0]
            }
        };
        return likes;
    }

}