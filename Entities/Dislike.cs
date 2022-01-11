using System;
using System.ComponentModel.DataAnnotations;

namespace PicturesAPI.Entities;

public class Dislike
{
    [Key]
    public int Id { get; set; }
    public Account DisLiker { get; set; }
    public Picture DisLiked { get; set; }
}