using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using PooPosting.Domain.DbContext.Entities.Joins;

namespace PooPosting.Domain.DbContext.Entities;

public class Account
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(16)]
    public string Nickname { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;
    
    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = null!;
    
    [Required] 
    [DefaultValue(false)] 
    public bool Verified { get; set; }
    
    [Required]
    public DateTime AccountCreated { get; set; } = DateTime.Now;
    
    [Required]
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
    
    [Required]
    [DefaultValue(1)]
    public int RoleId { get; set; }
    
    [AllowNull]
    public string ProfilePicUrl { get; set; }
    
    [AllowNull] 
    [MaxLength(64)] 
    public string RefreshToken { get; set; }
    
    [AllowNull] 
    public DateTime? RefreshTokenExpires { get; set; }
    
    

    public virtual Role Role { get; set; } = null!;
    
    [AllowNull] 
    public virtual ICollection<Picture> Pictures { get; set; }
    
    [AllowNull] 
    public virtual ICollection<Like> Likes { get; set; }
    
    [AllowNull] 
    public virtual ICollection<Comment> Comments { get; set; }
    
    [AllowNull] 
    public virtual ICollection<PictureSeenByAccount> PicturesSeen { get; set; }
   
    [AllowNull] 
    public virtual ICollection<AccountLikedTag> LikedTags { get; set; }
}