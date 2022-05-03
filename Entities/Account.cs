using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PicturesAPI.Entities;

public class Account
{
    [Key] 
    public Guid Id { get; set; }
        
    [Required] 
    [MinLength(4)] 
    [MaxLength(25)]
    public string Nickname { get; set; }
        
    [Required] 
    [MinLength(8)] 
    [MaxLength(40)]
    public string Email { get; set; }
        
    [Required] 
    [MaxLength(500)]
    public string PasswordHash { get; set; }
    
    [MaxLength(500)]
    public string LikedTags { get; set; } = "";

    // [Required]
    // public bool Verified { get; set; } = false;

    [MaxLength(2)]
    public int RoleId { get; set; } = 1;
    
    [AllowNull] 
    public virtual ICollection<Picture> Pictures { get; set; }
    [AllowNull]
    public virtual ICollection<Like> Likes { get; set; }
    [AllowNull] 
    public virtual ICollection<Comment> Comments { get; set; }

    public bool IsDeleted { get; set; } = false;
    
    public DateTime AccountCreated { get; set; } = DateTime.Now;

}