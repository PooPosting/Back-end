using System;
using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Entities;

public class Like
{
    [Key]
    public int Id { get; set; }
    public Account Liker { get; set; }
    public Picture Liked { get; set; }
}