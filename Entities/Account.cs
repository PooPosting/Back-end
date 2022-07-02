using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using PicturesAPI.Entities.Joins;

namespace PicturesAPI.Entities;

public class Account
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool Verified { get; set; }
    [AllowNull] public string ProfilePicUrl { get; set; }
    [AllowNull] public string BackgroundPicUrl { get; set; }
    [AllowNull] public string AccountDescription { get; set; }
    public DateTime AccountCreated { get; set; }
    public bool IsDeleted { get; set; }
    public int RoleId { get; set; }

    public Role Role { get; set; }
    [AllowNull] public virtual IEnumerable<Picture> Pictures { get; set; }
    [AllowNull] public virtual IEnumerable<Like> Likes { get; set; }
    [AllowNull] public virtual IEnumerable<Comment> Comments { get; set; }
    [AllowNull] public virtual IEnumerable<PictureSeenByAccount> PicturesSeen { get; set; }
    [AllowNull] public virtual IEnumerable<AccountLikedTags> LikedTags { get; set; }
}