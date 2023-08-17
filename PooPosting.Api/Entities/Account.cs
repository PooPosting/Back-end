using System.Diagnostics.CodeAnalysis;
using PooPosting.Api.Entities.Joins;

namespace PooPosting.Api.Entities;

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

    public virtual Role Role { get; set; }
    [AllowNull] public virtual ICollection<Picture> Pictures { get; set; }
    [AllowNull] public virtual ICollection<Like> Likes { get; set; }
    [AllowNull] public virtual ICollection<Comment> Comments { get; set; }
    [AllowNull] public virtual ICollection<PictureSeenByAccount> PicturesSeen { get; set; }
    [AllowNull] public virtual ICollection<AccountLikedTag> LikedTags { get; set; }
}