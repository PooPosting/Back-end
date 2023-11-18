using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using PooPosting.Api.Entities.Joins;

namespace PooPosting.Api.Entities;

public class Account
{
    public int Id { get; set; }
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool Verified { get; set; }
    public DateTime AccountCreated { get; set; }
    public bool IsDeleted { get; set; }
    public int RoleId { get; set; }
    [AllowNull] public string ProfilePicUrl { get; set; }
    [AllowNull] [MaxLength(64)] public string RefreshToken { get; set; }
    [AllowNull] public DateTime? RefreshTokenExpires { get; set; }

    public virtual Role Role { get; set; } = null!;
    [AllowNull] public virtual ICollection<Picture> Pictures { get; set; }
    [AllowNull] public virtual ICollection<Like> Likes { get; set; }
    [AllowNull] public virtual ICollection<Comment> Comments { get; set; }
    [AllowNull] public virtual ICollection<PictureSeenByAccount> PicturesSeen { get; set; }
    [AllowNull] public virtual ICollection<AccountLikedTag> LikedTags { get; set; }
}