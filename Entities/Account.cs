using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PicturesAPI.Entities.Joins;
using PicturesAPI.Models.Interfaces;

namespace PicturesAPI.Entities;

public class Account: IDeletable
{
    [Key]
    public int Id { get; set; }

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


    [Required]
    public bool Verified { get; set; } = false;

    [AllowNull]
    [MaxLength(250)]
    public string ProfilePicUrl { get; set; }

    [AllowNull]
    [MaxLength(250)]
    public string BackgroundPicUrl { get; set; }

    [AllowNull]
    [MaxLength(500)]
    public string AccountDescription { get; set; }


    public int RoleId { get; set; } = 1;
    public Role Role { get; set; }
    
    public DateTime AccountCreated { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; } = false;


    [AllowNull]
    public virtual ICollection<Picture> Pictures { get; set; }
    [AllowNull]
    public virtual ICollection<Like> Likes { get; set; }
    [AllowNull]
    public virtual ICollection<Comment> Comments { get; set; }

    public virtual ICollection<PictureSeenByAccountJoin> PictureAccountJoins { get; set; }
    public virtual ICollection<AccountLikedTagsJoin> AccountLikedTagJoins { get; set; }

}