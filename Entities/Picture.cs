﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PicturesAPI.Entities;

public class Picture
{
    [Key]
    public Guid Id { get; set; }
        
    [Required]
    public virtual Account Account { get; set; }
        
    public Guid AccountId { get; set; }
        
    [Required] [MinLength(4)] [MaxLength(40)]
    public string Name { get; set; }
        
    [MaxLength(500)]
    public string Description { get; set; }
        
    [MaxLength(500)]
    public string Tags { get; set; }
        
    [Comment("Picture URL")]
    [Required] [MaxLength(500)]
    public string Url { get; set; }
    public DateTime PictureAdded { get; set; } = DateTime.Now;
        
    public virtual List<Like> Likes { get; set; }
    
}