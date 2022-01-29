using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PicturesAPI.Entities;

public class Account
{
    [Key] 
    public Guid Id { get; set; }
        
    [Required] [MinLength(4)] [MaxLength(25)]
    public string Nickname { get; set; }
        
    [Required] [MinLength(8)] [MaxLength(40)]
    public string Email { get; set; }
        
    [Required] [MinLength(8)]
    public string PasswordHash { get; set; }

    [AllowNull] 
    public virtual List<Picture> Pictures { get; set; }

    public string LikedTags { get; set; } = "";
    
    public DateTime AccountCreated { get; set; } = DateTime.Now;

    public int RoleId { get; set; } = 1;

    public virtual List<Like> Likes { get; set; }

    public bool IsDeleted { get; set; } = false;

}