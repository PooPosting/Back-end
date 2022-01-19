using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.OData.Edm;

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
    
    [AllowNull]
    public string LikedTags { get; set; }
    
    public DateTime AccountCreated { get; set; } = DateTime.Now;
        
    [DefaultValue(1)]
    public int RoleId { get; set; }

    public virtual List<Like> Likes { get; set; }

    public virtual List<Dislike> Dislikes { get; set; }
}