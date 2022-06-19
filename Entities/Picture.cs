using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities.Interfaces;

namespace PicturesAPI.Entities;

public class Picture: IDeletable
{
    [Key]
    public int Id { get; set; }
    public int AccountId { get; set; }
        
    [Required] [MinLength(4)] [MaxLength(40)]
    public string Name { get; set; }
        
    [MaxLength(500)]
    public string Description { get; set; }
        
    [Comment("Picture URL")]
    [Required] [MaxLength(250)]
    public string Url { get; set; }
    public DateTime PictureAdded { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;


    // navigation props
        
    public virtual ICollection<Like> Likes { get; set; }
    
    public virtual ICollection<Comment> Comments { get; set; }
    
    [Required]
    public virtual Account Account { get; set; }

    public ICollection<PictureTagJoin> PictureTagJoins { get; set; }
    public ICollection<PictureSeenByAccountJoin> PictureAccountJoins { get; set; }
}