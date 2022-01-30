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
    
    [MaxLength(2)]
    public int RoleId { get; set; } = 1;
    

    [AllowNull] 
    public virtual List<Picture> Pictures { get; set; }
    [AllowNull]
    public virtual List<Like> Likes { get; set; }

    public bool IsDeleted { get; set; } = false;
    
    public DateTime AccountCreated { get; set; } = DateTime.Now;

}